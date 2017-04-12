using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StepMotorControllerUIPart.UsedTypes;

namespace RADON.SerialPortClasses
{
    interface IModBus
    {
        void Connect();
        void Disconnect();
        float Read(byte adc, byte channel);
        float Read(Adress adress);
    }
}
