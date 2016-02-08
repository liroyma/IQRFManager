using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using WpfApplication2.Actions;
using WpfApplication2.User_Controls;

namespace WpfApplication2
{
    internal class UART : IQRFAction
    {
        string StringMessage;
        int SelectedNode;

        public UART(int selectednode)
        {
            SelectedNode = selectednode;
            StringMessage = string.Format("Node {0} RS232 Communication: ", SelectedNode);
            //ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:{0},00,01,00,FF,FF", (selectednode).ToString("x2"))));
        }

        public override void Start()
        {
            base.Start();
        }

        public override void AllDone()
        {
        }

        public override void SetView()
        {
            AddItemsToView(new Rs232View(this));
        }

        public string OpenRS232Communication(int bauds)
        {
            string message;
            string baudRate;
            switch (bauds)
            {
                case 1200:
                    baudRate = "00";
                    break;
                case 2400:
                    baudRate = "01";
                    break;
                case 4800:
                    baudRate = "02";
                    break;
                case 9600:
                    baudRate = "03";
                    break;
                case 19200:
                    baudRate = "04";
                    break;
                case 38400:
                    baudRate = "05";
                    break;
                case 57600:
                    baudRate = "06";
                    break;
                case 115200:
                    baudRate = "07";
                    break;
                default:
                    baudRate = "03";
                    break;
            }
            message = string.Format("DS:{0},00,0C,00,FF,FF,{1}", (SelectedNode).ToString("x2"), baudRate);
            ActionMessages.Add(SentMessage.CreateMessage(message));
            Start();
            return message;
        }

        public string CloseRS232Communication()
        {
            string message = string.Format("DS:{0},00,0C,01,FF,FF", (SelectedNode).ToString("x2"));
            ActionMessages.Add(SentMessage.CreateMessage(message));
            Start();
            return message;
        }

        public string SendRS232Command(string command)
        {
            string message = string.Format("DS:{0},00,0C,02,FF,FF,FF,{1}", (SelectedNode).ToString("x2"), command);
            ActionMessages.Add(SentMessage.CreateMessage(message));
            Start();
            return command;
        }
    }
}