using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StepMotorControllerUIPart.SettingsFolder;

namespace StepMotorControllerUIPart.UsedTypes
{
    //todo add diaphragmas array instead of current realidation
    public class Diaphragms
    {
         public float[] DiaphragmsArray;

         public Diaphragms(Dictionary<string, float> diaphragms)
        {
            DiaphragmsArray = new float[diaphragms.Count];
            for (int i = 0; i < diaphragms.Count; i++)
            {

                string key = "d" + (i + 1);
                DiaphragmsArray[i] = diaphragms[key];
            }
        }

        public Diaphragms()
        {
            var count = DiaphragmsStn.Default.Properties.Count;
            DiaphragmsArray = new float[count];
            for (int i = 0; i < count; i++)
            {
                string key = "D" + (i + 1);
                DiaphragmsArray[i] = (float)DiaphragmsStn.Default[key];
            }
        }
    }
}
