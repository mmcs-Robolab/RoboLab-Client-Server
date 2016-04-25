using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboLab.MathLib;

namespace RoboLab
{
    class VirtualRobot : BaseRobot
    {
        public Vector3 velocity;
        public Vector3 position;
        public Vector3 direction;
        public Vector3 up;

        public double width;
        public double height;
        public double depth;

        private double speedCoef = 0.1;


        public VirtualRobot()
        {
            velocity = new Vector3(0, 0, 0);
            position = new Vector3(0, 0, 0);
            direction = new Vector3(0, 0, -1);
            up = new Vector3(0, 1, 0);
        }

        public override void MoveForward(double paramVelocity)
        {
            this.velocity = direction;
            this.velocity.multiplyScalar(paramVelocity * speedCoef);
        }

        public override void MoveBackward(double paramVelocity)
        {
            this.velocity = direction;
            this.velocity.multiplyScalar(-paramVelocity * speedCoef);
        }

        public override void StartMoveBackward(double distance)
        {
            throw new NotImplementedException();
        }

        public override void StartMoveForward(double distance)
        {
            throw new NotImplementedException();
        }

        public override void TurnLeft(double angle)
        {
            direction.rotateOnY(angle);
        }

        public override void TurnRight(double angle)
        {
            direction.rotateOnY(-angle);
        }

        public override void StartTurnLeft(double angle)
        {
            throw new NotImplementedException();
        }

        public override void StartTurnRight(double angle)
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            velocity = new Vector3();
        }

        public void updateSize(double width, double height, double depth)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
        }
    }
}
