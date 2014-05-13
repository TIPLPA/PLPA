using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Calculator.Annotations;
using OxyPlot;
using OxyPlot.Series;

namespace Calculator
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (value != _isEnabled)
                {
                    _isEnabled = value;
                    OnPropertyChanged("IsEnabled");
                }
            }
        }

        public MainViewModel()
        {
            this.MyModel = new PlotModel("Example 1");
            this.MyModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));

            _yLower = 0;
            _yUpper = 10;
            _xLower = 0;
            _xUpper = 10;
        }

        public PlotModel MyModel { get; private set; }
        private int _yLower, _yUpper, _xLower, _xUpper;

        #region Properties

        public int YLower
        {
            set
            {
                if (value != _yLower)
                {
                    _yLower = value;
                    OnPropertyChanged("YLower");
                }
            }
            get { return _yLower; }
        }

        public int YUpper
        {
            set
            {
                if (value != _yUpper)
                {
                    _yUpper = value;
                    OnPropertyChanged("YUpper");
                }
            }
            get { return _yUpper; }
        }

        public int XLower
        {
            set
            {
                if (value != _xLower)
                {
                    _xLower = value;
                    OnPropertyChanged("XLower");
                }
            }
            get { return _xLower; }
        }

        public int XUpper
        {
            set
            {
                if (value != _xUpper)
                {
                    _xUpper = value;
                    OnPropertyChanged("XUpper");
                }
            }
            get { return _xUpper; }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
