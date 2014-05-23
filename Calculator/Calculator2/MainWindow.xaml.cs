using System;
using System.Windows;
using IronScheme;
using OxyPlot;
using OxyPlot.Series;

namespace Calculator2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PlotModel ThePlotModel { get; private set; }
        

        public MainWindow()
        {
            ThePlotModel = new PlotModel("Example 1");
            ThePlotModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));

            DataContext = this;

            XMin = 0;
            XMax = 0;
            YMin = 0;
            YMax = 0;

            InitializeComponent();
        }

        public int XMin { set; get; }
        public int XMax { get; set; }
        public int YMin { get; set; }
        public int YMax { get; set; }

        private void AddFunction(object sender, RoutedEventArgs e)
        {
            ThePlotModel.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 0.1, "sin(x)"));
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
    }
}
