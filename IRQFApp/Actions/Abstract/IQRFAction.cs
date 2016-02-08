using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WpfApplication2.User_Controls;

namespace WpfApplication2
{
    public abstract class IQRFAction
    {
        static StackPanel ContainerPanel;
        public List<SentMessage> ActionMessages;
        public int CurrentMessageIndex = 0;

        public bool DontWaitForMore { get; set; }
        public bool DontADDElement { get; set; }

        private static LedsView ledsView;
        private static Rs232View rs232View;

        DispatcherTimer Timer = new DispatcherTimer();

        public event EventHandler CanSendNext;

        public static void SetPanel(StackPanel panel)
        {
            ContainerPanel = panel;
        }

        public IQRFAction()
        {
            ContainerPanel.Children.Clear();
            ActionMessages = new List<SentMessage>();
            Timer.Interval = TimeSpan.FromSeconds(5);
            Timer.Tick += Timer_Tick;
        }

        #region Answers
        public void GetAnswerText(ReciveMessage message)
        {
            if (message.PCMD < 128)
            {
                GotConfirmation(message);
                return;
            }
            Timer.Stop();
            UIElement usercontrol = null;
            DontADDElement = false;
            List<byte> list = new List<byte>();
            list.AddRange(message.Data);
            list.RemoveRange(0, 8);
            byte pcmd = (byte)(message.PCMD - 128);
            switch (message.PNUM)
            {
                case 00:
                    usercontrol = HandleCoordinatore(list, pcmd);
                    break;
                case 01:
                    usercontrol = HandleNode(list, pcmd);
                    break;
                case 02:
                    usercontrol = HandleOS(list, pcmd);
                    break;
                case 03:
                    usercontrol = HandleEEPROM(list, pcmd);
                    break;
                case 04:
                    usercontrol = HandleEEEPROM(list, pcmd);
                    break;
                case 05:
                    usercontrol = HandleRAM(list, pcmd);
                    break;
                case 06:
                    usercontrol = HandleLEDR(list, pcmd, message.PNUM);
                    break;
                case 07:
                    usercontrol = HandleLEDG(list, pcmd, message.PNUM);
                    break;
                case 08:
                    usercontrol = HandleSPI(list, pcmd);
                    break;
                case 09:
                    usercontrol = HandleIO(list, pcmd);
                    break;
                case 10:
                    usercontrol = HandleThermometer(list, pcmd);
                    break;
                case 11:
                    usercontrol = HandlePWM(list, pcmd);
                    break;
                case 12:
                    usercontrol = HandleUART(list, pcmd, this);
                    break;
                case 13:
                    usercontrol = HandleFRC(list, pcmd);
                    break;
                case 32:
                    usercontrol = HandleUSER(list, pcmd, message.PNUM);
                    break;
            }
            if (usercontrol == null)
            {
                if (!DontADDElement)
                {
                    TextBlock text = new TextBlock();
                    text.Text = string.Format("Not Handled -- PNUM: {1} ,PCMD: {0} .", message.PCMD - 128, (message.PNUM));
                    text.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    text.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    ContainerPanel.Children.Add(text);
                }
            }
            else
            {
                bool exists = false;
                foreach (var item in ContainerPanel.Children)
                {
                    if (item is LedsView || item is Rs232View)
                    {
                        exists = true;
                    }
                }

                if (!exists)
                {
                    ContainerPanel.Children.Add(usercontrol);
                }
            }
            SendNext();

        }

        public virtual void GetUSBAnswer(ReciveMessage temp)
        {
            if (DontWaitForMore)
            {
                Timer.Stop();
                SendNext();
            }

        }
        #endregion

        #region Timer
        public void SetTimeInterval(int seconds)
        {
            Timer.Interval = TimeSpan.FromSeconds(seconds);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Timer.Stop();
            SendNext();
        }

        #endregion

        public virtual void Start()
        {
            SetView();
            CanSendNext(null, null);
            Timer.Start();
        }

        public bool GetMessage(out SentMessage message)
        {
            message = null;
            if (CurrentMessageIndex >= ActionMessages.Count)
            {
                AllDone();
                return false;
            }
            Timer.Start();
            message = ActionMessages[CurrentMessageIndex++];
            return true;
        }

        private void SendNext()
        {
            System.Threading.Thread.Sleep(300);
            CanSendNext(null, null);
        }

        public abstract void SetView();

        public virtual void GotConfirmation(ReciveMessage message)
        {
            if (message.Data[0] == 255)
            {
                Timer.Stop();
                SendNext();
            }
        }

        public abstract void AllDone();

        public void AddItemsToView(UIElement element)
        {
            ContainerPanel.Children.Add(element);
        }

        

