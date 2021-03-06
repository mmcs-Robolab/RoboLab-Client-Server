﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboServer.lib
{
    public interface IRobotClient
    {
        SortedSet<int> Users { get; set; }
        

        void BindUserRobot(int UserID, string Robot);

        void UnbindUser(int UserID);

        void SendSource(int UserID, string Source, string MainClass);

        void CommandRobot(int UserID, string Command);

        void GetRobots(int UserID);

        void ManualControl(int UserID);

        event ReceiveMessagehandler ReceiveMessage;
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
