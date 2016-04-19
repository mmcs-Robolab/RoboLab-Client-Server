using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboServer.lib
{
    public delegate void ConnectionEventHadler(object sender, ConnectionEventArgs args);

    public class ConnectionEventArgs : EventArgs
    {
        public string Message { get; set; }
        public ConnectionEventArgs(string message = "")
        {
            Message = message;
        }
    }

    
}
