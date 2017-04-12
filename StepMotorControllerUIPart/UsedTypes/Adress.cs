using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepMotorControllerUIPart.UsedTypes
{
    public class Adress
    {
        public readonly byte AdcNumber;
        public readonly byte ChannelNumber;

        public Adress(byte adc, byte channel)
        {
            AdcNumber = adc;
            ChannelNumber = channel;
        }
    }
}
