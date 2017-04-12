using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using StepMotorControllerUIPart.Helper;
using StepMotorControllerUIPart.SerialPortClasses;
using StepMotorControllerUIPart.UsedTypes;
using StepMotorControllerUIPart.Logic;
using StepMotorControllerUIPart.Properties;
using StepMotorControllerUIPart.SettingsFolder;
using ZedGraph;
using AxisType = ZedGraph.AxisType;

namespace StepMotorControllerUIPart.View
{
    public partial class GeneralView : Form
    {
        private double R;

        private List<Mesure> _mesures; 

        private Diaphragms _diaphragms;
        private Resistors _resistors;
        private ConnectionParams _connectionParams;
        private MesureParams _mesureParameters;
        private Zeros _zeroDeviation ;
       


        public GeneralView()
        {
            InitializeComponent();
        }



    

        private void DrawLineGraph(PointPairList list, GraphParams graphParams)
        {
            GraphPane pane = zedGraph.GraphPane;

            pane.CurveList.Clear();
            pane.GraphObjList.Clear();

            pane.Title.Text = graphParams.TitleText;
            pane.XAxis.Title.Text = graphParams.XAxisText;
            pane.YAxis.Title.Text = graphParams.YAxisText;
            pane.Legend.IsVisible = false;
            pane.CurveList.Clear();


            GraphObjList obgList = new GraphObjList();


            // *** Выведем текст с фоном по умолчанию (с белым фоном) ***
            for (int i = 0; i < list.Count; i++)
            {

                TextObj temp = new TextObj((i + 1).ToString(), list[i].X + graphParams.X, list[i].Y + graphParams.Y);

                // Отключим рамку вокруг текста
                temp.FontSpec.Border.IsVisible = false;
                temp.FontSpec.Fill = new Fill();
                obgList.Add(temp);

            }
            pane.GraphObjList.AddRange(obgList);


            LineItem myCurve = pane.AddCurve("Sinc", list, Color.Blue, SymbolType.Circle);
            
            myCurve.Symbol.Fill.Type = FillType.Solid;
            myCurve.Symbol.Size = 5;

            zedGraph.AxisChange();

            // Обновляем график
            zedGraph.Invalidate();

        }

        private void ShowMessageBox(string message)
        {
            MessageBox.Show(message, "Important Message");
        }

        private void updateBaseObjects()
        {
            _diaphragms = new Diaphragms();
            _resistors = new Resistors();
            _zeroDeviation = new Zeros();



            var secondaryEmisionMonitor = new Adress(Settings.Default.SecondaryEmisionMonitorAdcNumber, Settings.Default.SecondaryEmisionMonitorChannelNumber);
            var channel1 = new Adress(Settings.Default.Channel1AdcNumber, Settings.Default.Channel1ChannelNumber);
            var channel2 = new Adress(Settings.Default.Channel2AdcNumber, Settings.Default.Channel2ChannelNumber);

            string arduinoPort = Settings.Default.ArduinoCOMPort;
            string ADCPort = Settings.Default.AKONCOMPort;

            _connectionParams = new ConnectionParams(ADCPort, arduinoPort, secondaryEmisionMonitor, channel1, channel2);


            int stepsCount = Convert.ToInt32(Settings.Default.StepsCount);
            int mesuresPerStep = Convert.ToInt32(Settings.Default.MesuresPerStep);
            int delayBeforeStep = Convert.ToInt32(Settings.Default.DelayBeforeStep);


            _mesureParameters = new MesureParams(stepsCount, mesuresPerStep, delayBeforeStep);

        }


        private void GeneralView_Shown(object sender, EventArgs e)
        {
            InitSettings();
            updateBaseObjects();
        }

        private void zedGraph_MouseClick(object sender, MouseEventArgs e)
        {
            // Сюда будет сохранена кривая, рядом с которой был произведен клик
            CurveItem curve;

            // Сюда будет сохранен номер точки кривой, ближайшей к точке клика
            int index;

            GraphPane pane = zedGraph.GraphPane;
            

            // Максимальное расстояние от точки клика до кривой в пикселях, 
            // при котором еще считается, что клик попал в окрестность кривой.
            GraphPane.Default.NearestTol = 10;

            bool result = pane.FindNearestPoint(e.Location, out curve, out index);

            if (result)
            {
                // Максимально расстояние от точки клика до кривой не превысило NearestTol

                // Добавим точку на график, вблизи которой произошел клик
                PointPairList point = new PointPairList();

                point.Add(curve[index]);

                // Кривая, состоящая из одной точки. Точка будет отмечена синим кругом
                LineItem curvePount = pane.AddCurve("",
                    new double[] { curve[index].X },
                    new double[] { curve[index].Y },
                    Color.Red,
                    SymbolType.Circle);

                // 
                curvePount.Line.IsVisible = false;

                // Цвет заполнения круга - колубой
                curvePount.Symbol.Fill.Color = Color.Red;

                // Тип заполнения - сплошная заливка
                curvePount.Symbol.Fill.Type = FillType.Solid;

                // Размер круга
                curvePount.Symbol.Size = 7;
            }
        }

