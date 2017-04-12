using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepMotorControllerUIPart.UsedTypes
{
    public class Energy
    {
        public string Formula;
        public double Value;
        public string InfoText;
        public double Rp;

        public Energy(string formula, double value, string infoText, double rp)
        {
            Formula = formula;
            Value = value;
            InfoText = infoText;
            Rp = rp;
        }
    }
}
