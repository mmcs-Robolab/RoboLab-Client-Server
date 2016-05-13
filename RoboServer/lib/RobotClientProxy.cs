using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboServer.lib
{
    class RobotClientProxy : RobotClient
    {

        public override event ReceiveMessagehandler ReceiveMessage;

        public override void BindUserRobot(int UserID, string Robot)
        {
            
        }

        public void UnbindUser(int UserID)
        {

        }

        public override void CommandRobot(int UserID, string Command)
        {
            
        }

        public override void GetRobots(int UserID)
        {
            
        }

        public override void SendSource(int UserID, string Source, string MainClass)
        {
            
        }
    }
}
