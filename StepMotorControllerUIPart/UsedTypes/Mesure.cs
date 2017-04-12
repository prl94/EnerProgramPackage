using System;
using System.Linq;
using StepMotorControllerUIPart.Helper;

namespace StepMotorControllerUIPart.UsedTypes
{
    public class Mesure
    {
        public readonly int MesureNumber;
        private readonly float[] USecondaryEmitionMonitor;
        private readonly float[] UChannel;
        public float Resistor;
        public float Diaphragm;
        public double ZeroDeviation;
        public double SEMzero;

        public double Inorm {
            get
            {
              //  if (USecondaryEmitionMonitorAverage!=0 && UChannelAverage != 0 && Resistor !=0)
               // {
                    return (UChannelAverage / Resistor)/USecondaryEmitionMonitorAverage;
               // }
               // else
               // {
               //     return 0;
               // }
            }
        }
    
        //average
        public double USecondaryEmitionMonitorAverage {
            get { return USecondaryEmitionMonitor.Average() - SEMzero; }
        }
        public double UChannelAverage
        {
            get { return (UChannel.Average() - ZeroDeviation); }
        }

        //StandardDeviation
        public double USecondaryEmitionMonitorStandardDeviation
        {
            get { return MathHelper.GetStandardDeviation(USecondaryEmitionMonitor); }
        }
        public double UChannelStandardDeviation
        {
            get { return MathHelper.GetStandardDeviation(UChannel); }
        }

        public Mesure(int mesureNumber, float[] uSecondaryEmitionMonitor, float[] uChannel, float resistor, float diaphragm, double zeroDeviation, double semZero)
        {
            MesureNumber = mesureNumber;
            USecondaryEmitionMonitor = uSecondaryEmitionMonitor;
            UChannel = uChannel;
            Resistor = resistor;
            Diaphragm = diaphragm;
            ZeroDeviation = zeroDeviation;
            SEMzero = semZero;
        }
    }
}
