using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StepMotorControllerUIPart.SettingsFolder;

namespace StepMotorControllerUIPart.UsedTypes
{
  public class Zeros
    {
        public double[] ZeroDeviationArray;

        public Zeros(Dictionary<string, double> resistors)
        {
            ZeroDeviationArray = new double[resistors.Count];
            for (int i = 0; i < resistors.Count; i++)
            {
                string key = "z" + (i + 1);
                ZeroDeviationArray[i] = resistors[key];
            }
        }
        public Zeros()
        {
            var count = ZeroDeviation.Default.Properties.Count;
            ZeroDeviationArray = new double[count];
            for (int i = 0; i < count; i++)
            {
                string key = "z" + (i + 1);
                ZeroDeviationArray[i] = (double)ZeroDeviation.Default[key];
            }
        }
    }
}

