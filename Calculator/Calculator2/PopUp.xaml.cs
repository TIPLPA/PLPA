using System;
using System.Windows;
using IronScheme;

namespace Calculator2
{
    /// <summary>
    /// Interaction logic for PopUp.xaml
    /// </summary>
    public partial class PopUp : Window
    {
        private Controller _controller;

        public PopUp(Controller controller)
        {
            _controller = controller;
            _controller.a = 1;
            _controller.b = 0;

            DataContext = this;

            InitializeComponent();
        }

        private void Calculate(object sender, RoutedEventArgs e)
        {
            _controller.Interface = _controller.Interface.Replace("[0]", _controller.a.ToString());
            //_controller.Interface = _controller.Interface.Replace("[1]", _controller.b.ToString());
            var tmp = string.Format(_controller.Math + _controller.Interface, _controller.a);//, _controller.b);
            _controller.Data = SchemeCalculation(tmp);
            this.Close();

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
    }
}
