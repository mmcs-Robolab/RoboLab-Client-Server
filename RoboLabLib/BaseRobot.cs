using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RoboLab
{
    
    public abstract class BaseRobot : MarshalByRefObject
    {
        protected List<Motor> motors;
        protected List<Sensor> sensors;

        public BaseRobot()
        {
            motors = new List<Motor>();
            sensors = new List<Sensor>();
        }

        public IList<Motor> GetMotors()
        {
            return motors.AsReadOnly();
        }

        public IList<Sensor> GetSensors()
        {
            return sensors.AsReadOnly();
        }
    }

    public abstract class PollResult : MarshalByRefObject
    {}

    public delegate void PollDelegate(Pollable sender, PollResult e);

    public abstract class Pollable : MarshalByRefObject
    {
        public event PollDelegate Polled;

        private Timer pollTimer;
        
        public double PollInterval
        {
            get
            {
                return pollTimer.Interval;
            }
            set
            {
                pollTimer.Interval = value;
                if (value > 0)
                    pollTimer.Start();
                else
                    pollTimer.Stop();
            }
        }

        public abstract PollResult Poll();

        public Pollable()
        {
            pollTimer = new Timer(0);
            pollTimer.AutoReset = true;
            pollTimer.Elapsed += PollTimer_Elapsed;
        }

        private void PollTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Poll();
        }
    }
    [Flags]
    public enum LogicalMotorType { Nothing = 0, Forward = 1, Backward = 2, RightTurn = 4, LeftTurn = 8, Steer = 16 };

    public abstract class Motor : Pollable
    {
        public LogicalMotorType MotorType { get; protected set; } = LogicalMotorType.Nothing;

        public abstract void Run(double power);

        public abstract void Brake();
    }

    public class MotorPollResult : PollResult
    {
        public double TachoCount { get; private set; }

        public MotorPollResult(double tachoCount)
        {
            TachoCount = tachoCount;
        }
    }

    public enum SensorType { Laser }

    public abstract class Sensor : Pollable
    {
        public SensorType SensorType { get; protected set; }
    }
}