        // draw line
        private void button1_Click(object sender, EventArgs e)
        {
            int point1 = Convert.ToInt32(dot1TextBox.Text) - 1;
            int point2 = Convert.ToInt32(dot2TextBox.Text) - 1;

            var graphData = GraphLogic.FinalDataArray;
            graphData.Skip(point1);
      
            var linearSection = new SortedList<double, double>();

            for (int i = point1; i <= point2; i++)
            {
               var elem = graphData.ElementAt(i);
               linearSection.Add(elem.Key,elem.Value);
            }

            _calculateLine(linearSection);


        }

        private void _calculateLine(SortedList<double, double> list)
        {


            double[] xPoints = new double[list.Count];
            double[] yPoints = new double[list.Count];



            for (int i = 0; i < list.Count; i++)
            {
                var elem = list.ElementAt(i);
                xPoints[i] = elem.Key;
                yPoints[i] = elem.Value;

            }

            LinearRegression ln = new LinearRegression(xPoints, yPoints);

            double a = ln.a;
            double b = ln.b;
            R = Math.Abs(Math.Round(b / a, 4));

            // y = a*x + b 

            double x1 = 0;
            // y = a * x + b, x = x1
            double y1 = a * x1 + b;


            double y2 = 0;
            // x = (y - b) / a, y = y2
            double x2 = (y2 - b) / a;

            var line = new PointPairList();
            line.Add(x1, y1);
            line.Add(x2, y2);

            DrawLine(line);
        }

        private void DrawLine(PointPairList list)
        {


            GraphPane pane = zedGraph.GraphPane;
           
            
            LineItem myCurve = pane.AddCurve("", list, Color.Red, SymbolType.None);

            myCurve.Line.Width = 2;
            zedGraph.AxisChange();

            // Обновляем график
            zedGraph.Invalidate();

        }
        private void saveSattingsButton_Click(object sender, EventArgs e)
        {
            UpdateSettings();
            updateBaseObjects();
            MessageBox.Show("Дані збережено");

        }


