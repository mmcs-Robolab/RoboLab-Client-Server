using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLab
{
    public sealed class ControllableMovingRobot : MovingRobot
    {
        private bool finished = false;
        public ControllableMovingRobot()
        {
            this.ReceiveMessage += ControllableMovingRobot_ReceiveMessage;
        }

        private void ControllableMovingRobot_ReceiveMessage(object sender, ReceiveEventArgs args)
        {
            //Print(args.Message);
            switch(args.Message)
            {
                case "Forward":
                    BeginMoveForward(1);
                    break;
                case "Backward":
                    BeginMoveBackward(1);
                    break;
                case "Left":
                    BeginTurnLeft(1);
                    break;
                case "Right":
                    BeginTurnRight(1);
                    break;
                case "Stop":
                    Stop();
                    break;
                case "Finish":
                    finished = true;
                    break;
            }
        }

        public override void Run()
        {
            finished = true;
            while(!finished)
            { }
        }
    }
}
