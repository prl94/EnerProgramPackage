using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using StepMotorControllerUIPart.UsedTypes;


namespace StepMotorControllerUIPart.Helper
{
    public static class ConfigReader
    {
        
        private static NameValueCollection GetSettings(String section)
        {

            NameValueCollection  settings = ConfigurationManager.GetSection(section) as
                System.Collections.Specialized.NameValueCollection;

            return settings;
        }


        public static Dictionary<string, float> GetResistors()
        {
            var resistors = GetSettings(Constans.SectionResistors);
            var resistorsDictionary = new Dictionary<string, float>();

            foreach (var key in resistors.AllKeys)
            {
                resistorsDictionary.Add(key, Convert.ToSingle(resistors[key]));
            }
            return resistorsDictionary;
        }

        public static Adress GetAdress(string adc, string channel)
        {
            var adresses = GetSettings(Constans.SectionModBusAdresses);
            var adress = new Adress(Convert.ToByte(adresses[adc]), Convert.ToByte(adresses[channel]));
            return adress;
        }


        public static Dictionary<string, float> GetDiaphragmas()
        {
            var diaphragms = GetSettings(Constans.SectionDiaphragms);
            var diaphragmsDictionary = new Dictionary<string, float>();

            foreach (var key in diaphragms.AllKeys)
            {
                diaphragmsDictionary.Add(key, Convert.ToSingle(diaphragms[key]));
            }

            return diaphragmsDictionary;
        }
    }
}
