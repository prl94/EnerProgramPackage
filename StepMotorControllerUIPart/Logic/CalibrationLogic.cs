using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using NLog;
using RADON.SerialPortClasses;
using RADON.SerialPortClasses.test;
using StepMotorControllerUIPart.Properties;
using StepMotorControllerUIPart.SerialPortClasses;
using StepMotorControllerUIPart.SettingsFolder;
using StepMotorControllerUIPart.UsedTypes;

namespace StepMotorControllerUIPart.Logic
{
    static class CalibrationLogic
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void StartCalibration(ConnectionParams connectionParams, BackgroundWorker watcherWorker)
        {
            bool failed = true;
            var calibrationProgress = new CalibrationProgress();

          //  IModBus adc = new ModBus(connectionParams.ModBusComPort);
          //  IArduino arduino = new Arduino(connectionParams.ArduinoComPort);

             IModBus adc = new ModBusTest();
             IArduino arduino = new ArduinoTest();

            adc.Connect();
            arduino.Connect();

            calibrationProgress.CalibrationStarted = true;

            watcherWorker.ReportProgress(0, calibrationProgress);

            for (int i = 1; i <= 15; i++)
            {
                calibrationProgress.CalibrationStarted = false;
                calibrationProgress.ReadValue = true;

                watcherWorker.ReportProgress(50, calibrationProgress);

                float[] calibrationData = new float[10];
                for (int j = 0; j < calibrationData.Length; j++)
                {
                    var value = adc.Read(connectionParams.Channel1Adress);
                    calibrationData[j] = value;

                    calibrationProgress.Value = value;

                    watcherWorker.ReportProgress(50, calibrationProgress);

                }
                // if average value < CalibrationVoltage stepper in correct position
                if (calibrationData.Average() >= Settings.Default.CalibrationVoltage)
                {
                    failed = false;
                    calibrationProgress.ReadValue = false;
                    calibrationProgress.CalibrationFinished = true;
                    calibrationProgress.FinishedSuccesfully = true;

                    adc.Disconnect();
                    arduino.Disconnect();

                    watcherWorker.ReportProgress(100, calibrationProgress);

                    break;
                }

                arduino.MakeOneStep();
                Thread.Sleep(3000);
            }

            if (failed)
            {
                calibrationProgress.ReadValue = false;
                calibrationProgress.CalibrationFinished = true;
                calibrationProgress.FinishedSuccesfully = false;

                adc.Disconnect();
                arduino.Disconnect();
                watcherWorker.ReportProgress(100, calibrationProgress);

            }

        }

        public static void CalibrateZeros(MesureParams mesureParams, ConnectionParams connectionParams, BackgroundWorker watcherWorker)
        {
           //  IModBus adc = new ModBus(connectionParams.ModBusComPort);
           //  IArduino arduino = new Arduino(connectionParams.ArduinoComPort);

             IModBus adc = new ModBusTest();
             IArduino arduino = new ArduinoTest();

            adc.Connect();
            arduino.Connect();

            var steps = mesureParams.StepsCount;
            var length = mesureParams.MesuresCount;

            double[] calibrationDataSEM = new double[steps];

            double[] calibrationData1 = new double[steps];
            double[] calibrationData2 = new double[steps];

            logger.Debug("Start Calibration");
            for (int i = 1; i <= steps; i++)
            {

                logger.Trace("Calibration for {0} step", i);

                arduino.MakeOneStep();
                watcherWorker.ReportProgress(i);

                Thread.Sleep(mesureParams.DelayBeforeStep);

                float[] dataFromSEMArray = new float[length], dataFromStepper1Array = new float[length],
                    dataFromStepper2Array = new float[length];

                for (int j = 0; j < mesureParams.MesuresCount; j++)
                {
                    dataFromSEMArray[j] = adc.Read(connectionParams.SecondaryEmisionMonitorAdress);
                    dataFromStepper1Array[j] = adc.Read(connectionParams.Channel1Adress);
                    dataFromStepper2Array[j] = adc.Read(connectionParams.Channel2Adress);
                }

                calibrationDataSEM[i - 1] = dataFromSEMArray.Average();
                 calibrationData1[i-1] = dataFromStepper1Array.Average();
                calibrationData2[i-1] = dataFromStepper2Array.Average();
            }
            

            adc.Disconnect();
            arduino.Disconnect();

            

            ZeroDeviation.Default.z1 = calibrationData1[0];
            ZeroDeviation.Default.z2 = calibrationData1[1]; 
            ZeroDeviation.Default.z3 = calibrationData1[2];
            ZeroDeviation.Default.z4 = calibrationData1[3];
            ZeroDeviation.Default.z5 = calibrationData1[4];
            ZeroDeviation.Default.z6 = calibrationData1[5];
            ZeroDeviation.Default.z7 = calibrationData1[6];
            ZeroDeviation.Default.z8 = calibrationData1[7];
            ZeroDeviation.Default.z9 = calibrationData1[8];
            ZeroDeviation.Default.z10 = calibrationData1[9];
            ZeroDeviation.Default.z11 = calibrationData2[0];
            ZeroDeviation.Default.z12 = calibrationData2[1];
            ZeroDeviation.Default.z13 = calibrationData2[2];
            ZeroDeviation.Default.z14 = calibrationData2[3];
            ZeroDeviation.Default.z15 = calibrationData2[4];
            ZeroDeviation.Default.z16 = calibrationData2[5];
            ZeroDeviation.Default.z17 = calibrationData2[6];
            ZeroDeviation.Default.z18 = calibrationData2[7];
            ZeroDeviation.Default.z19 = calibrationData2[8];
            ZeroDeviation.Default.z20 = calibrationData2[9];

            //sem data
            ZeroDeviation.Default.z21 = calibrationDataSEM.Average();

            ZeroDeviation.Default.Save();
        }

        public class CalibrationProgress
        {
            public bool CalibrationStarted = false;

            public bool ReadValue = false;
            public double Value = 0;

            public bool CalibrationFinished = false;
            public bool FinishedSuccesfully;

        }

    }
}
