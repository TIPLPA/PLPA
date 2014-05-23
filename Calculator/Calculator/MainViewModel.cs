using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Calculator.Annotations;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using LinearAxis = OxyPlot.Axes.LinearAxis;

namespace Calculator
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private PlotModel _plotModel;
        public PlotModel PlotModel1
        {
            get { return _plotModel; }
            set
            {
                _plotModel = value;
                OnPropertyChanged("PlotModel1");
                PlotModel1.InvalidatePlot(true);
            }
        }

        public MainViewModel()
        {
            _plotModel = new PlotModel();
            //PlotModel.InvalidatePlot(true);

            //SetUpModel();
            //LoadData();

            PlotModel1.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
            //PlotModel.Series.Add(new FunctionSeries(Math.Abs, 0, 2, 0.1, "abs(x)"));
            
            //_yLower = 0;
            //_yUpper = 10;
            //_xLower = 0;
            //_xUpper = 10;
        }

        private int _yLower, _yUpper, _xLower, _xUpper;

        private void UpdateAxis(int xl , int xu , int yl , int yu )
        {
            var axis = new LinearAxis(AxisPosition.Bottom, xl, xu);

            PlotModel1.Axes.Clear();
            PlotModel1.Axes.Add(axis);
            PlotModel1.InvalidatePlot(true);
        }

        #region Properties

        public int YLower
        {
            set
            {
                if (value != _yLower)
                {
                    _yLower = value;
                    OnPropertyChanged("YLower");
                    UpdateAxis(XLower, XUpper, YLower, YUpper);
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
                    UpdateAxis(XLower, XUpper, YLower, YUpper);
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
                    UpdateAxis(value, XUpper, YLower, YUpper);
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
                    UpdateAxis(XLower, XUpper, YLower, YUpper);
                }
            }
            get { return _xUpper; }
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) 
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #endregion

        private void SetUpModel()
        {
            //PlotModel.LegendTitle = "Legend";
            //_plotModel.LegendOrientation = LegendOrientation.Horizontal;
            //_plotModel.LegendPlacement = LegendPlacement.Outside;
            //_plotModel.LegendPosition = LegendPosition.TopRight;
            //_plotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            //_plotModel.LegendBorder = OxyColors.Black;

            //var dateAxis = new DateTimeAxis(AxisPosition.Bottom, "Date", "dd/MM/yy HH:mm");
            //dateAxis.MajorGridlineStyle = LineStyle.Solid;
            //dateAxis.MinorGridlineStyle = LineStyle.Dot;
            //dateAxis.IntervalLength = 80;
            //PlotModel.Axes.Add(dateAxis);

            //var valueAxis = new LinearAxis(AxisPosition.Left, 0);
            //valueAxis.MajorGridlineStyle = LineStyle.Solid;
            //valueAxis.MinorGridlineStyle = LineStyle.Dot;
            //valueAxis.Title = "Value";
            //PlotModel.Axes.Add(valueAxis);
        }

        private void LoadData()
        {
            //List<Measurement> measurements = Data.GetData();

            //var dataPerDetector = measurements.GroupBy(m => m.DetectorId).ToList();

            //foreach (var data in dataPerDetector)
            //{
            //    var lineSerie = new LineSeries
            //    {
            //        StrokeThickness = 2,
            //        MarkerSize = 3,
            //        MarkerStroke = colors[data.Key],
            //        MarkerType = markerTypes[data.Key],
            //        CanTrackerInterpolatePoints = false,
            //        Title = string.Format("Detector {0}", data.Key),
            //        Smooth = false,
            //    };

            //    data.ToList().ForEach(d => lineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.DateTime), d.Value)));
            //    PlotModel.Series.Add(lineSerie);
            //}
        }

        public void UpdateModel()
        {
            //List<Measurement> measurements = Data.GetUpdateData(lastUpdate);
            //var dataPerDetector = measurements.GroupBy(m => m.DetectorId).OrderBy(m => m.Key).ToList();

            //foreach (var data in dataPerDetector)
            //{
            //    var lineSerie = PlotModel.Series[data.Key] as LineSeries;
            //    if (lineSerie != null)
            //    {
            //        data.ToList()
            //            .ForEach(d => lineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.DateTime), d.Value)));
            //    }
            //}
            //lastUpdate = DateTime.Now;
        }

        public void AddFunction()
        {
            PlotModel1.Series.Add(new FunctionSeries(Math.Sin, 0, 10, 0.1, "sin(x)"));
            PlotModel1.Series.Add(new FunctionSeries(Math.Abs, 0, 2, 0.1, "abs(x)"));
            PlotModel1.Axes.Clear();
            PlotModel1.InvalidatePlot(true);
        }
    }
}
