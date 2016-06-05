using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboLab.MathLib;

namespace RoboLab
{
    public class VirtualRobot : BaseRobot
    {
        public double velocity;
        public Vector3 position;
        public double lateralVelocity;
        public Vector3 direction;
        public Vector3 up;

        public double width;
        public double height;
        public double depth;

        public Rect boundRect;

        private double speedCoef = 0.1;
        
        public VirtualRobot()
        {
            velocity = 0;
            position = new Vector3(0, 0, 0);
            direction = new Vector3(0, 0, -1);
            up = new Vector3(0, 1, 0);

            width = 0;
            height = 0;
            depth = 0;

            boundRect = new Rect();
        }

        public void createRectBound()
        {
            boundRect = new Rect(position, 
                                 new Vector3(position.x - width/2, position.y, position.z - depth/2),
                                 new Vector3(position.x + width/2, position.y, position.z + depth/2));
        }
        /*
        public override void BeginMoveForward(double paramVelocity)
        {
            this.velocity = direction;
            this.velocity.multiplyScalar(paramVelocity * speedCoef);
        }

        public override void BeginMoveBackward(double paramVelocity)
        {
            this.velocity = direction;
            this.velocity.multiplyScalar(-paramVelocity * speedCoef);
        }

        

        public override void BeginTurnLeft(double angle)
        {
            direction.rotateOnY(angle);
            boundRect.rotate(angle);
        }

        public override void BeginTurnRight(double angle)
        {
            direction.rotateOnY(-angle);
            boundRect.rotate(angle);
        }
        
        public override void Stop()
        {
            velocity = new Vector3();
        }
        */
        public void updateSize(double width, double height, double depth)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
        }
        
    }

    public class VirtualForwardMotor : Motor
    {
        VirtualRobot robot;

        public VirtualForwardMotor(VirtualRobot robot)
        {
            this.robot = robot;
            this.MotorType = LogicalMotorType.Forward;
        }

        public override void Brake()
        {
            robot.velocity = 0;
        }

        public override PollResult Poll()
        {
            return new MotorPollResult(0);
        }

        public override void Run(double power)
        {
            robot.velocity = power;
        }
    }

    public class VirtualLateralMotor : Motor
    {
        VirtualRobot robot;

        public VirtualLateralMotor(VirtualRobot robot)
        {
            this.robot = robot;
        }

        public override void Brake()
        {
            robot.lateralVelocity = 0;
        }

        public override PollResult Poll()
        {
            return new MotorPollResult(0);
        }

        public override void Run(double power)
        {
            robot.lateralVelocity = power;
        }
    }
}
