using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLab
{
    public class Logger
    {
        static public List<String> LogRecord { private set; get; }
        static public event LogUpdateEventHandler LogUpdated;

        static public void Log(String s, Object sender = null)
        {
            if (LogRecord == null)
                LogRecord = new List<string>();
            String msg = DateTime.Now.ToString() + ": " + s;
            LogRecord.Add(msg);
            if (LogUpdated != null)
                LogUpdated(sender, new LogUpdateEventArgs(msg));
        }
    }

    public class LogUpdateEventArgs : EventArgs
    {
        public String Message { get; set; }
        public LogUpdateEventArgs(String msg = "")
        {
            Message = msg;
        }
    }

    public delegate void LogUpdateEventHandler(Object sender, LogUpdateEventArgs e);
}
