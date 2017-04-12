using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StepMotorControllerUIPart.Helper
{
   public class LinearRegression
    {
        private double[] _xArr;
        private double[] _yArr;
        private int N; 

        public double a {
            get
            {
                return (N * XiYiSum() - _xArr.Sum() * _yArr.Sum()) / (N * PowXi() - Math.Pow(_xArr.Sum(), 2));
            }
        }


        public double b
        {
            get
            {
                return (_yArr.Sum() - a * _xArr.Sum()) / (N);        
            }
        }

        public LinearRegression(double[] xArr, double[] yArr)
        {
            _xArr = xArr;
            _yArr = yArr;
            N = _xArr.Length;
        }

        private double XiYiSum()
        {
            double sum = 0;
            for (int i = 0; i < _xArr.Length; i++)
            {
                sum += _xArr[i]*_yArr[i];
            }
            return sum;
        }

        private double PowXi()
        {
            double powSum = 0;
            for (int i = 0; i < _xArr.Length; i++)
            {
                powSum += Math.Pow(_xArr[i], 2);
            }
            return powSum;
        }
    }
}
