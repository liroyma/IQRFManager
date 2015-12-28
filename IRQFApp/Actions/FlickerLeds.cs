using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication2.Actions
{
    class FlickerLeds : IQRFAction
    {
        public FlickerLeds(int selectednode)
        {
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:{0},00,20,00,FF,FF", (selectednode).ToString("x2"))));
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:{0},00,20,01,FF,FF", (selectednode).ToString("x2"))));
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:{0},00,20,00,FF,FF", (selectednode).ToString("x2"))));
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:{0},00,20,01,FF,FF", (selectednode).ToString("x2"))));
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:{0},00,20,00,FF,FF", (selectednode).ToString("x2"))));
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:{0},00,20,01,FF,FF", (selectednode).ToString("x2"))));
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:{0},00,20,00,FF,FF", (selectednode).ToString("x2"))));
            ActionMessages.Add(SentMessage.CreateMessage(string.Format("DS:{0},00,20,01,FF,FF", (selectednode).ToString("x2"))));
        }
        public override void AllDone()
        {
        }

        public override void SetView()
        {
        }
    }
}
