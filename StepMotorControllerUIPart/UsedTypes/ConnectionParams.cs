using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepMotorControllerUIPart.UsedTypes
{
    public class ConnectionParams
    {
        public readonly String ModBusComPort;
        public readonly String ArduinoComPort;
        public readonly Adress SecondaryEmisionMonitorAdress;
        public readonly Adress Channel1Adress;
        public readonly Adress Channel2Adress;

        public ConnectionParams(String modBusComPort, String arduinoComPort, Adress secondaryEmisionMonitorAdress,
            Adress channel1Adress, Adress channel2Adress)
        {
            ModBusComPort = modBusComPort;
            ArduinoComPort = arduinoComPort;
            SecondaryEmisionMonitorAdress = secondaryEmisionMonitorAdress;
            Channel1Adress = channel1Adress;
            Channel2Adress = channel2Adress;
        }
    }
}
