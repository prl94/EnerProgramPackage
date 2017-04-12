using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StepMotorControllerUIPart.UsedTypes;

namespace StepMotorControllerUIPart.Helper
{
    public  class MathHelper
    {
        public static double GetStandardDeviation(float[] list)
        {
            double average = list.Average();
            double sumOfSquaresOfDifferences = list.Select(val => (val - average) * (val - average)).Sum();
            return Math.Sqrt(sumOfSquaresOfDifferences / list.Length);
        }

        public static double GetAverage(float[] list)
        {
            return list.Average();
        }
    }
}
