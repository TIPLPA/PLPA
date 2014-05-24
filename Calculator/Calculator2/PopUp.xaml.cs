using System.Windows;

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

            DataContext = this;

            InitializeComponent();
        }
    }
}
