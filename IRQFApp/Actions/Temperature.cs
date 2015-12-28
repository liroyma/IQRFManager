using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApplication2.Actions
{
    class Temperature : IQRFAction
    {
        int SelectedNode;
        string StringMessage;

        public Temperature(int selectednode)
        {
            SelectedNode = selectednode;
            StringMessage = string.Format("Node {0} Temperature is: ", SelectedNode);
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:{0},00,0A,00,FF,FF", (selectednode).ToString("x2"))));
        }

        public override void AllDone()
        {
        }
        

        public override void SetView()
        {
        }
    }
}