        private void InitSettings()
        {

            StepsCountTextBox.Text = Settings.Default.StepsCount.ToString();
            MesuresPerStepTextBox.Text = Settings.Default.MesuresPerStep.ToString();
            DelayBeforeStepTextBox.Text = Settings.Default.DelayBeforeStep.ToString();
            CalibrationVoltageTextBox.Text = Settings.Default.CalibrationVoltage.ToString();

            AKONCOMPortTextBox.Text = Settings.Default.AKONCOMPort;

            SecondaryEmisionMonitorAdcNumberTextBox.Text = Settings.Default.SecondaryEmisionMonitorAdcNumber.ToString();
            SecondaryEmisionMonitorChannelNumberTextBox.Text = Settings.Default.SecondaryEmisionMonitorChannelNumber.ToString();

            Channel1AdcNumberTextBox.Text = Settings.Default.Channel1AdcNumber.ToString();
            Channel1ChannelNumberTextBox.Text = Settings.Default.Channel1ChannelNumber.ToString();

            Channel2AdcNumberTextBox.Text = Settings.Default.Channel2AdcNumber.ToString();
            Channel2ChannelNumberTextBox.Text = Settings.Default.Channel2ChannelNumber.ToString();

            ArduinoCOMPortTextBox.Text = Settings.Default.ArduinoCOMPort;


            R1textBox.Text = ResistorsStn.Default.R1.ToString();
            R2textBox.Text = ResistorsStn.Default.R2.ToString();
            R3textBox.Text = ResistorsStn.Default.R3.ToString();
            R4textBox.Text = ResistorsStn.Default.R4.ToString();
            R5textBox.Text = ResistorsStn.Default.R5.ToString();
            R6textBox.Text = ResistorsStn.Default.R6.ToString();
            R7textBox.Text = ResistorsStn.Default.R7.ToString();
            R8textBox.Text = ResistorsStn.Default.R8.ToString();
            R9textBox.Text = ResistorsStn.Default.R9.ToString();
            R10textBox.Text = ResistorsStn.Default.R10.ToString();
            R11textBox.Text = ResistorsStn.Default.R11.ToString();
            R12textBox.Text = ResistorsStn.Default.R12.ToString();
            R13textBox.Text = ResistorsStn.Default.R13.ToString();
            R14textBox.Text = ResistorsStn.Default.R14.ToString();
            R15textBox.Text = ResistorsStn.Default.R15.ToString();
            R16textBox.Text = ResistorsStn.Default.R16.ToString();
            R17textBox.Text = ResistorsStn.Default.R17.ToString();
            R18textBox.Text = ResistorsStn.Default.R18.ToString();
            R19textBox.Text = ResistorsStn.Default.R19.ToString();
            R20textBox.Text = ResistorsStn.Default.R20.ToString();

            D1textBox.Text = DiaphragmsStn.Default.D1.ToString();
            D2textBox.Text = DiaphragmsStn.Default.D2.ToString();
            D3textBox.Text = DiaphragmsStn.Default.D3.ToString();
            D4textBox.Text = DiaphragmsStn.Default.D4.ToString();
            D5textBox.Text = DiaphragmsStn.Default.D5.ToString();
            D6textBox.Text = DiaphragmsStn.Default.D6.ToString();
            D7textBox.Text = DiaphragmsStn.Default.D7.ToString();
            D8textBox.Text = DiaphragmsStn.Default.D8.ToString();
            D9textBox.Text = DiaphragmsStn.Default.D9.ToString();
            D10textBox.Text = DiaphragmsStn.Default.D10.ToString();
            D11textBox.Text = DiaphragmsStn.Default.D11.ToString();
            D12textBox.Text = DiaphragmsStn.Default.D12.ToString();
            D13textBox.Text = DiaphragmsStn.Default.D13.ToString();
            D14textBox.Text = DiaphragmsStn.Default.D14.ToString();
            D15textBox.Text = DiaphragmsStn.Default.D15.ToString();
            D16textBox.Text = DiaphragmsStn.Default.D16.ToString();
            D17textBox.Text = DiaphragmsStn.Default.D17.ToString();
            D18textBox.Text = DiaphragmsStn.Default.D18.ToString();
            D19textBox.Text = DiaphragmsStn.Default.D19.ToString();
            D20textBox.Text = DiaphragmsStn.Default.D20.ToString();


            z1TextBox.Text = ZeroDeviation.Default.z1.ToString();
            z2TextBox.Text = ZeroDeviation.Default.z2.ToString();
            z3TextBox.Text = ZeroDeviation.Default.z3.ToString();
            z4TextBox.Text = ZeroDeviation.Default.z4.ToString();
            z5TextBox.Text = ZeroDeviation.Default.z5.ToString();
            z6TextBox.Text = ZeroDeviation.Default.z6.ToString();
            z7TextBox.Text =  ZeroDeviation.Default.z7.ToString();
            z8TextBox.Text = ZeroDeviation.Default.z8.ToString();
            z9TextBox.Text = ZeroDeviation.Default.z9.ToString();
            z10TextBox.Text = ZeroDeviation.Default.z10.ToString();
            z11TextBox.Text =  ZeroDeviation.Default.z11.ToString();
            z12TextBox.Text =  ZeroDeviation.Default.z12.ToString();
            z13TextBox.Text = ZeroDeviation.Default.z13.ToString();
            z14TextBox.Text = ZeroDeviation.Default.z14.ToString();
            z15TextBox.Text = ZeroDeviation.Default.z15.ToString();
            z16TextBox.Text = ZeroDeviation.Default.z16.ToString();
            z17TextBox.Text = ZeroDeviation.Default.z17.ToString();
            z18TextBox.Text = ZeroDeviation.Default.z18.ToString();
            z19TextBox.Text = ZeroDeviation.Default.z19.ToString();
            z20TextBox.Text = ZeroDeviation.Default.z20.ToString();
            //SEM
            zSEMtextBox.Text = ZeroDeviation.Default.z21.ToString();


        }

