using NUnit.Framework;
using OxyPlot;

namespace Calculator2
{
    [TestFixture, RequiresSTA]
    public class TestMainWindow
    {
        private Controller _controller;
        private MainWindow _main;

        [SetUp]
        public void Init()
        {
            _controller = new Controller();
            _main = new MainWindow();

            _controller.Data = "'(1 . 1)";
        }

        [TearDown]
        public void TearDown()
        {
            _controller = null;
        }

        [Test]
        public void CalculateClick_simpleFunction_IsAdded()
        {
            _main.XMin = 0;
            _main.XMax = 10;
            _main.DataPoints = 2;
            _main.CodeBox.Text = "(lambda (x) (expt x 2))";
            _main.CalculateClick(null, null);

            Assert.IsTrue(_main.ObsFunctionList.Count == 1);
        }
    }


    [TestFixture, RequiresSTA]
    public class TestMainWindow
    {
        private Controller _controller;
        private MainWindow _main;

        [SetUp]
        public void Init()
        {
            _controller = new Controller();
            _main = new MainWindow();

            _controller.Data = "'(1 . 1)";
        }

        [TearDown]
        public void TearDown()
        {
            _controller = null;
        }

        [Test]
        public void CalculateClick_simpleFunction_IsAdded()
        {
            _main.XMin = 0;
            _main.XMax = 10;
            _main.DataPoints = 2;
            _main.CodeBox.Text = "(lambda (x) (expt x 2))";
            _main.CalculateClick(null, null);

            Assert.IsTrue(_main.ObsFunctionList.Count == 1);
        }
        
        [Test]
        public void CalculateClick_logFunctionMinZero_Error()
        {
            _main.XMin = 0;
            _main.XMax = 10;
            _main.DataPoints = 2;
            _main.CodeBox.Text = "(lambda (x) (log x))";
            _main.CalculateClick(null, null);

            Assert.IsTrue(_main.ObsFunctionList.Count == 0);
        }

        [Test]
        public void CalculateClick_logFunctionMinZeroPlus_Correct()
        {
            _main.XMin = 1;
            _main.XMax = 10;
            _main.DataPoints = 2;
            _main.CodeBox.Text = "(lambda (x) (log x))";
            _main.CalculateClick(null, null);

            Assert.IsTrue(_main.ObsFunctionList.Count == 1);
        }
         
    }
}