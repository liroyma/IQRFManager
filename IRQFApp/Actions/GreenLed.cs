using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApplication2.Actions
{
    class GreenLed : IQRFAction
    {
        int SelectedNode;
        string StringMessage;

        public GreenLed(int selectednode, int state)
        {
            string pcmd = "00";
            SelectedNode = selectednode;
            switch (state)
            {
                case 0:
                    pcmd = "00";
                    StringMessage = string.Format("turnning Node {0} Green Lamp Off.", SelectedNode);
                    break;
                case 1:
                    pcmd = "01";
                    StringMessage = string.Format("turnning Node {0} Green Lamp On.", SelectedNode);
                    break;
                case 2:
                    pcmd = "02";
                    StringMessage = string.Format("Node {0} Green Lamp State is.", SelectedNode);
                    break;
            }
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:{0},00,07,{1},FF,FF", (selectednode).ToString("x2"), pcmd)));
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
        }


    }
}