        private void UpdateSettings()
        {
            Settings.Default.StepsCount = Convert.ToInt32(StepsCountTextBox.Text);
            Settings.Default.MesuresPerStep = Convert.ToInt32(MesuresPerStepTextBox.Text);
            Settings.Default.DelayBeforeStep = Convert.ToInt32(DelayBeforeStepTextBox.Text);
            Settings.Default.CalibrationVoltage = Convert.ToSingle(CalibrationVoltageTextBox.Text); 

            Settings.Default.AKONCOMPort = AKONCOMPortTextBox.Text;

            Settings.Default.SecondaryEmisionMonitorAdcNumber = Convert.ToByte(SecondaryEmisionMonitorAdcNumberTextBox.Text);
            Settings.Default.SecondaryEmisionMonitorChannelNumber = Convert.ToByte(SecondaryEmisionMonitorChannelNumberTextBox.Text);

            Settings.Default.Channel1AdcNumber = Convert.ToByte(Channel1AdcNumberTextBox.Text);
            Settings.Default.Channel1ChannelNumber = Convert.ToByte(Channel1ChannelNumberTextBox.Text);

            Settings.Default.Channel2AdcNumber = Convert.ToByte(Channel2AdcNumberTextBox.Text);
            Settings.Default.Channel2ChannelNumber = Convert.ToByte(Channel2ChannelNumberTextBox.Text);

            Settings.Default.ArduinoCOMPort = ArduinoCOMPortTextBox.Text;

            Settings.Default.Save();

            ResistorsStn.Default.R1 = Convert.ToSingle(R1textBox.Text);
            ResistorsStn.Default.R2 = Convert.ToSingle(R2textBox.Text);
            ResistorsStn.Default.R3 = Convert.ToSingle(R3textBox.Text);
            ResistorsStn.Default.R4 = Convert.ToSingle(R4textBox.Text);
            ResistorsStn.Default.R5 = Convert.ToSingle(R5textBox.Text);
            ResistorsStn.Default.R6 = Convert.ToSingle(R6textBox.Text);
            ResistorsStn.Default.R7 = Convert.ToSingle(R7textBox.Text);
            ResistorsStn.Default.R8 = Convert.ToSingle(R8textBox.Text);
            ResistorsStn.Default.R9 = Convert.ToSingle(R9textBox.Text);
            ResistorsStn.Default.R10 = Convert.ToSingle(R10textBox.Text);
            ResistorsStn.Default.R11 = Convert.ToSingle(R11textBox.Text);
            ResistorsStn.Default.R12 = Convert.ToSingle(R12textBox.Text);
            ResistorsStn.Default.R13 = Convert.ToSingle(R13textBox.Text);
            ResistorsStn.Default.R14 = Convert.ToSingle(R14textBox.Text);
            ResistorsStn.Default.R15 = Convert.ToSingle(R15textBox.Text);
            ResistorsStn.Default.R16 = Convert.ToSingle(R16textBox.Text);
            ResistorsStn.Default.R17 = Convert.ToSingle(R17textBox.Text);
            ResistorsStn.Default.R18 = Convert.ToSingle(R18textBox.Text);
            ResistorsStn.Default.R19 = Convert.ToSingle(R19textBox.Text);
            ResistorsStn.Default.R20 = Convert.ToSingle(R20textBox.Text);

            ResistorsStn.Default.Save();

            DiaphragmsStn.Default.D1 = Convert.ToSingle(D1textBox.Text);
            DiaphragmsStn.Default.D2 = Convert.ToSingle(D2textBox.Text);
            DiaphragmsStn.Default.D3 = Convert.ToSingle(D3textBox.Text);
            DiaphragmsStn.Default.D4 = Convert.ToSingle(D4textBox.Text);
            DiaphragmsStn.Default.D5 = Convert.ToSingle(D5textBox.Text);
            DiaphragmsStn.Default.D6 = Convert.ToSingle(D6textBox.Text);
            DiaphragmsStn.Default.D7 = Convert.ToSingle(D7textBox.Text);
            DiaphragmsStn.Default.D8 = Convert.ToSingle(D8textBox.Text);
            DiaphragmsStn.Default.D9 = Convert.ToSingle(D9textBox.Text);
            DiaphragmsStn.Default.D10 = Convert.ToSingle(D10textBox.Text);
            DiaphragmsStn.Default.D11 = Convert.ToSingle(D11textBox.Text);
            DiaphragmsStn.Default.D12 = Convert.ToSingle(D12textBox.Text);
            DiaphragmsStn.Default.D13 = Convert.ToSingle(D13textBox.Text);
            DiaphragmsStn.Default.D14 = Convert.ToSingle(D14textBox.Text);
            DiaphragmsStn.Default.D15 = Convert.ToSingle(D15textBox.Text);
            DiaphragmsStn.Default.D16 = Convert.ToSingle(D16textBox.Text);
            DiaphragmsStn.Default.D17 = Convert.ToSingle(D17textBox.Text);
            DiaphragmsStn.Default.D18 = Convert.ToSingle(D18textBox.Text);
            DiaphragmsStn.Default.D19 = Convert.ToSingle(D19textBox.Text);
            DiaphragmsStn.Default.D20 = Convert.ToSingle(D20textBox.Text);

            DiaphragmsStn.Default.Save();



            ZeroDeviation.Default.z1 = Convert.ToSingle(z1TextBox.Text);
            ZeroDeviation.Default.z2 = Convert.ToSingle(z2TextBox.Text);
            ZeroDeviation.Default.z3 = Convert.ToSingle(z3TextBox.Text);
            ZeroDeviation.Default.z4 = Convert.ToSingle(z4TextBox.Text);
            ZeroDeviation.Default.z5 = Convert.ToSingle(z5TextBox.Text);
            ZeroDeviation.Default.z6 = Convert.ToSingle(z6TextBox.Text);
            ZeroDeviation.Default.z7 = Convert.ToSingle(z7TextBox.Text);
            ZeroDeviation.Default.z8 = Convert.ToSingle(z8TextBox.Text);
            ZeroDeviation.Default.z9 = Convert.ToSingle(z9TextBox.Text);
            ZeroDeviation.Default.z10 = Convert.ToSingle(z10TextBox.Text);
            ZeroDeviation.Default.z11 = Convert.ToSingle(z11TextBox.Text);
            ZeroDeviation.Default.z12 = Convert.ToSingle(z12TextBox.Text);
            ZeroDeviation.Default.z13 = Convert.ToSingle(z13TextBox.Text);
            ZeroDeviation.Default.z14 = Convert.ToSingle(z14TextBox.Text);
            ZeroDeviation.Default.z15 = Convert.ToSingle(z15TextBox.Text);
            ZeroDeviation.Default.z16 = Convert.ToSingle(z16TextBox.Text);
            ZeroDeviation.Default.z17 = Convert.ToSingle(z17TextBox.Text);
            ZeroDeviation.Default.z18 = Convert.ToSingle(z18TextBox.Text);
            ZeroDeviation.Default.z19 = Convert.ToSingle(z19TextBox.Text);
            ZeroDeviation.Default.z20 = Convert.ToSingle(z20TextBox.Text);
            ZeroDeviation.Default.z21 = Convert.ToSingle(zSEMtextBox.Text);


            ZeroDeviation.Default.Save();

        }



