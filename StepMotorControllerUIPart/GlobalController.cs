using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StepMotorControllerUIPart.DTO;
using StepMotorControllerUIPart.Helper;
using StepMotorControllerUIPart.Model;
using ZedGraph;

namespace StepMotorControllerUIPart.Controller
{
    class GlobalController
    {

        public static bool SendDataToSwitcherController(int steps)
        {
            return ComPortController.SendDataToSwitcherController((byte)steps);
        }

        public static PointPairList StartTestMesures(ParametersDto parameters)
        {
            var myMesures = ComPortController.GetTestMesuresList(parameters);


            MathHelper myHelper = new MathHelper(myMesures);

            var calculetedList = myHelper.getCalculatedData();
            
              
            myHelper.IEfective();

            WritingToFile.WriteDataToFile("myfile", calculetedList, myHelper.IEfective());

           var myDict = new Dictionary<int, double>();

           foreach (var value in calculetedList)
            {
                myDict.Add(value.SwitcherPosition, value.AverageValue);
            }

            return GraphPointsGenerator(myDict);
        }

        public static Dictionary<int, double> StartMesures(ParametersDto parameters)
        {

            var testData = ComPortController.StartMesures(parameters);

            foreach (var mesure in testData)
            {
                Console.WriteLine(mesure.SwitcherPosition);
                Console.WriteLine(mesure.DataFromOscillatorArray[0]);
                Console.WriteLine(mesure.DataFromSwitcherArray[0]);
            }
            Console.WriteLine(testData.Count);

            return null;
        }

        private static PointPairList GraphPointsGenerator(Dictionary<int, double> lineArray)
        {
            var pointList = new PointPairList();

            foreach (var point in lineArray)
            {
                pointList.Add(point.Key, point.Value);

            }
            return pointList;
        }
    }
}
