using System;
using System.IO;
using System.IO.Ports;
using NLog;
using RADON.SerialPortClasses;
using StepMotorControllerUIPart.UsedTypes;

namespace StepMotorControllerUIPart.SerialPortClasses
{

    public class ModBus : IModBus
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private String _portName; 
        private int _baudRate;
        private SerialPort _serialPort;

        public ModBus()
        {
            _portName = "COM1";
            _baudRate =  9600;
            logger.Trace("ModBus default constructor create object");
        }
    
        public ModBus(String portName)
        {
            _portName = portName;
            _baudRate = 9600;
            logger.Trace("ModBus custom constructor create object");
        }

        public void Connect()
        {
            logger.Trace("if serial port = 0");
            if (_serialPort == null)            
            {
                _serialPort = new SerialPort(_portName, _baudRate, Parity.None, 8, StopBits.One);

                try
                {
                    logger.Trace("Oppened Serial Port");
                    _serialPort.Open();

                }
                catch (UnauthorizedAccessException e)
                {
                    logger.Error(e);
                    throw e;
                }
                catch (IOException e)
                {
                    logger.Error(e);
                    throw e;
                }
            }
  }

        public void Disconnect()
        {
            if (_serialPort != null)
            {
                try
                {
                    logger.Trace("Closed Serial Port");
                    _serialPort.Close();
                }
                catch (IOException e)
                {
                    logger.Error(e);
                    throw e;
                }      
            }
        }
        public bool IsConnected()
        {
            return _serialPort != null;
        }
        // my custom read
        public float Read(Adress adress)
        {
            logger.Trace("start read method {0} {1}", adress.AdcNumber, adress.ChannelNumber);
            return read(adress.AdcNumber, new byte[] { adress.ChannelNumber, 0 });
        }

        public float Read(byte adc, byte channel)
        {
            return read(adc, new byte[] { channel, 0 });
        }

        private float read(byte address, byte[] regNumber)
        {
            
            byte[] requestData = new byte[8];

            requestData[0] = address;
            requestData[1] = 3;
            requestData[2] = regNumber[0];
            requestData[3] = regNumber[1];
            requestData[4] = 0;
            requestData[5] = 2;
            byte[] requestCrc = CrcCalculator.GetCRC(requestData);
            requestData[6] = requestCrc[0];
            requestData[7] = requestCrc[1];
          
            // Responce for function 3
            // [0] Address  
            // [1] Function  
            // [2] Byte counter
            // [3] data1
            // [4] data2
            // [5] data3
            // [6] data4
            // [7] crc H
            // [8] crc L
            byte[] responceData = new byte[9];
            if (IsConnected())
            {
                try
                {
                    logger.Trace("Send request to SerialPort");
                    _serialPort.Write(requestData, 0, requestData.Length);
                }
                catch (IOException e)
                {
                    logger.Error(e);
                    throw e;
                }
                try
                {
                    for (int i = 0; i < responceData.Length; i++)
                    {
                        responceData[i] = (byte)(_serialPort.ReadByte());
                        logger.Trace("Responce from Serial, byte={0}",responceData[i]);
                    }
                }
                catch (TimeoutException e)
                {
                    logger.Error(e);
                    throw e;
                }
                catch (IOException e)
                {
                    logger.Error(e);
                    throw new Exception("Error while getting response.", e);
                }
            }

            byte[] resultBytes = new byte[4];
            byte[] revertResultBytes = new byte[4];

            resultBytes = copyOfRange(responceData, 3, 7);

            revertResultBytes[3] = resultBytes[0];
            revertResultBytes[2] = resultBytes[1];
            revertResultBytes[1] = resultBytes[2];
            revertResultBytes[0] = resultBytes[3];

            var result = CommunicatorUtils.ParseFloat(revertResultBytes);
            logger.Trace("Read result: result={0}", result);

            return result;         
        }

        //include start value, does not include end value
        private  byte[] copyOfRange(byte[] src, byte start, byte end)
         {
             int len = end - start;
             byte[] dest = new byte[len];
             // note i is always from 0
             for (byte i = 0; i < len; i++)
             {
                 dest[i] = src[start + i]; // so 0..n = 0+x..n+x
             }
             return dest;
         }

        static class CommunicatorUtils
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();
            public static float ParseFloat(byte[] data)
            {
                float result;

                try
                {
                    logger.Trace("Start converting bits to float");
                    result = BitConverter.ToSingle(data, 0);
                }
                catch (ArgumentNullException e)
                {
                    logger.Error(e);
                    throw e;
                }
                return result;
            }
        }

        public static class CrcCalculator
        {
            public static byte[] GetCRC(byte[] message)
            {


                byte[] CRC = new byte[2];
                //Function expects a modbus message of any length as well as a 2 byte CRC array in which to 
                //return the CRC values:

                ushort CRCFull = 0xFFFF;
                byte CRCHigh = 0xFF, CRCLow = 0xFF;
                char CRCLSB;

                for (int i = 0; i < (message.Length) - 2; i++)
                {
                    CRCFull = (ushort)(CRCFull ^ message[i]);

                    for (int j = 0; j < 8; j++)
                    {
                        CRCLSB = (char)(CRCFull & 0x0001);
                        CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                        if (CRCLSB == 1)
                            CRCFull = (ushort)(CRCFull ^ 0xA001);
                    }
                }
                CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
                CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);

                return CRC;
            }
        }

    }
}