        private void f1_CheckedChanged(object sender, EventArgs e)
        {
            f2.Checked = false;
            f3.Checked = false;
        }

        private void f2_CheckedChanged(object sender, EventArgs e)
        {
            f1.Checked = false;
            f3.Checked = false;
        }

        private void f3_CheckedChanged(object sender, EventArgs e)
        {
            f1.Checked = false;
            f2.Checked = false;
        }



        //-------------- calculate final energy ----------------
        private string calculateEnergy(double Rp)
        {
            Energy energy;
            double e;
            String formula;

            if (f1.Checked)
            {
                double cRp = Rp*2.7;
                e = (0.22 + 1.98*cRp + 0.0025*cRp*cRp);
                formula = "0,22+1,98*Rp +0,0025*Rp^2";

            }
            else if(f2.Checked)
            {
                e = (0.423 + 4.69 * Rp +0.0532 * Rp * Rp);
                formula = "0,423+4,69*Rp +0,0532*Rp^2";

            }
            else if (f3.Checked)
            {
                e = (0.2 + 5.09 * Rp);
                formula = "0,2+5,09*Rp";

            }
            else
            {
                e = 0;
                formula = "formula not selected";

            }

            energy = new Energy(formula, e, infoTextTextBox.Text, Rp);

            WritingToFile.WriteEnergyToFile(energy);
            return energy.Value.ToString();
        }

        private void culculateEnergy_Click(object sender, EventArgs e)
        {
            string energy = calculateEnergy(R).ToString();
            RLabel.Text = energy;
        }




