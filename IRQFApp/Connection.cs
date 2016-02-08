using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WpfApplication2.Actions;

namespace WpfApplication2
{
    class Connection : INotifyPropertyChanged
    {
        System.IO.Ports.SerialPort comPort = new System.IO.Ports.SerialPort();
        IQRFAction action;

        DispatcherTimer Timer = new DispatcherTimer();

        #region UI Properties
        private int nodenumber;
        public int NodeNumber
        {
            get { return nodenumber; }
            set
            {
                nodenumber = value;
                TextOnButton = nodenumber == 255 ? "BroadCast" : (nodenumber == 0 ? "Coordinator" : "Node " + nodenumber);
                NodeNumberVisibility = (IsBroadCast || NodeNumber < 1) ? Visibility.Hidden : Visibility.Visible;
                NotifyPropertyChanged("NodeNumber");
            }
        }

       

        private bool enablenodenumber = true;
        public bool NodeNumberEnable
        {
            get { return enablenodenumber; }
            set
            {
                enablenodenumber = value;
                NotifyPropertyChanged("NodeNumberEnable");
            }
        }

        private bool isbroadcast;
        public bool IsBroadCast
        {
            get { return isbroadcast; }
            set
            {
                isbroadcast = value;
                NodeNumber = isbroadcast ? 255 : 1;
                NodeNumberEnable = !isbroadcast;
                NodeNumberVisibility = (IsBroadCast || NodeNumber < 1) ? Visibility.Hidden : Visibility.Visible;
                NotifyPropertyChanged("IsBroadCast");
            }
        }

        private Visibility nodenumbervisibility;
        public Visibility NodeNumberVisibility
        {
            get { return nodenumbervisibility; }
            set
            {
                nodenumbervisibility = value;
                NotifyPropertyChanged("NodeNumberVisibility");
            }
        }

        private string textonbutton;
        public string TextOnButton
        {
            get { return textonbutton; }
            set
            {
                textonbutton = value;
                NotifyPropertyChanged("TextOnButton");
            }
        }

        private ObservableCollection<Message> messages = new ObservableCollection<Message>();
        public ObservableCollection<Message> Messages
        {
            get { return messages; }
            set
            {
                messages = value;
                NotifyPropertyChanged("Messages");
            }
        }

        private ObservableCollection<string> ports = new ObservableCollection<string>();
        public ObservableCollection<string> Ports
        {
            get { return ports; }
            set
            {
                ports = value;
                NotifyPropertyChanged("Ports");
            }
        }

        private string selectedport;
        public string SelectedPort
        {
            get { return selectedport; }
            set
            {
                selectedport = value;
                NotifyPropertyChanged("SelectedPort");
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

        private bool foundports;
        public bool FoundPorts
        {
            get { return foundports; }
            set
            {
                foundports = value;
                if (!value)
                    Timer.Start();
                else
                    Timer.Stop();
                NotifyPropertyChanged("FoundPorts");
            }
        }

        private bool isconnected;
        public bool IsConnected
        {
            get { return isconnected; }
            set
            {
                isconnected = value;
                ButtonText = isconnected ? "Disconnect" : "Connect";
                ConnectionButtenColor = isconnected ? Colors.LightCoral : Colors.LightGreen;
                ConnectionInfo = isconnected ? "Connected" : "Disconnected";
                NotifyPropertyChanged("IsConnected");
            }
        }

        private Color connectionbuttencolor;
        public Color ConnectionButtenColor
        {
            get { return connectionbuttencolor; }
            set
            {
                connectionbuttencolor = value;
                NotifyPropertyChanged("ConnectionButtenColor");
            }
        }

        private string connectioninfo;
        public string ConnectionInfo
        {
            get { return connectioninfo; }
            set
            {
                connectioninfo = value;
                NotifyPropertyChanged("ConnectionInfo");
            }
        }

        private bool enablewindow;
        public bool EnableWindow
        {
            get { return enablewindow; }
            set
            {
                enablewindow = value;
                NotifyPropertyChanged("EnableWindow");
            }
        }

        #endregion

        public Connection()
        {
            EnableWindow = true;
            NodeNumber = 1;
            IsConnected = false;
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(5);
            Timer.Tick += Timer_Tick;
            comPort.DataReceived += port_DataReceived;
            comPort.Disposed += comPort_Disposed;
            comPort.ErrorReceived += comPort_ErrorReceived;
            comPort.RtsEnable = true;
            comPort.DtrEnable = true;
            comPort.Handshake = System.IO.Ports.Handshake.None;
            comPort.Parity = System.IO.Ports.Parity.None;
            comPort.StopBits = System.IO.Ports.StopBits.One;
            comPort.DataBits = 8;
            comPort.BaudRate = 19200;
            LookForAvilablePorts();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("LookForAvilablePorts");
            LookForAvilablePorts();
        }

        private void comPort_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            Console.WriteLine("comPort_ErrorReceived");
        }

        private void comPort_Disposed(object sender, EventArgs e)
        {
            Console.WriteLine("comPort_Disposed");
        }

        private void port_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            List<byte> data = new List<byte>();
            System.IO.Ports.SerialPort spL = (System.IO.Ports.SerialPort)sender;
            byte[] buf = new byte[spL.BytesToRead];
            spL.Read(buf, 0, buf.Length);
            data.AddRange(buf);
            if (data.Count > 2)
            {
                ReciveMessage temp = new ReciveMessage(data);
                App.Current.Dispatcher.Invoke(new Action(() =>
                {
                    Messages.Insert(0, temp);
                    if (temp.MessageType == MessageType.Rxa)
                    {
                        action.GetAnswerText(temp);
                    }
                    if (temp.MessageType == MessageType.Rx)
                    {
                        action.GetUSBAnswer(temp);
                    }
                }));
            }
            else
            {
                if (data.Count == 0)
                    Console.WriteLine("Empty Array");
                else
                    Console.WriteLine("To Small data ==> {}", string.Join("-", data.ToArray()));
            }
        }

