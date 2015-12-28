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
using System.Windows.Threading;

namespace WpfApplication2.User_Controls
{
    /// <summary>
    /// Interaction logic for NewNode.xaml
    /// </summary>
    public partial class NewNodeView : UserControl, INotifyPropertyChanged
    {
        DispatcherTimer CountDownTimer = new DispatcherTimer();
        public const int WaitTime = 10;


        private string mark;
        public string Mark
        {
            get { return mark; }
            set
            {
                mark = value;
                NotifyPropertyChanged("Mark");
            }
        }

        private Brush markcolor;
        public Brush MarkColor
        {
            get { return markcolor; }
            set
            {
                markcolor = value;
                NotifyPropertyChanged("MarkColor");
            }
        }

        private string resulttext;
        public string ResultText
        {
            get { return resulttext; }
            set
            {
                resulttext = value;
                NotifyPropertyChanged("ResultText");
            }
        }

        private int timeleft;
        public int TimeLeft
        {
            get { return timeleft; }
            set
            {
                timeleft = value;
                NotifyPropertyChanged("TimeLeft");
            }
        }

        private Visibility resulttextvisability;
        public Visibility ResultTextVisability
        {
            get { return resulttextvisability; }
            set
            {
                resulttextvisability = value;
                ResultTextVisability = LookingTextVisability == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                NotifyPropertyChanged("ResultTextVisability");
            }
        }

        private Visibility lookingtextvisability;

        public Visibility LookingTextVisability
        {
            get { return lookingtextvisability; }
            set
            {
                lookingtextvisability = value;
                NotifyPropertyChanged("LookingTextVisability");
            }
        }

        public NewNodeView()
        {
            InitializeComponent();
            this.DataContext = this;
            CountDownTimer.Interval = TimeSpan.FromSeconds(1);
            CountDownTimer.IsEnabled = true;
            CountDownTimer.Tick += CountDownTimer_Tick;
        }

        internal void TimerStart()
        {
            TimeLeft = WaitTime;
            LookingTextVisability = Visibility.Visible;
            CountDownTimer.Start();
        }

        public void SetFail()
        {

            LookingTextVisability = Visibility.Collapsed;
            CountDownTimer.Stop();
            ResultText = "Unable to detect node";
            Mark = "X";
            MarkColor = Brushes.Red;
        }

        public void SetSuccess(List<byte> data)
        {
            LookingTextVisability = Visibility.Collapsed;
            CountDownTimer.Stop();
            ResultText = string.Format("Now node Bonded\nNode ID: {0}\nTotal Bonded nodes: {1}\nRun Discovary To View the map", data[0], data[1]);
            Mark = "\u221A";
            MarkColor = Brushes.Green;

        }

        private void CountDownTimer_Tick(object sender, EventArgs e)
        {
            --TimeLeft;
            if (TimeLeft == 0)
            {
                SetFail();
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
