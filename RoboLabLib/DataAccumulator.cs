using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLab
{
    
    public class DataAccumulator
    {

        private string accum = "";
        private int numLeft = 0;

        public event MessageReceivedEventHandler DataReceived;

        public DataAccumulator()
        {

        }

        private void checkForMessages()
        {
            if (numLeft == 0)
            {
                int ind = accum.IndexOf('#');
                if (ind != -1 && int.TryParse(accum.Substring(0, ind), out numLeft))
                    accum = accum.Substring(ind + 1);
            }
            if (numLeft > 0 && accum.Length >= numLeft)
            {
                string message = accum.Substring(0, numLeft);
                accum = accum.Substring(numLeft);
                numLeft = 0;
                if (DataReceived != null)
                    DataReceived(this, new MessageReceivedEventArgs(message));
                checkForMessages();
            }
        }

        public void AcceptData(string data)
        {
            accum = accum + data;
            checkForMessages();
        }
    }
    public class MessageReceivedEventArgs : EventArgs
    {
        public String Message { private set; get; }
        public MessageReceivedEventArgs(String msg = "")
        {
            Message = msg;
        }
    }
    public delegate void MessageReceivedEventHandler(Object sender, MessageReceivedEventArgs e);

}
