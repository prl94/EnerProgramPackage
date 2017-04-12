using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using StepMotorControllerUIPart.UsedTypes;

namespace StepMotorControllerUIPart.Helper
{
    public class WritingToFile
    {

        private static String _fileName;



        public static void WriteMesureToFile(List<Mesure> mesures)
        {
            _fileName = ("виміри " + DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss") + ".txt");
            String fileName = "1 "+_fileName;
            StreamWriter writer = new StreamWriter(fileName);

            // Display header
            string header = String.Format("{0,-4}{1,-25}{2,-25}{3,-25}{4,-25}{5,-25}{6,-10}{7,-10}\n",
                "#", "U_SEM ", "StDev_SEM", "U_Diaphragm", "StDev_Diaphragm", "Inorm", "R", "D");
            writer.WriteLine(header);

            StringBuilder sb = new StringBuilder();

            foreach (var mesure in mesures)
            {
                writer.Write("{0,-4}{1,-25}{2,-25}{3,-25}{4,-25}{5,-25}{6,-10}{7,-10}{8}",
                    mesure.MesureNumber, mesure.USecondaryEmitionMonitorAverage, mesure.USecondaryEmitionMonitorStandardDeviation,
                    mesure.UChannelAverage, mesure.UChannelStandardDeviation, mesure.Inorm, mesure.Resistor, mesure.Diaphragm, Environment.NewLine);
                
            }

            writer.WriteLine(sb.ToString());
            writer.Close();

        }
        public static void WriteFinaleToFile(SortedList<double, double> arrForTextFile)
        {
            _fileName = ("виміри " + DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss") + ".txt");
            String fileName = "2" + _fileName;
            StreamWriter writer = new StreamWriter(fileName);

            // Display header
            string header = String.Format("{0,-25}{1,-25}\n","D", "T");

            writer.WriteLine(header);

            StringBuilder sb = new StringBuilder();

            foreach (var data in arrForTextFile)
            {
                writer.Write("{0,-25}{1,-25}{2}",
                    data.Key, data.Value, Environment.NewLine);

            }

            writer.WriteLine(sb.ToString());
            writer.Close();




        }

        public static void WriteEnergyToFile(Energy energy)
        {
            _fileName = ("виміри " + DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss") + ".txt");
            String fileName = "3" + _fileName;
            StreamWriter writer = new StreamWriter(fileName);

            writer.WriteLine(energy.InfoText);
            writer.WriteLine("Rp = "+energy.Rp.ToString());
            writer.WriteLine("Енергія по формулі: "+energy.Formula + " = "+ energy.Value.ToString());
            writer.Close();
        }
    }
}
