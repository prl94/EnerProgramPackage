using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using StepMotorControllerUIPart.SerialPortClasses;

namespace RADON.SerialPortClasses
{
    interface IArduino
    {
        void Connect();
        void Disconnect();
        void MakeOneStep();
    }
}
