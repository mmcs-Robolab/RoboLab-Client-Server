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
        public ConnectionInfo Connection { get; set; }
        public ConnectionEventArgs(ConnectionInfo connection = null)
        {
            Connection = connection;
        }
    }

    public delegate void WebConnectionEventHadler(object sender, WebConnectionEventArgs args);

    public class WebConnectionEventArgs : EventArgs
    {
        public WebConnectionInfo Connection { get; set; }
        public WebConnectionEventArgs(WebConnectionInfo connection = null)
        {
            Connection = connection;
        }
    }
}
