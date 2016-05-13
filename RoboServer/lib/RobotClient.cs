using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboServer.lib
{
    abstract class RobotClient
    {
        public SortedSet<int> Users;

        public RobotClient()
        {
            Users = new SortedSet<int>();
        }

        public abstract void BindUserRobot(int UserID, string Robot);

        public abstract void SendSource(int UserID, string Source, string MainClass);

        public abstract void CommandRobot(int UserID, string Command);

        public abstract void GetRobots(int UserID);

        public abstract event ReceiveMessagehandler ReceiveMessage;
    }

    public class MessageEventArgs : EventArgs
    {
        public string Message { get; set; }
        public int UserID { get; set; }
        public MessageEventArgs(int userID, string message = "")
        {
            UserID = userID;
            Message = message;
        }
    }

    public delegate void ReceiveMessagehandler(Object sender, MessageEventArgs args);
}
