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
        public Vector3 velocity;
        public Vector3 position;
        public Vector3 direction;
        public Vector3 up;

        public double width;
        public double height;
        public double depth;

        public Rect boundRect;

        private double speedCoef = 0.1;


        public VirtualRobot()
        {
            velocity = new Vector3(0, 0, 0);
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

        public void updateSize(double width, double height, double depth)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
        }
        

        public override void BeginGetSensorValue(SensorType type)
        {
            throw new NotImplementedException();
        }

        public override double[] GetSensorValue(SensorType type)
        {
            throw new NotImplementedException();
        }
    }
}
