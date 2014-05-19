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

        private MainViewModel viewModel;

        public MainWindow()
        {

            viewModel = new MainViewModel();
            DataContext = viewModel;

            InitializeComponent();
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
