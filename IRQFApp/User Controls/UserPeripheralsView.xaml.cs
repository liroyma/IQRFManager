using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication2.User_Controls
{
    /// <summary>
    /// Interaction logic for UserPeripheralsView.xaml
    /// </summary>
    public partial class UserPeripheralsView : UserControl, INotifyPropertyChanged
    {
        private GradientStopCollection red_col;
        private GradientStopCollection green_col;
        private GradientStopCollection white_col;
        private LinearGradientBrush Red;
        private LinearGradientBrush Green;
        private LinearGradientBrush White;

        private LinearGradientBrush redledcolor;
        public LinearGradientBrush RedLedColor
        {
            get { return redledcolor; }
            set
            {
                redledcolor = value;
                NotifyPropertyChanged("RedLedColor");
            }
        }

        private LinearGradientBrush greenledcolor;
        public LinearGradientBrush GreenLedColor
        {
            get { return greenledcolor; }
            set
            {
                greenledcolor = value;
                NotifyPropertyChanged("GreenLedColor");
            }
        }

        private string redledstate;
        public string RedLedState
        {
            get { return redledstate; }
            set
            {
                redledstate = value;
                NotifyPropertyChanged("RedLedState");
            }
        }

        private string greenledstate;
        public string GreenLedState
        {
            get { return greenledstate; }
            set
            {
                greenledstate = value;
                NotifyPropertyChanged("GreenLedState");
            }
        }


        public UserPeripheralsView(byte pcmd)
        {
            InitializeComponent();
            this.DataContext = this;

            red_col = new GradientStopCollection();
            red_col.Add(new GradientStop(Color.FromRgb(245, 17, 17), 0.0));
            red_col.Add(new GradientStop(Color.FromRgb(189, 49, 49), 1.0));
            Red = new LinearGradientBrush(red_col);

            green_col = new GradientStopCollection();
            green_col.Add(new GradientStop(Color.FromRgb(17, 245, 17), 0.0));
            green_col.Add(new GradientStop(Color.FromRgb(49, 189, 49), 1.0));
            Green = new LinearGradientBrush(green_col);

            white_col = new GradientStopCollection();
            white_col.Add(new GradientStop(Color.FromRgb(255, 255, 255), 0.0));
            white_col.Add(new GradientStop(Color.FromRgb(228, 228, 228), 1.0));
            White = new LinearGradientBrush(white_col);
            GreenLedColor = White;
            RedLedColor = White;
            GreenLedState = "OFF";
            RedLedState = "OFF";
        }

        public void updateView(byte pcmd)
        {
            switch (pcmd)
            {
                case 0:
                    GreenLedColor = Green;
                    RedLedColor = White;
                    GreenLedState = "ON";
                    RedLedState = "OFF";
                    break;
                case 1:
                    GreenLedColor = White;
                    RedLedColor = Red;
                    GreenLedState = "OFF";
                    RedLedState = "ON";
                    break;
            }

        }

        #region INotifyPropertyChanged Handler

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
