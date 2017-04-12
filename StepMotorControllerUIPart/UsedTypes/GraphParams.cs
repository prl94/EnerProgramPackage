using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepMotorControllerUIPart.UsedTypes
{
    public class GraphParams
    {
        public string TitleText;
        public string XAxisText;
        public string YAxisText;
        public double X;
        public double Y;

        public GraphParams(string titleText, string xAxisText, string yAxisText, double x, double y)
        {
            TitleText = titleText;
            XAxisText = xAxisText;
            YAxisText = yAxisText;
            X = x;
            Y = y;
        }

    }
}
