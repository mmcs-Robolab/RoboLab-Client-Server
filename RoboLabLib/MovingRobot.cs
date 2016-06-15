using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLab
{
    public class MovingRobot : Robot
    {
        
        internal protected Dictionary<LogicalMotorType, List<IMotor>> motorsByTypes = new Dictionary<LogicalMotorType, List<IMotor>>();
        internal protected double turnRatio = 0;
        internal protected double power = 0;
        internal override void SetBaseRobot(BaseRobot baseRobot)
        {
            base.SetBaseRobot(baseRobot);
            if (baseRobot != null)
            {
                foreach (IMotor m in baseRobot.GetMotors())
                {
                    if (!motorsByTypes.ContainsKey(m.MotorType))
                        motorsByTypes[m.MotorType] = new List<IMotor>();
                    motorsByTypes[m.MotorType].Add(m);
                }
            }
            else
                motorsByTypes.Clear();
        }

        private void updateMotors()
        {
            if(motorsByTypes.ContainsKey(LogicalMotorType.Forward))
                foreach (IMotor m in motorsByTypes[LogicalMotorType.Forward])
                    if (m.MotorType.HasFlag(LogicalMotorType.Right) && turnRatio > 0)
                        m.Run(power * (1 - turnRatio * 2));
                    else if (m.MotorType.HasFlag(LogicalMotorType.Left) && turnRatio < 0)
                        m.Run(power * (-1 - turnRatio * 2));
                    else
                        m.Run(power);
            if (motorsByTypes.ContainsKey(LogicalMotorType.Backward))
                foreach (IMotor m in motorsByTypes[LogicalMotorType.Backward])
                    if (m.MotorType.HasFlag(LogicalMotorType.Left) && turnRatio > 0)
                        m.Run(-power * (1 - turnRatio * 2));
                    else if (m.MotorType.HasFlag(LogicalMotorType.Right) && turnRatio < 0)
                        m.Run(-power * (-1 - turnRatio * 2));
                    else
                        m.Run(-power);
            if (motorsByTypes.ContainsKey(LogicalMotorType.Right))
                foreach (IMotor m in motorsByTypes[LogicalMotorType.Right])
                    if (!m.MotorType.HasFlag(LogicalMotorType.Forward) && !m.MotorType.HasFlag(LogicalMotorType.Backward))
                        m.Run(turnRatio);
            if (motorsByTypes.ContainsKey(LogicalMotorType.Left))
                foreach (IMotor m in motorsByTypes[LogicalMotorType.Left])
                    if (!m.MotorType.HasFlag(LogicalMotorType.Forward) && !m.MotorType.HasFlag(LogicalMotorType.Backward))
                        m.Run(-turnRatio);
            if (motorsByTypes.ContainsKey(LogicalMotorType.Steer))
                foreach (IMotor m in motorsByTypes[LogicalMotorType.Steer])
                    if (!m.MotorType.HasFlag(LogicalMotorType.Forward) && !m.MotorType.HasFlag(LogicalMotorType.Backward))
                        m.Run(turnRatio);
        }

        public virtual void BeginMoveForward(double power, double turnRatio = 0)
        {
            this.power = power;
            this.turnRatio = turnRatio;
            updateMotors();
        }

        public virtual void BeginMoveBackward(double power, double turnRatio = 0)
        {
            BeginMoveForward(-power, turnRatio);
        }

        public virtual void BeginTurnRight(double power)
        {
            BeginMoveForward(power, 1);
        }

        public virtual void BeginTurnLeft(double power)
        {
            BeginMoveForward(power, -1);
        }
        
        public virtual void Stop()
        {
            this.power = 0;
            this.turnRatio = 0;
            updateMotors();
        }

        public MovingRobot()
        {

        }

    }
}