        private void SendData(SentMessage message)
        {
            IsConnected = comPort.IsOpen;
            if (!IsConnected)
            {
                MessageBox.Show("Port not open");
                return;
            }
            if (message == null)
            {
                MessageBox.Show("Error Biudilnn message");
                return;
            }
            comPort.DiscardInBuffer();
            try
            {
                if (message.HaveData)
                    comPort.Write(message.FullData, 0, message.FullData.Length);
                else
                    comPort.Write(message.FullString);
                Messages.Insert(0, message);
                comPort.DiscardInBuffer();
            }
            catch
            {
                Console.WriteLine("comPort_Disposed");
                //comPort.Close();
                IsConnected = comPort.IsOpen;
                LookForAvilablePorts();
            }
        }

        private void LookForAvilablePorts()
        {
            Ports.Clear();
            foreach (var item in System.IO.Ports.SerialPort.GetPortNames())
            {
                Ports.Add(item);
            }
            FoundPorts = Ports.Count > 0;
            SelectedPort = Ports.Count > 0 ? Ports.First() : null;

        }

        #region Commands

        #region Connect
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
            if (SelectedPort != null)
            {
                if (!comPort.IsOpen)
                {
                    comPort.PortName = SelectedPort;
                    comPort.Open();
                    IsConnected = comPort.IsOpen;
                }
                else
                {
                    LookForAvilablePorts();
                    //SelectedPort = null;
                    comPort.Close();
                    IsConnected = comPort.IsOpen;
                }
            }
        }
        #endregion

        #region Exit
        private ICommand _exitCommand;
        public ICommand ExitCommand
        {
            get
            {
                return _exitCommand ?? (_exitCommand = new CommandHandler((string ms) => ExitAction(), true));
            }
        }
        public void ExitAction()
        {
            try
            {
                comPort.Close();
                comPort.Dispose();
            }
            catch
            {

            }
            App.Current.Shutdown();
        }
        #endregion

        #region Send
        private ICommand _sendCommand;
        public ICommand SendCommand
        {
            get
            {
                return _sendCommand ?? (_sendCommand = new CommandHandler((string ms) => SendAction(ms), true));
            }
        }

        public void SendAction(string message)
        {
            SentMessage temp;
            if (message.StartsWith(">"))
            {
                temp = SentMessage.CreateMessage(string.Format("{0}", message.Substring(1)));
                SendData(temp);
                return;
            }
            GetAction(message);
        }
        #endregion

        #endregion

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

        internal void SendData(string str)
        {
            str = string.Format(">{0}\r", str);
            IsConnected = comPort.IsOpen;
            if (!IsConnected)
            {
                MessageBox.Show("Port not open");
                return;
            }
            comPort.DiscardInBuffer();
            try
            {
                comPort.Write(str);
                System.Threading.Thread.Sleep(1000);
                //Messages.Insert(0, message);
                comPort.DiscardInBuffer();
            }
            catch
            {
                Console.WriteLine("comPort_Disposed");
                //comPort.Close();
                IsConnected = comPort.IsOpen;
                LookForAvilablePorts();
            }
        }

        private void GetAction(string text)
        {
            action = null;
            switch (text)
            {
                case "I":
                    action = new USBInfo(NodeNumber);
                    break;
                case "Red LED On":
                    action = new RedLed(NodeNumber, 1);
                    break;
                case "Red LED Off":
                    action = new RedLed(NodeNumber, 0);
                    break;
                case "Red LED Status":
                    action = new RedLed(NodeNumber, 2);
                    break;
                case "Green LED On":
                    action = new GreenLed(NodeNumber, 1);
                    break;
                case "Green LED Off":
                    action = new GreenLed(NodeNumber, 0);
                    break;
                case "Green LED Status":
                    action = new GreenLed(NodeNumber, 2);
                    break;
                case "Remove Bond":
                    action = new RemoveNode(NodeNumber);
                    break;
                case "Get Temp":
                    action = new Temperature(NodeNumber);
                    break;
                case "Get Network":
                    action = new Network(NodeNumber);
                    break;
                case "Discovery":
                    action = new Discovery();
                    break;
                case "Bond new Node":
                    action = new NewNode();
                    break;
                case "FullInfo":
                    action = new NodeFullInfo(NodeNumber);
                    break;
                case "Leds Flick":
                    action = new FlickerLeds(NodeNumber);
                    break;
                case "RS232":
                    action = new UART(NodeNumber);
                    break;
            }
            if (action != null)
            {
                action.CanSendNext += Action_CanSendNext;
                EnableWindow = false;
                action.Start();
            }
        }
        
        private void Action_CanSendNext(object sender, EventArgs e)
        {
            SentMessage mess;
            if (action.GetMessage(out mess) && mess != null)
                SendData(mess);
            else
                EnableWindow = true;
        }
    }

    public class CommandHandler : ICommand
    {
        private Action<string> _action;
        public bool _canExecute;
        public CommandHandler(Action<string> action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (parameter is string)
            {

                _action((string)parameter);
                return;
            }
            _action(null);
        }
    }
}
