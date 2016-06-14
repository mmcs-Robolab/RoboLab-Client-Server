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
        protected List<IMotor> motors;
        protected List<ISensor> sensors;

        public BaseRobot()
        {
            motors = new List<IMotor>();
            sensors = new List<ISensor>();
        }

        public IList<IMotor> GetMotors()
        {
            return motors.AsReadOnly();
        }

        public IList<ISensor> GetSensors()
        {
            return sensors.AsReadOnly();
        }
    }

    public abstract class PollResult : MarshalByRefObject
    {}

    public class EmptyPollResult : PollResult
    { }

    public delegate void PollDelegate(IPollable sender, PollResult e);

    public interface IPollable
    {
        event PollDelegate Polled;
        
        double PollInterval {
            get; set;
        } 

        PollResult Poll();
    }

    public abstract class BasePollable : MarshalByRefObject, IPollable
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

        public BasePollable()
        {
            pollTimer = new Timer(0);
            pollTimer.AutoReset = true;
            pollTimer.Elapsed += PollTimer_Elapsed;
        }

        protected void onPolled(PollResult pr)
        {
            if (Polled != null)
                Polled(this, pr);
        }

        private void PollTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Poll();
        }
    }
    [Flags]
    public enum LogicalMotorType { Nothing = 0, Forward = 1, Backward = 2, Right = 4, Left = 8, Steer = 16 };

    public interface IMotor
    {
        LogicalMotorType MotorType { get; }
        
        void Run(double power);

        void Brake();
    }

    public class MotorPollResult : PollResult
    {
        public double TachoCount { get; private set; }

        public MotorPollResult(double tachoCount)
        {
            TachoCount = tachoCount;
        }
    }

    public enum SensorType { Laser, Touch }

    public interface ISensor : IPollable
    {
        SensorType SensorType { get; }
    }
}
