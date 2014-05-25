using System;
using System.Globalization;
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
            AValue = 1;
            BValue = 1;
            Pre = controller.Pre;
            Mid = controller.Mid;
            Post = controller.Post;
            TypeOfFunction = controller.Title;

            DataContext = this;

            InitializeComponent();
        }

        private double _a, _b;

        public string TypeOfFunction { get; set; }
        public double AValue
        {
            get { return _a; }
            set
            {
                _controller.a = value;
                _a = value;
            }
        }
        public double BValue
        {
            get { return _b; }
            set
            {
                _controller.b = value;
                _b = value;
            }
        }
        public string Pre { get; set; }
        public string Mid { get; set; }
        public string Post { get; set; }

        private void Calculate(object sender, RoutedEventArgs e)
        {
            _controller.Interface = _controller.Interface.Replace("[0]", AValue.ToString(CultureInfo.InvariantCulture));
            _controller.Interface = _controller.Interface.Replace("[1]", BValue.ToString(CultureInfo.InvariantCulture));
            var tmp = string.Format(_controller.Math + _controller.Interface);
            _controller.Data = SchemeCalculation(tmp);

            Close();
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
