using System;
using System.Windows;
using System.Windows.Media;
using IronScheme;
using OxyPlot.Wpf;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public MainWindow()
        {
            _viewModel = new MainViewModel();
            DataContext = _viewModel;

            CompositionTarget.Rendering += CompositionTargetRendering;

            InitializeComponent();
        }

        private void CompositionTargetRendering(object sender, EventArgs e)
        {
            _viewModel.UpdateModel();
            _viewModel.PlotModel.Update(true);
            _viewModel.PlotModel.InvalidatePlot(true);
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

        private void AddFunction(object sender, RoutedEventArgs e)
        {
            _viewModel.AddFunction();
            _viewModel.PlotModel.InvalidatePlot(true);
            
        }
    }
}