        // ------------- calibration --------------
        private void calibrButton_Click(object sender, EventArgs e)
        {
            if (!calibrationBackgroundWorker.IsBusy)
            {
                calibrationBackgroundWorker.RunWorkerAsync();
            }
        }

        private void calibrationBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            CalibrationLogic.StartCalibration(_connectionParams, calibrationBackgroundWorker);
        }

        private void calibrationBackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            var status = e.UserState as CalibrationLogic.CalibrationProgress;

            if (status.CalibrationStarted)
            {
                calibrStatusLabel.Text = "Почалась калібрація";
            }
            else if(status.ReadValue)
            {
                calibrStatusLabel.Text = "Значення: " + status.Value.ToString();
            }
            else if(status.CalibrationFinished)
            {
                if (status.FinishedSuccesfully)
                {
                    calibrStatusLabel.Text = "Калібрація успішна: " + status.Value.ToString(); ;
                }
                else
                {
                    calibrStatusLabel.Text = "Не вдалось відкалібрувати";
                }

            }
            else
            {
                calibrStatusLabel.Text = "Помилка системи";
            }
        }




        // ------------- zero calibration --------------
        private void zerosCalibrationButton_Click(object sender, EventArgs e)
        {
            if (!zeroCalibrationBackgroundWorker.IsBusy)
            {
                zeroCalibrationBackgroundWorker.RunWorkerAsync();
            }
        }

        private void zeroCalibrationBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            CalibrationLogic.CalibrateZeros(_mesureParameters, _connectionParams, zeroCalibrationBackgroundWorker);
            e.Result = true;

        }

        private void zeroCalibrationBackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            zeroCalibrationProgressBar.Value = e.ProgressPercentage * 10;
            ZeroStepLabel.Text ="Крок: " + e.ProgressPercentage.ToString();
        }

        private void zeroCalibrationBackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            ZeroStepLabel.Text = "Калібрація нулів завершилась";
            InitSettings();
            updateBaseObjects();
            

        }



        // ----------  mesures -----------------------
        private void startButton_Click(object sender, EventArgs e)
        {
            if (!mesureBackgroundWorker.IsBusy)
            {
                mesureBackgroundWorker.RunWorkerAsync();

            }
        }

        private void mesureBackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            var status = e.UserState as MesureProgress;
            if (status.MesureStarted)
            {
                mesureProlabel.Text = "Вимір почався";
            }
            else if (status.ReadValue)
            {
                mesureProgressBar.Value = status.Step * 10;
                mesureProlabel.Text = "Крок: " + status.Step.ToString();
                semLabel.Text = status.Value[0].ToString();
                u1Label.Text = status.Value[1].ToString();
                u2Label.Text = status.Value[2].ToString();

            }
            else if (status.MesureFinished)
            {
                mesureProlabel.Text = "Вимір закінчився";
            }
            else
            {
                mesureProlabel.Text = "Помилка системи";
            }
        }

        private void mesureBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            _mesures = GeneralLogic.StartMesures(_mesureParameters, _connectionParams, _resistors, _diaphragms, _zeroDeviation, mesureBackgroundWorker);
           // _mesures = GeneralLogic.TestMesures1();
            WritingToFile.WriteMesureToFile(_mesures);

           // var pointPairList = GraphLogic.GetTDataForGraph(mesures);
            var pointPairList = GraphLogic.GetUDUSDataForGraph(_mesures);
           

           

            DrawLineGraph(pointPairList, new GraphParams("U_Diaphragm/U_SEM", "Diaphragm", "U_Diaphragm/U_SEM", 0.03, 0.05));
        }
    
        private void button2_Click(object sender, EventArgs e)
        {
             var pointPairList = GraphLogic.GetTDataForGraph(_mesures);
            DrawLineGraph(pointPairList, new GraphParams("T(D)", "T", "D", 0.03, 0.05));
            ShowMessageBox("виберіть лінійний участок");

        }

        private void initSEMChart(double [] semValuel)
        {
            if (SEMChart.Series.Count != 0)
            {
                SEMChart.Series.Clear();
                
            }

            SEMChart.Palette = ChartColorPalette.BrightPastel;
            Series series = SEMChart.Series.Add("SEM, U");

            for (int i = 0; i < semValuel.Length; i++)
            {
                series.Points.Add(semValuel[i]);
            }
        }


        private void showSEMUbutton_Click(object sender, EventArgs e)
        {
            var semEnergy = GraphLogic.GetSEMEnergyArray(_mesures);
            initSEMChart(semEnergy);
        }
    }

}
    

