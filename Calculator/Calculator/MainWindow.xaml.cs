using System;
using System.Windows;
using IronScheme;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            _viewModel = new MainViewModel();
            DataContext = _viewModel;

            InitializeComponent();
        }


        private void CalculateClick(object sender, RoutedEventArgs e)
        {
            //dynamic input = SchemeCalculation(CodeBox.Text);

            //if (input != null)
            //{
            //    var data = input.ToString();
            //    MessageBox.Show(data);
            //}
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
            _viewModel.PlotModel1.InvalidatePlot(true);           
        }
    }
}
