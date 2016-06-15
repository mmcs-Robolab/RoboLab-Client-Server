using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NKH.MindSqualls;

namespace RoboLab
{
    
    public class LegoRobot : BaseRobot
    {
        public enum MotorPort { PortA, PortB, PortC }
        internal NxtBrick brick;
        public LegoRobot(NxtCommLinkType commLinkType, byte serialPortNo)
        {
            brick = new NxtBrick(commLinkType, serialPortNo);
        }
        public LegoRobot(string portName)
        {
            brick = new NxtBrick(portName);
        }
        public bool IsConnected()
        {
            return brick.IsConnected;
        }

        public bool Connect()
        {
            brick.Connect();
            return brick.IsConnected;
        }

        public void AddMotor(MotorPort port, LogicalMotorType motorTypes)
        {
            NxtMotor motor = new NxtMotor();
            SetMotorToPort(port, motor);
            LegoMotor legoMotor = new LegoMotor(motor, motorTypes);
            this.motors.Add(legoMotor);
        }
        private void SetMotorToPort(MotorPort port, NxtMotor motor)
        {
            switch (port)
            {
                case MotorPort.PortA:
                    brick.MotorA = motor;
                    break;
                case MotorPort.PortB:
                    brick.MotorB = motor;
                    break;
                case MotorPort.PortC:
                    brick.MotorC = motor;
                    break;
            }
        }
        public void AddMotorPair(MotorPort portX, MotorPort portY, LogicalMotorType motorTypes = LogicalMotorType.Forward, LogicalMotorType steerMotorTypes = LogicalMotorType.Steer)
        {
            MotorSync msync = new MotorSync(new NxtMotor(), new NxtMotor());
            SetMotorToPort(portX, msync.MotorX);
            SetMotorToPort(portY, msync.MotorY);
            LegoMotorPairForward motorForward = new LegoMotorPairForward(msync, motorTypes);
            LegoMotorPairSteer motorSteer = new LegoMotorPairSteer(msync, steerMotorTypes);
            motors.Add(motorForward);
            motors.Add(motorSteer);
        }
    }
    public class MotorSync
    {
        NxtMotorSync motorSync;
        public double TurnRatio
        {
            get; private set;
        }
        public double Power
        {
            get; private set;
        }
        public NxtMotor MotorX
        {
            get; private set;
        }
        public NxtMotor MotorY
        {
            get; private set;
        }

        public MotorSync(NxtMotor motorX, NxtMotor motorY)
        {
            TurnRatio = 0;
            Power = 0;
            MotorX = motorX;
            MotorY = motorY;
            motorSync = new NxtMotorSync(motorX, motorY);
        }

        public void Run(double power, double turnRatio)
        {
            Power = power;
            TurnRatio = turnRatio;
            motorSync.Run((sbyte)(Power * 127), 0, (sbyte)(TurnRatio * 127));
        }

        public void Brake()
        {
            Power = 0;
            motorSync.Brake();
        }
    }
    public class LegoMotorPairForward : BasePollable, IMotor
    {
        MotorSync motorSync;
        public LegoMotorPairForward(MotorSync motorSync, LogicalMotorType type)
        {
            this.motorSync = motorSync;
            MotorType = type;
        }

        public LogicalMotorType MotorType { get; private set; }

        public void Brake()
        {
            motorSync.Brake();
        }

        public override PollResult Poll()
        {
            motorSync.MotorX.Poll();
            motorSync.MotorY.Poll();
            double tacho = (motorSync.MotorY.TachoCount ?? 0 + motorSync.MotorX.TachoCount ?? 0) * 0.5;
            motorSync.MotorY.ResetMotorPosition(false);
            motorSync.MotorX.ResetMotorPosition(false);
            MotorPollResult res = new MotorPollResult(tacho);
            onPolled(res);
            return res;
        }

        public void Run(double power)
        {
            motorSync.Run(power, motorSync.TurnRatio);
        }
    }
    public class LegoMotorPairSteer : IMotor
    {
        MotorSync motorSync;
        public LegoMotorPairSteer(MotorSync motorSync, LogicalMotorType type)
        {
            this.motorSync = motorSync;
            MotorType = type;
        }

        public LogicalMotorType MotorType { get; private set; }

        public void Brake()
        {
            Run(0);
        }
        
        public void Run(double power)
        {
            motorSync.Run(motorSync.Power, power);
            if (motorSync.Power == 0)
                motorSync.Brake();
        }
    }
    public class LegoMotor : IMotor, IPollable
    {
        NxtMotor motor;
        public LogicalMotorType MotorType { get; private set; }
        PollResult lastResult;
        public double PollInterval {
            get
            {
                return motor.PollInterval;
            }

            set
            {
                motor.PollInterval = (int)value;
            }
        }

        public event PollDelegate Polled;

        public void Brake()
        {
            motor.Brake();
        }

        public PollResult Poll()
        {
            motor.Poll();
            return lastResult;
        }

        public void Run(double power)
        {
            motor.Run((sbyte)(power * 127), 0);
        }

        public LegoMotor(NxtMotor motor, LogicalMotorType type)
        {
            this.motor = motor;
            motor.OnPolled += Motor_OnPolled;
            this.MotorType = type;
        }

        private void Motor_OnPolled(NxtPollable polledItem)
        {
            lastResult = new MotorPollResult(motor.TachoCount ?? 0);
            motor.ResetMotorPosition(false);
            if (Polled != null)
                Polled(this, lastResult);
        }
    }
}
