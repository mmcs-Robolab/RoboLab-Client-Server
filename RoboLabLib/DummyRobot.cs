using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLab
{
    public class DummyRobot : BaseRobot
    {
        public override void BeginGetSensorValue(SensorType type)
        {
            onSensorUpdate(new SensorUpdateEventArgs(SensorType.Odometry, new double[] { }));
        }

        public override void BeginMoveBackward(double speed)
        {
            
        }

        public override void BeginMoveForward(double speed)
        {
           
        }

        public override void BeginTurnLeft(double speed)
        {
            
        }

        public override void BeginTurnRight(double speed)
        {
            
        }

        public override double[] GetSensorValue(SensorType type)
        {
            return new double[] { };
        }

        public override void Stop()
        {
           
        }
    }
}
