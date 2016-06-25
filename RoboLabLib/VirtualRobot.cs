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
            motors.Add(new VirtualForwardMotor(this));
            motors.Add(new VirtualLateralMotor(this));
            
        }

        public void createRectBound()
        {
            boundRect = new Rect(position, 
                                 new Vector3(position.x - width/2, position.y, position.z - depth/2),
                                 new Vector3(position.x + width/2, position.y, position.z + depth/2));
        }
        public void updateSize(double width, double height, double depth)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
        }
        
    }
    public abstract class VirtualMotor : BasePollable, IMotor
    {
        protected VirtualRobot robot;
        protected double tacho = 0;
        public VirtualMotor(VirtualRobot robot)
        {
            MotorType = LogicalMotorType.Nothing;
            this.robot = robot;
        }

        public LogicalMotorType MotorType { get; protected set; }

        public abstract void Brake();

        public override PollResult Poll()
        {
            MotorPollResult res = new MotorPollResult(tacho);
            tacho = 0;
            onPolled(res);
            return res;
        }

        public abstract void Run(double power);

        internal abstract void SimulationTick();

    }
    public class VirtualForwardMotor : VirtualMotor
    {
        public VirtualForwardMotor(VirtualRobot robot):base(robot)
        {
            this.MotorType = LogicalMotorType.Forward;
        }

        public override void Brake()
        {
            robot.velocity = 0;
        }
        
        public override void Run(double power)
        {
            robot.velocity = power;
        }

        internal override void SimulationTick()
        {
            tacho += Math.Abs(robot.velocity);
        }
    }

    public class VirtualLateralMotor : VirtualMotor
    {
        public VirtualLateralMotor(VirtualRobot robot) : base(robot)
        {
            this.MotorType = LogicalMotorType.Steer;
        }

        public override void Brake()
        {
            robot.lateralVelocity = 0;
        }

        public override PollResult Poll()
        {
            MotorPollResult res = new MotorPollResult(tacho);
            tacho = 0;
            onPolled(res);
            return res;
        }

        public override void Run(double power)
        {
            robot.lateralVelocity = -power;
        }

        internal override void SimulationTick()
        {
            tacho += Math.Abs(robot.lateralVelocity);
        }
    }
}
