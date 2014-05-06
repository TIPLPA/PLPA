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
            dynamic input = CodeBox.Text.Eval();
            var data = input.ToString();
            MessageBox.Show(data);
        }
    }
}
