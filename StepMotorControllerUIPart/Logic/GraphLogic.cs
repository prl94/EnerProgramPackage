using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StepMotorControllerUIPart.Helper;
using StepMotorControllerUIPart.UsedTypes;
using ZedGraph;

namespace StepMotorControllerUIPart.Logic
{
    static class GraphLogic
    {
        public static SortedList<double, double> FinalDataArray;

        public static PointPairList GetTDataForGraph(List<Mesure> mesuresList)
        {
            int count = mesuresList.Count;
            double[] inormArray = new double[count];
            double[] diaphragmasArray = new double[count];

            for (int i = 0; i < count; i++)
            {
                inormArray[i] = mesuresList[i].Inorm;
                diaphragmasArray[i] = mesuresList[i].Diaphragm;
            }

            double[] inormArrayComplete = PrepareCurrentArray(inormArray);
            double[] diaphragmasArrayComplete = PrepareDiaphragmArray(diaphragmasArray);


            var pointPairList = new PointPairList();

            FinalDataArray = new SortedList<double, double>();

            // todo need to refactor next loop
            for (int i = 0; i < count; i++)
            {
                FinalDataArray.Add(diaphragmasArrayComplete[i], inormArrayComplete[i] / inormArrayComplete[0]);
                pointPairList.Add(diaphragmasArrayComplete[i], inormArrayComplete[i] / inormArrayComplete[0]);
            }
            WritingToFile.WriteFinaleToFile(FinalDataArray);
            return pointPairList;
        }


        public static PointPairList GetUDUSDataForGraph(List<Mesure> mesuresList)
        {
            int count = mesuresList.Count;
          
            var pointPairList = new PointPairList();


            // todo need to refactor next loop
            for (int i = 1; i <= count; i++)
            {
                pointPairList.Add(i, mesuresList[i-1].UChannelAverage / mesuresList[i-1].USecondaryEmitionMonitorAverage);
            }
            return pointPairList;
        }

        //todo rework logic
        public static double[] GetSEMEnergyArray(List<Mesure> mesuresList)
        {
            double[] e = new double[10];

            for (int i = 0; i < 10; i++)
            {
                e[i] = mesuresList[i].USecondaryEmitionMonitorAverage;
            }

            return e;        
        }

        private static double[] PrepareCurrentArray(double[] arr)
        {
            double[] inormArrayComplete = new double[arr.Length];

            inormArrayComplete[0] = arr.Sum();
            for (int i = 1; i < arr.Count(); i++)
            {
                inormArrayComplete[i] = arr.Skip(i).Sum();
            }
            return inormArrayComplete;
        }
        private static double[] PrepareDiaphragmArray(double[] arr)
        {
            double[] diaphragmasArrayComplete = new double[arr.Length];

            for (int i = 1; i < arr.Count(); i++)
            {
                diaphragmasArrayComplete[i] = arr.Take(i).Sum();
            }
            return diaphragmasArrayComplete;
        }
    }
}
