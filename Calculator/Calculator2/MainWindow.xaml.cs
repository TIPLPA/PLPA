using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using IronScheme;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Calculator2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PlotModel ThePlotModel { get; private set; }

        public List<FunctionSeries> FunctionCollection;
        

        public MainWindow()
        {
            ThePlotModel = new PlotModel("Example 1");

            XMin = 0;
            XMax = 10;
            YMin = 0;
            YMax = 10;

            SetupPlot();

            XLogarithmCheck = false;
            YLogarithmCheck = false;

            FunctionCollection = new List<FunctionSeries>();

            var dummyPlot = new FunctionSeries(Math.Cos, 0, 100, 0.1, "cos(x)");
            ThePlotModel.Series.Add(dummyPlot);

            DataContext = this;

            InitializeComponent();
        }

        public int XMin { set; get; }
        public int XMax { get; set; }
        public int YMin { get; set; }
        public int YMax { get; set; }

        private void AddFunction(object sender, RoutedEventArgs e)
        {
            var func = new FunctionSeries(Math.Sin, 0, 10, 0.1, "sin(x)");
            AddFunctionToPlot(func);
        }

        private void AddFunctionToPlot(FunctionSeries function)
        {
            FunctionCollection.Add(function);
            ThePlotModel.Series.Clear();

            foreach (var func in FunctionCollection)
            {
                ThePlotModel.Series.Add(func);
            }

            ThePlotModel.InvalidatePlot(true);
        }

        private bool _xlogarithmCheck;
        public bool XLogarithmCheck 
        {
            get { return _xlogarithmCheck; }
            set
            {
                _xlogarithmCheck = value;
                SetScale(1, value);
            }
        }

        private bool _ylogarithmCheck;
        public bool YLogarithmCheck
        {
            get { return _ylogarithmCheck; }
            set
            {
                _ylogarithmCheck = value;
                SetScale(2, value);
            }
        }

        private void SetupPlot()
        {
            var xAxis = new LinearAxis();
            xAxis.Minimum = XMin;
            xAxis.Maximum = XMax;
            ThePlotModel.Axes.Add(xAxis);

            var yAxis = new LinearAxis();
            yAxis.Minimum = YMin;
            yAxis.Maximum = YMax;
            ThePlotModel.Axes.Add(yAxis);
        }

        private void SetScale(int axis, bool log)
        {
            Axis axis1;

            if(log)
                axis1 = new LogarithmicAxis();
            else
                axis1 = new LinearAxis();

            if(axis == 1)
                axis1.Position = AxisPosition.Bottom;
            else
                axis1.Position = AxisPosition.Left;

            axis1.MinorStep = ((XMax - XMin)/5)%5; // 5 steps on axis
            if (axis1.MinorStep < 1)
                axis1.MinorStep = 1;
            axis1.MinorGridlineStyle = LineStyle.Dot;

            axis1.Minimum = (axis == 2? YMin : XMin);
            axis1.Maximum = (axis == 2? YMax : XMax);

            ThePlotModel.Axes[axis - 1] = axis1;

            ThePlotModel.InvalidatePlot(true);
        }

        private void CalculateClick(object sender, RoutedEventArgs e)
        {
            dynamic input = SchemeCalculation(CodeBox.Text);

            if (input != null)
            {
                var data = input.ToString();
                MessageBox.Show(data);
            }
        }

        private dynamic SchemeCalculation(string inputString)
        {
            try
            {
                return inputString.Eval();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }


        #region Scale update
        private void XMinChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                var temp = int.Parse(XMinBox.Text);

                if (temp < XMax)
                {
                    XMin = temp;
                    SetScale(1, XLogarithmCheck);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void XMaxChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                var temp = int.Parse(XMaxBox.Text);

                if (temp > XMin)
                {
                    XMax = temp;
                    SetScale(1, XLogarithmCheck);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void YMinChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                var temp = int.Parse(YMinBox.Text);

                if (temp < YMax)
                {
                    YMin = temp;
                    SetScale(2, YLogarithmCheck);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void YMaxChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                var temp = int.Parse(YMaxBox.Text);

                if (temp > YMin)
                {
                    YMax = temp;
                    SetScale(2, YLogarithmCheck);
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Function click

        private void LinearClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LogaClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ExpoClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RootClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
