using System.Windows;
using IronScheme;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateClick(object sender, RoutedEventArgs e)
        {
            dynamic input = SchemeCalculation(CodeBox.Text);
            var data = input.ToString();
            MessageBox.Show(data);
        }

        private dynamic SchemeCalculation(string inputString)
        {
            return inputString.Eval();
        }
    }
}
