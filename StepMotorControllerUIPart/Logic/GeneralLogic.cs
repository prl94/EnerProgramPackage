 using System;
using System.Collections.Generic;
 using System.ComponentModel;
 using System.Linq;
using System.Threading;
using NLog;
using RADON.SerialPortClasses;
using RADON.SerialPortClasses.test;
using StepMotorControllerUIPart.SerialPortClasses;
using StepMotorControllerUIPart.UsedTypes;

namespace StepMotorControllerUIPart.Logic
{
    public static class GeneralLogic
    {


        private static Logger logger = LogManager.GetCurrentClassLogger();


        public static event Action CalibrationStart;
        public static event Action<int> CalibrationStep;
        public static event Action<bool> CalibrationFinish;


        public static List<Mesure> TestMesures()
        {
            var mesuresList = new List<Mesure>();

            mesuresList.Add(new Mesure(1, new []{ (float)0.889332998543978 }, new[]{ (float)0 }, 1000, (float)0.053, 0, 0));

            mesuresList.Add(new Mesure(2, new[] { (float)0.859622288495302 }, new[] { (float)-0.166142195463181 }, 1000, (float)0.053, 0, 0));

            mesuresList.Add(new Mesure(3, new[] { (float)0.859506953507662 }, new[] { (float)-0.00919562578201294 }, 1000, (float)0.095, 0, 0));

            mesuresList.Add(new Mesure(4, new[] { (float)0.895745623856783 }, new[] { (float)0.00646239519119263 }, 1000, (float)0.095, 0, 0));

            mesuresList.Add(new Mesure(5, new[] { (float)0.905093181878328 }, new[] { (float)0.0320420861244202 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(6, new[] { (float)0.870678830891848 }, new[] { (float)0.121829986572266 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(7, new[] { (float)0.862671006470919 }, new[] { (float)0.225058987736702 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(8, new[] { (float)0.875609803944826 }, new[] { (float)0.338822323828936 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(9, new[] { (float)0.928998220711946 }, new[] { (float)0.514135122299194 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(10, new[] { (float)0.927274871617556 }, new[] { (float)0.680631637573242 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(11, new[] { (float)0.889332998543978 }, new[] { (float)0.78 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(12, new[] { (float)0.859622288495302 }, new[] { (float)0.848756283521652 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(13, new[] { (float)0.859506953507662 }, new[] { (float)0.920673310756683 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(14, new[] { (float)0.895745623856783 }, new[] { (float)0.842506754444912 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(15, new[] { (float)0.905093181878328 }, new[] { (float)0.656907021999359 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(16, new[] { (float)0.870678830891848 }, new[] { (float)0.353736409917474 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(17, new[] { (float)0.862671006470919 }, new[] { (float)0.133262399584055 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(18, new[] { (float)0.875609803944826 }, new[] { (float)0.0282758772373199 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(19, new[] { (float)0.928998220711946 }, new[] { (float)0.000701546669006348 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(20, new[] { (float)0.927274871617556 }, new[] { (float)0.00177302956581116 }, 1000, (float)0.188, 0, 0));


            return mesuresList;

        }


        public static List<Mesure> TestMesures1()
        {
            var mesuresList = new List<Mesure>();

            mesuresList.Add(new Mesure(1, new[] { (float)2.82873677648604 }, new[] { (float)0.00018310546875 }, 1000, (float)0.053, 0, 0));

            mesuresList.Add(new Mesure(2, new[] { (float)2.79021381773055 }, new[] { (float)-0.850358054041862 }, 1000, (float)0.053, 0, 0));

            mesuresList.Add(new Mesure(3, new[] { (float)2.71014141477644 }, new[] { (float)-0.0376817584037781 }, 1000, (float)0.095, 0, 0));

            mesuresList.Add(new Mesure(4, new[] { (float)2.61383056081831 }, new[] { (float)0.000209629535675049 }, 1000, (float)0.095, 0, 0));

            mesuresList.Add(new Mesure(5, new[] { (float)2.85789441503584 }, new[] { (float)0.0725559890270233 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(6, new[] { (float)2.93780755437911 }, new[] { (float)0.371692627668381 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(7, new[] { (float)2.79551386274397 }, new[] { (float)0.703427448868752 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(8, new[] { (float)2.85979604162276 }, new[] { (float)1.11533784866333 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(9, new[] { (float)3.04191946424544 }, new[] { (float)1.7527959048748 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(10, new[] { (float)3.06149625219405 }, new[] { (float)2.34970310330391 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(11, new[] { (float)2.82873677648604 }, new[] { (float)0 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(12, new[] { (float)2.79021381773055 }, new[] { (float)2.92210498079658 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(13, new[] { (float)2.71014141477644 }, new[] { (float)3.02459919452667 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(14, new[] { (float)2.61383056081831 }, new[] { (float)2.52603846788406 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(15, new[] { (float)2.85789441503584 }, new[] { (float)2.11043399572372 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(16, new[] { (float)2.93780755437911 }, new[] { (float)1.21242526173592 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(17, new[] { (float)2.79551386274397 }, new[] { (float)0.416985839605331 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(18, new[] { (float)2.85979604162276 }, new[] { (float)0.0829680562019348 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(19, new[] { (float)3.04191946424544 }, new[] { (float)0.0162792205810547 }, 1000, (float)0.188, 0, 0));

            mesuresList.Add(new Mesure(20, new[] { (float)3.06149625219405 }, new[] { (float)0.0187235847115517 }, 1000, (float)0.188, 0, 0));


            return mesuresList;

        }

        //todo current realisation works only to 10 steps rework for ability work with 12
        public static List<Mesure> StartMesures(MesureParams mesureParams, ConnectionParams connectionParams, Resistors resistors, Diaphragms diaphragms, Zeros zeroDeviation,
            BackgroundWorker watcher)
        {

           IModBus adc = new ModBus(connectionParams.ModBusComPort);
           IArduino arduino = new Arduino(connectionParams.ArduinoComPort);

           // IModBus adc = new ModBusTest();
           // IArduino arduino = new ArduinoTest();

            adc.Connect();
            arduino.Connect();

            var mesuresList = new List<Mesure>();

            List<Mesure> stepper1List = new List<Mesure>();
            List<Mesure> stepper2List = new List<Mesure>();

            logger.Debug("Start Mesures");

            var progress = new MesureProgress();

            progress.MesureStarted = true;

            watcher.ReportProgress(0, progress);


            for (int i = 1; i <= mesureParams.StepsCount; i++)
            {
                logger.Trace("Mesure for {0} step", i);

                arduino.MakeOneStep();
                Thread.Sleep(mesureParams.DelayBeforeStep);

                var length = mesureParams.MesuresCount;
                float[] dataFromOscillatorArray = new float[length],
                    dataFromStepper1Array = new float[length],
                    dataFromStepper2Array = new float[length];

                progress.MesureStarted = false;
                progress.ReadValue = true;
                progress.Step = i;
                watcher.ReportProgress(50, progress);

                for (int j = 0; j < mesureParams.MesuresCount; j++)
                {
                    var dFO = adc.Read(connectionParams.SecondaryEmisionMonitorAdress);
                    var dFS1 = adc.Read(connectionParams.Channel1Adress);
                    var dFS2 = adc.Read(connectionParams.Channel2Adress);

                    dataFromOscillatorArray[j] = dFO;
                    dataFromStepper1Array[j] = dFS1;
                    dataFromStepper2Array[j] = dFS2;

                    progress.Value[0] = dFO;
                    progress.Value[1] = dFS1;
                    progress.Value[2] = dFS2;

                    watcher.ReportProgress(50, progress);
                }

                stepper1List.Add(new Mesure(i, dataFromOscillatorArray, dataFromStepper1Array,resistors.ResistorsArray[i-1],diaphragms.DiaphragmsArray[i-1], zeroDeviation.ZeroDeviationArray[i-1], zeroDeviation.ZeroDeviationArray[20]));
                stepper2List.Add(new Mesure(i + 10, dataFromOscillatorArray, dataFromStepper2Array,resistors.ResistorsArray[(i-1)+10],diaphragms.DiaphragmsArray[(i-1)+10], zeroDeviation.ZeroDeviationArray[(i - 1)+10], zeroDeviation.ZeroDeviationArray[20]));

            }
            mesuresList.AddRange(stepper1List);
            mesuresList.AddRange(stepper2List);

            adc.Disconnect();
            arduino.Disconnect();

            logger.Debug("Finished Mesures");

            progress.ReadValue = false;
            progress.MesureFinished = true;
            watcher.ReportProgress(100, progress);

            return mesuresList;
        }
    }

    public class MesureProgress
    {
        public bool MesureStarted = false;

        public int Step = 0;
        public bool ReadValue = false;
        public float[] Value = new float[3];

        public bool MesureFinished = false;
    }

}

