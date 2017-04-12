using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StepMotorControllerUIPart.SettingsFolder;

namespace StepMotorControllerUIPart.UsedTypes
{
    public class Resistors
    {
       public float[] ResistorsArray;

        public Resistors(Dictionary<string, float> resistors)
        {
            ResistorsArray = new float[resistors.Count];
            for (int i= 0; i < resistors.Count; i++)
            {
                string key = "R" + (i + 1);
                ResistorsArray[i] = resistors[key];
            }
        }
        public Resistors()
        {
            var count = ResistorsStn.Default.Properties.Count;
            ResistorsArray = new float[count];
            for (int i = 0; i < count; i++)
            {
                string key = "R" + (i + 1);
                ResistorsArray[i] = (float)ResistorsStn.Default[key];
            }
        }
    }    
}