        #region 00 - HandleCoordinatore
        private UIElement HandleCoordinatore(List<byte> data, byte pcmd)
        {
            switch (pcmd)
            {
                case 0:
                    return CreateNumberOfNodesView(data);
                case 1:
                    return CreateDiscoversNodesView(data);
                case 2:
                    return CreateNumberOfBondedsView(data);
                case 3:
                    return ClearAllBondesView(data);
                case 4:
                    return CreateNodeBondView(data);
                case 5:
                    return CreateRemoveNodeView(data);
                case 7:
                    return CreateDiscovertBondedsView(data);
                default:
                    return null;
            }
        }

        public virtual UIElement CreateNumberOfNodesView(List<byte> data)
        {
            return null;
        }
        public virtual UIElement CreateDiscoversNodesView(List<byte> data)
        {
            return null;
        }
        public virtual UIElement CreateNumberOfBondedsView(List<byte> data)
        {
            return null;
        }
        public virtual UIElement ClearAllBondesView(List<byte> data)
        {
            return null;
        }
        public virtual UIElement CreateNodeBondView(List<byte> data)
        {
            return null;
        }
        public virtual UIElement CreateRemoveNodeView(List<byte> data)
        {
            return null;
        }
        public virtual UIElement CreateDiscovertBondedsView(List<byte> data)
        {
            return null;
        }

        #endregion

        #region 01 - Handle Node
        private UIElement HandleNode(List<byte> data, byte pcmd)
        {
            switch (pcmd)
            {
                case 0:
                    return CreateNTWView(data);
                case 1:
                    break;
                case 2:
                    break;
            }
            return null;
        }

        public virtual UIElement CreateNTWView(List<byte> data)
        {
            return new NetworkInfo(data);
        }
        #endregion

        #region 02 - Handle OS
        private UIElement HandleOS(List<byte> data, byte pcmd)
        {
            switch (pcmd)
            {
                case 0:
                    return CreateTXInfo(data);
                case 1:
                    break;
                case 2:
                    return CreateTHWPConf(data);
            }
            return null;
        }

        public virtual UIElement CreateTXInfo(List<byte> data)
        {
            return new TXInfo(data);
        }

        public virtual UIElement CreateTHWPConf(List<byte> data)
        {
            return new HWPConf(data);
        }
        #endregion

        #region Handle EEPROM
        private UIElement HandleEEPROM(List<byte> data, byte pcmd)
        {
            return null;
        }
        #endregion

        #region Handle EEEPROM
        private UIElement HandleEEEPROM(List<byte> data, byte pcmd)
        {
            return null;
        }
        #endregion

        #region Handle USER
        private UIElement HandleUSER(List<byte> data, byte pcmd, byte pnum)
        {
            if (ledsView == null) {
                ledsView = new LedsView();
            }
            ledsView.updateView(data, pcmd, pnum);
            return ledsView;
        }
        #endregion

        #region Handle FRC
        private UserControl HandleFRC(List<byte> data, byte pcmd)
        {
            return null;
        }
        #endregion

        #region Handle UART
        private UserControl HandleUART(List<byte> data, byte pcmd, IQRFAction uart)
        {
            if (rs232View == null)
            {
                rs232View = new Rs232View(uart);
            }
            return rs232View;
        }
        #endregion

        #region PWM
        private UserControl HandlePWM(List<byte> data, byte pcmd)
        {
            return null;
        }
        #endregion

        #region Handle Thermometer
        private UserControl HandleThermometer(List<byte> data, byte pcmd)
        {
            return new TemperatureInfo(data);
        }
        #endregion

        #region Handle IO
        private UserControl HandleIO(List<byte> data, byte pcmd)
        {
            return null;
        }
        #endregion

        #region Handle SPI
        private UserControl HandleSPI(List<byte> data, byte pcmd)
        {
            return null;
        }
        #endregion

        #region Handle LEDG
        private UserControl HandleLEDG(List<byte> data, byte pcmd, byte pnum)
        {
            if (ledsView == null)
            {
                ledsView = new LedsView();
            }
            ledsView.updateView(data, pcmd, pnum);
            return ledsView;
        }
        #endregion

        #region Handle LEDR
        private UserControl HandleLEDR(List<byte> data, byte pcmd, byte pnum)
        {
            if (ledsView == null)
            {
                ledsView = new LedsView();
            }
            ledsView.updateView(data, pcmd, pnum);
            return ledsView;
        }
        #endregion

        #region Handle RAM
        private UserControl HandleRAM(List<byte> data, byte pcmd)
        {
            return null;
        }
        #endregion

    }
}
