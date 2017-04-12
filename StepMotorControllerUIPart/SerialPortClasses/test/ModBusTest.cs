using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using StepMotorControllerUIPart.UsedTypes;

namespace RADON.SerialPortClasses.test
{
    class ModBusTest : IModBus
    {
        public void Connect()
        {
        }

        public void Disconnect()
        {
        }

        public float Read(byte adc, byte channel)
        {
            var value = new Random();
            Thread.Sleep(10);
            return (float)value.NextDouble();
        }

        public float Read(Adress adress)
        {
            var value = new Random();
            Thread.Sleep(10);
            //double rDouble = (value.NextDouble()*2) - 1.0;
            //return (float) rDouble;
            return  (float) value.NextDouble();
        }
    }
}
