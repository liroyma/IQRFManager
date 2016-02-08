using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Windows.Input;
using WpfApplication2.Helpers;

namespace WpfApplication2.User_Controls
{
    /// <summary>
    /// Interaction logic for Rs232View.xaml
    /// </summary>
    public partial class Rs232View : UserControl, INotifyPropertyChanged
    {

        DispatcherTimer Timer = new DispatcherTimer();


        private static int[] BaudRatesArray = { 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200 };

        private ObservableCollection<int> baudrates = new ObservableCollection<int>();
        public ObservableCollection<int> BaudRates
        {
            get { return baudrates; }
            set
            {
                baudrates = value;
                NotifyPropertyChanged("BaudRates");
            }
        }

        private int selectedbaudrate;
        public int SelectedBaudRate
        {
            get { return selectedbaudrate; }
            set
            {
                selectedbaudrate = value;
                NotifyPropertyChanged("SelectedBaudRate");
            }
        }

        private string buttontext;
        public string ButtonText
        {
            get { return buttontext; }
            set
            {
                buttontext = value;
                NotifyPropertyChanged("ButtonText");
            }
        }

        private bool ischangeablebaudrate;
        public bool IsChangeableBaudRate
        {
            get { return ischangeablebaudrate; }
            set
            {
                ischangeablebaudrate = value;
                NotifyPropertyChanged("IsChangeableBaudRate");
            }
        }

        private bool isconnected;
        public bool IsConnected
        {
            get { return isconnected; }
            set
            {
                isconnected = value;
                IsChangeableBaudRate = !isconnected;
                ButtonText = isconnected ? "Disconnect" : "Connect";
                //ConnectionButtenColor = isconnected ? Colors.LightCoral : Colors.LightGreen;
                NotifyPropertyChanged("IsConnected");
            }
        }

        #region Rs232Connect
        private ICommand _connectCommand;
        public ICommand ConnectCommand
        {
            get
            {
                return _connectCommand ?? (_connectCommand = new CommandHandler((string ms) => ConnectAction(), true));
            }
        }

        public void ConnectAction()
        {
            IsConnected = !IsConnected;

            if (IsConnected)
            {
                Uart.OpenRS232Communication(SelectedBaudRate);
            }
            else
            {
                Uart.CloseRS232Communication();
            }
        }
        #endregion

        #region SendRs232Command

        public class RS232Message
        {
            public string ASCIIMessage { get; set; }
            public string HEXMessage   { get; set; }

            public RS232Message(string message)
            {
                ASCIIMessage = message;
                HEXMessage = Utility.StringToFormatedASCIIString(ASCIIMessage, ',');
            }
        }

        private string _messageToSend;
        public string MessageToSend
        {
            get { return _messageToSend; }
            set
            {
                _messageToSend = value;
                NotifyPropertyChanged("MessageToSend");
            }
        }

        private ObservableCollection<RS232Message> _rs232messagecollection = new ObservableCollection<RS232Message>();
        public ObservableCollection<RS232Message> RS232MessageCollection
        {
            get { return _rs232messagecollection; }
            set
            {
                _rs232messagecollection = value;
                NotifyPropertyChanged("RS232MessageCollection");
            }
        }

        private ICommand _sendRs232MessageCommand;
        public ICommand SendRS232MessageCommand
        {
            get
            {
                return _sendRs232MessageCommand ?? (_sendRs232MessageCommand = new CommandHandler((string ms) => SendMessageAction(), true));
            }
        }

        public void SendMessageAction()
        {
            RS232MessageCollection.Insert(0, new RS232Message(MessageToSend));
            Uart.SendRS232Command(RS232MessageCollection.ElementAt(0).HEXMessage);
            MessageToSend = "";
        }


        #endregion

        private UART Uart;

        public Rs232View(IQRFAction uart)
        {
            InitializeComponent();
            this.DataContext = this;

            Uart = (UART)uart;

            BaudRates.Clear();
            foreach (var item in BaudRatesArray)
            {
                BaudRates.Add(item);
            }
            SelectedBaudRate = BaudRates.Count > 0 ? BaudRates.First() : -1 ;

            IsConnected = false;
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
