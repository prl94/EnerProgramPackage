using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using StepMotorControllerUIPart.Helper;
using StepMotorControllerUIPart.Logic;
using StepMotorControllerUIPart.Properties;
using StepMotorControllerUIPart.SettingsFolder;
using StepMotorControllerUIPart.UsedTypes;
using StepMotorControllerUIPart.View;

namespace StepMotorControllerUIPart
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GeneralView());
        }
    }
}
