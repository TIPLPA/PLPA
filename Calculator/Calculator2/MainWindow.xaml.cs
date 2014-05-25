using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public List<FunctionSeries> FunctionCollection;
        

        public MainWindow()
        {
            ThePlotModel = new PlotModel("Plot");

            XMin = 0;
            XMax = 10;
            YMin = 0;
            YMax = 10;

            SetupPlot();

            XLogarithmCheck = false;
            YLogarithmCheck = false;

            var dummyPlot = new FunctionSeries(Math.Cos, 0, 100, 0.1, "cos(x)");

            ThePlotModel.Series.Add(dummyPlot);
            ObsFunctionList = new ObservableCollection<FunctionList>();

            ObsFunctionList.Add(new FunctionList()
            {
                Name = "test1", 
                Function = new  FunctionSeries(Math.Sin,0, 100, 0.1, "sin(x)")
            });

            DataContext = this;

            InitializeComponent();

            CheckBoxs.DataContext = ObsFunctionList;
        }

        public int XMin { set; get; }
        public int XMax { get; set; }
        public int YMin { get; set; }
        public int YMax { get; set; }

        private void AddFunctionToPlot(FunctionList function)
        {
            ObsFunctionList.Add(function);
            ThePlotModel.Series.Clear();
            ThePlotModel.InvalidatePlot(true);

            foreach (var func in ObsFunctionList)
            {
                ThePlotModel.Series.Add(func.Function);
            }

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

        private void LinearClick(object sender, RoutedEventArgs e)
        {
            var path = Directory.GetCurrentDirectory(); 
            var file = File.ReadAllText(path + SchemePath + "linear_logrithmic function_task2.rkt");
            string function = string.Format("(linearFunc {0} {1} [0] (lambda (x) x))", XMin, XMax);

            var con = new Controller { Math = file, Interface = function};

            var tmp = new FunctionList()
            {
                Function = AddFunction(con),
                Name = "Linear"
            };
            //ObsFunctionList.Add(tmp);
            con.Title = string.Format("{0}*x + {1}", con.a, con.b);

            AddFunctionToPlot(tmp);
            //ThePlotModel.Series.Add(serie);
            //ThePlotModel.InvalidatePlot(true);
        }

        private void LogaClick(object sender, RoutedEventArgs e)
        {
            var path = Directory.GetCurrentDirectory();
            var file = File.ReadAllText(path + SchemePath + "linear_logrithmic function_task2.rkt");
            string function = string.Format("(logarithmicFunc {0} {1} [0] (lambda (x) x))", XMin, XMax);

            var con = new Controller { Math = file, Interface = function};

            var serie = AddFunction(con);
            con.Title = string.Format("log({0}*x)", con.a);

            ThePlotModel.Series.Add(serie);
            ThePlotModel.InvalidatePlot(true);
        }

        private void ExpoClick(object sender, RoutedEventArgs e)
        {
            var path = Directory.GetCurrentDirectory();
            var file = File.ReadAllText(path + SchemePath + "linear_logrithmic function_task2.rkt");
            string function = string.Format("(Exponential {0} {1} [0] (lambda (x) x))", XMin, XMax);

            var con = new Controller { Math = file, Interface = function, Title = "Exponential"};

            var serie = AddFunction(con);
            con.Title = string.Format("exp({0}*x)^{1}", con.a, con.b);

            ThePlotModel.Series.Add(serie);
            ThePlotModel.InvalidatePlot(true);
        }

        private void RootClick(object sender, RoutedEventArgs e)
        {
            var path = Directory.GetCurrentDirectory();
            var file = File.ReadAllText(path + SchemePath + "linear_logrithmic function_task2.rkt");
            string function = string.Format("(Root {0} {1} [0] (lambda (x) x))", XMin, XMax);

            var con = new Controller { Math = file, Interface = function, Title = "Root"};

            var serie = AddFunction(con);
            con.Title = string.Format("root({0}*x)^(-{1})", con.a, con.b);

            ThePlotModel.Series.Add(serie);
            ThePlotModel.InvalidatePlot(true);
        }
        #endregion

        private LineSeries AddFunction(Controller con)
        {
            var window = new PopUp(con);
            window.ShowDialog();

            var serie = new LineSeries();
            serie.Title = con.Title;

            foreach (var datapoint in con.Data)
            {
                double x = datapoint.car;
                double y = datapoint.cdr;
                serie.Points.Add(new DataPoint(x, y));
            }
            return serie;
        }

        private ObservableCollection<FunctionList> _observable; 
        public ObservableCollection<FunctionList> ObsFunctionList
        {
            get { return _observable; } 
            set { _observable = value; }
        }

        public class FunctionList
        {
            public string Name { get; set; }
            public Series Function { get; set; }
        }

    }

    //public class FunctionList
    //{
    //    public string Name { get; set; }
    //    public FunctionSeries Function { get; set; }
    //}
}
