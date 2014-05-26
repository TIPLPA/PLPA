using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
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
        public ObservableCollection<FunctionList> ObsFunctionList { get; set; }

        public MainWindow()
        {
            ThePlotModel = new PlotModel("Plot");
            ObsFunctionList = new ObservableCollection<FunctionList>();

            SetupPlot();

            XLogarithmCheck = false;
            YLogarithmCheck = false;

            DataContext = this;
            InitializeComponent();
            ItemList.ItemsSource = ObsFunctionList;
            ThePlotModel.InvalidatePlot(true);
        }

        public int XMin { set; get; }
        public int XMax { get; set; }
        public int YMin { get; set; }
        public int YMax { get; set; }

        private int _dataPoint;
        public int DataPoints 
        { 
            get { return _dataPoint; }
            set { _dataPoint = value < 2 ? 2 : value;}
        }

        public void AddFunctionToPlot(FunctionList function)
        {
            function.IsChecked = true;
            ObsFunctionList.Add(function);

            ThePlotModel.Series.Add(function.Function);

            ThePlotModel.InvalidatePlot(true);
        }

        #region Logarithm check
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

        #endregion

        private void SetupPlot()
        {
            XMin = 0;
            XMax = 10;
            YMin = 0;
            YMax = 10;
            DataPoints = 10;

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

            if (log)
                axis1 = new LogarithmicAxis();
            else
                axis1 = new LinearAxis();

            if (axis == 1)
                axis1.Position = AxisPosition.Bottom;
            else
                axis1.Position = AxisPosition.Left;

            axis1.MinorStep = ((XMax - XMin) / 5) % 5; // 5 steps on axis
            if (axis1.MinorStep < 1)
                axis1.MinorStep = 1;
            axis1.MinorGridlineStyle = LineStyle.Dot;

            axis1.Minimum = (axis == 2 ? YMin : XMin);
            axis1.Maximum = (axis == 2 ? YMax : XMax);

            ThePlotModel.Axes[axis - 1] = axis1;

            ThePlotModel.InvalidatePlot(true);
        }

        public void CalculateClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var path = Directory.GetCurrentDirectory();
                var file = File.ReadAllText(path + SchemePath + FileName);
                string function = string.Format("(linearFunc {0} {1} {2} {3})", XMin, XMax, DataPoints, CodeBox.Text);

                var DataString = string.Format(file + function);
                dynamic data = SchemeCalculation(DataString);

                var functionList = new FunctionList()
                {
                    Function = CreateFunction(data, LineStyle.Solid),
                    Name = "Custom",
                    SchemeFunction = function,
                    IsNotDerivative = true,
                    IsNotIntegral = true,
                    IsChecked = true,
                };

                if (functionList.Function == null)
                    return;

                functionList.ID = _idCounter++;
                AddFunctionToPlot(functionList);           
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sorry to bother you!");
            }
        }

        private dynamic SchemeCalculation(string inputString)
        {
            try
            {
                dynamic item = inputString.Eval();
                return item;
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

        private const string SchemePath = "/../../../SchemeFiles/";
        private const string FileName = "linear_logrithmic_function_task2.rkt";
        private const string FileDerivative = "derivative_integral_task3.rkt";

        private void LinearClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var path = Directory.GetCurrentDirectory();
                var file = File.ReadAllText(path + SchemePath + FileName);
                string function = string.Format("(linearFunc {0} {1} 1 (lambda (x) (+ (* [0] x) [1])))", XMin, XMax);

                var con = new Controller
                {
                    Math = file, 
                    Interface = function,
                    Pre = "",
                    Mid = "*x+ ",
                    Post = ""
                };

                var tmp = new FunctionList()
                {
                    Function = CreateFunction(con, LineStyle.Solid),
                    Name = "Linear",
                    SchemeFunction = function,
                    IsNotDerivative = true,
                    IsChecked = true,
                    ID = _idCounter++
                };

                tmp.Function.Title = string.Format("{0}*x " + (con.b > 1? "+" : "") + " {1}", con.a, con.b);

                AddFunctionToPlot(tmp);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void LogaClick(object sender, RoutedEventArgs e)
        {
            var path = Directory.GetCurrentDirectory();
            var file = File.ReadAllText(path + SchemePath + FileName);
            string function = string.Format("(logarithmicFunc {0} {1} 1 (lambda (x) (+ (expt (log x) [0]) [1])))", XMin, XMax);

            var con = new Controller
            {
                Math = file,
                Interface = function,
                Pre = "log(",
                Mid = "^x)*",
                Post = ""
            };

            var tmp = new FunctionList()
            {
                Function = CreateFunction(con, LineStyle.Solid),
                Name = "Logarithmic",
                SchemeFunction = function,
                IsNotDerivative = true,
                IsChecked = true,
                ID = _idCounter++
            };

            tmp.Function.Title = string.Format("log({0}^x) + {1}", con.a, con.b);

            AddFunctionToPlot(tmp);
        }

        //private void ExpoClick(object sender, RoutedEventArgs e)
        //{
        //    var path = Directory.GetCurrentDirectory();
        //    var file = File.ReadAllText(path + SchemePath + FileName);
        //    string function = string.Format("(Exponential {0} {1} 1 (lambda (x) (+ ([0] x) [1])", XMin, XMax);

        //    var con = new Controller { Math = file, Interface = function, Title = "Exponential" };

        //    var serie = CreateFunction(con,true);
        //    con.Title = string.Format("exp({0}*x)^{1}", con.a, con.b);

        //    ThePlotModel.Series.Add(serie);
        //    ThePlotModel.InvalidatePlot(true);
        //}

        //private void RootClick(object sender, RoutedEventArgs e)
        //{
        //    var path = Directory.GetCurrentDirectory();
        //    var file = File.ReadAllText(path + SchemePath + FileName);
        //    string function = string.Format("(Root {0} {1} [0] (lambda (x) x))", XMin, XMax);

        //    var con = new Controller { Math = file, Interface = function, Title = "Root" };

        //    var serie = CreateFunction(con,true);
        //    con.Title = string.Format("root({0}*x)^(-{1})", con.a, con.b);

        //    ThePlotModel.Series.Add(serie);
        //    ThePlotModel.InvalidatePlot(true);
        //}
        #endregion

        private int _idCounter = 0;
        public LineSeries CreateFunction(dynamic data, LineStyle style, LineSeries line = null)
        {
            try
            {
                var serie = new LineSeries();
                if(line != null)
                    serie = line;

                serie.LineStyle = style;

                foreach (var datapoint in data)
                {
                    double x = (double)datapoint.car;
                    double y = (double)datapoint.cdr;
                    serie.Points.Add(new DataPoint(x, y));
                }
                return serie;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public class FunctionList
        {
            public string Name { get; set; }
            public int ID { get; set; }
            public Series Function { get; set; }
            public bool IsChecked { get; set; }

            public bool ShowDerivative { get; set; }
            public string SchemeFunction { get; set; }
            public bool IsNotDerivative { get; set; }
            public bool IsNotIntegral { get; set; }
        }

        private void CheckIDClick(object sender, RoutedEventArgs e)
        {
            var ID = (int)((CheckBox)sender).Tag;
            ThePlotModel.Series[ID].IsVisible = ObsFunctionList.First(t => t.ID == ID).IsChecked;
            ObsFunctionList.First(t => t.ID == ID).IsChecked = !ObsFunctionList.First(t => t.ID == ID).IsChecked;
            ThePlotModel.InvalidatePlot(true);
        }

        private void DerivativeClick(object sender, RoutedEventArgs e)
        {
            if( ((CheckBox)sender).IsChecked == false)
                return;
            try
            {
                var path = Directory.GetCurrentDirectory();
                var file = File.ReadAllText(path + SchemePath + FileDerivative);
                string function = string.Format("(derivativeFunc {0} {1} {2} {3})", XMin, XMax, DataPoints, CodeBox.Text);

                var dataString = string.Format(file + function);
                dynamic data = SchemeCalculation(dataString);

                var functionList = new FunctionList()
                {
                    Function = CreateFunction(data, LineStyle.Dot),
                    Name = "Derivative",
                    SchemeFunction = function,
                    IsNotDerivative = false,
                    IsNotIntegral = true,
                    IsChecked = true,
                };

                if (functionList.Function == null)
                    return;

                functionList.ID = _idCounter++;
                AddFunctionToPlot(functionList); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sorry to bother you!");
            }
        }

        private void IntegralClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((CheckBox)sender).IsChecked == false)
                    return;

                var path = Directory.GetCurrentDirectory();
                var file = File.ReadAllText(path + SchemePath + FileName);

                var enable = ObsFunctionList.First(t => t.ID == (int) ((CheckBox) sender).Tag).IsNotDerivative;

                string function = "";
                
                if (enable == false)
                    function = string.Format("(derivativeFunc {0} {1} {2} {3})", XMin, XMax, DataPoints, CodeBox.Text);
                else
                    function = string.Format("(linearFunc {0} {1} {2} {3})", XMin, XMax, DataPoints, CodeBox.Text);

                var dataString = string.Format(file + function);
                dynamic data = SchemeCalculation(dataString);

                LineSeries dataline = CreateFunction(data, LineStyle.Solid, new AreaSeries());

                var serie = new AreaSeries();

                serie.Points.Add(new DataPoint(XMin, 0));
                foreach (var dataPoint in dataline.Points)
                {
                    serie.Points.Add(new DataPoint(dataPoint.X, dataPoint.Y));
                }
                serie.Points.Add(new DataPoint(XMax, 0));

                var functionList = new FunctionList()
                {
                    Function = serie,
                    Name = "integral",
                    SchemeFunction = function,
                    IsNotDerivative = false,
                    IsNotIntegral = false,
                    IsChecked = true,
                };

                if (functionList.Function == null)
                    return;

                functionList.ID = _idCounter++;
                AddFunctionToPlot(functionList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sorry to bother you!");
            }
        }
    }

}
