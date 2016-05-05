using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboServer.lib
{
    class RobotClientProxy : IRobotClient
    {

        public event ReceiveMessagehandler ReceiveMessage;

        public void BindUserRobot(int UserID, string Robot)
        {
            
        }

        public void UnbindUser(int UserID)
        {

        }

        public void CommandRobot(int UserID, string Command)
        {
            
        }

        public void GetRobots()
        {
            
        }

        public void SendSource(int UserID, string Source, string MainClass)
        {
            
        }
    }
}
