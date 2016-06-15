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
            switch(args.Message)
            {
                case "Forward":
                    Stop();
                    BeginMoveForward(1);
                    break;
                case "Backward":
                    Stop();
                    BeginMoveBackward(1);
                    break;
                case "Left":
                    Stop();
                    BeginTurnLeft(1);
                    break;
                case "Right":
                    Stop();
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
