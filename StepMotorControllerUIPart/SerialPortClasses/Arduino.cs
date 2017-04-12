using System;
using System.IO;
using System.IO.Ports;
using NLog;
using RADON.SerialPortClasses;

namespace StepMotorControllerUIPart.SerialPortClasses
{
    public class Arduino : IArduino
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private SerialPort _serialPort;

        private string _comPort;

        public Arduino(string comPort)
        {
            _comPort = comPort;
        }

        public  bool IsConnected()
        {
            return _serialPort != null;
        }

        public  void Connect()
        {
            if (_serialPort == null)
            {
                _serialPort = new SerialPort(_comPort, 9600, Parity.None, 8, StopBits.One);
                try
                {
                    logger.Debug("Trying to open serial port: {0}",_comPort);
                    _serialPort.Open();
                }
                catch (IOException e)
                {
                    logger.Error(e);
                    throw e;
                }
            }
            else
            {
                logger.Debug("Port already open {0}", _comPort);
            }     
        }

        public void Disconnect()
        {
            if (_serialPort != null)
            {
                try
                {
                    logger.Debug("Closing Serial Port");
                    _serialPort.Close();
                }
                catch (IOException e)
                {
                    logger.Error(e);
                    throw e;
                }

            }
        }

        public static string[] GetAvaiblePorts()
        {
            return SerialPort.GetPortNames();
        }

        public void MakeOneStep()
        {
            makeStep(1);
        }
        public void MakeSteps(byte steps)
        {
            makeStep(steps);
        }

        private void makeStep(byte steps)
        {
            if (IsConnected())
            {
                try
                {
                    byte[] data = { 255, steps};
                    logger.Trace("Send request to SerialPort to make {0} steps", steps);
                    _serialPort.Write(data, 0, data.Length);
                }
                catch (Exception e)
                {
                    logger.Error(e);
                    throw e;
                }
            }
            else
            {
                logger.Debug("Port did not open");
            }

        }

    }
}
