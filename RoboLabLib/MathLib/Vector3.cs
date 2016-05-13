using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLab.MathLib
{
    public class Vector3
    {
        public double x;
        public double y;
        public double z;

        public Vector3()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void multiplyScalar(double mult)
        {
            this.x *= mult;
            this.y *= mult;
            this.z *= mult;
        }

        public void rotateOnY(double angle)
        {
            this.x = this.x * Math.Cos(angle) - this.z * Math.Sin(angle);
            this.z = this.x * Math.Sin(angle) + this.z * Math.Cos(angle);
        }

        public void add(Vector3 vec)
        {
            this.x += vec.x;
            this.y += vec.y;
            this.z += vec.z;
        }

        public bool isNull()
        {
            return x == 0 && y == 0 && z == 0;
        }

        public bool isReverseDirection(Vector3 vec)
        {
            return x == -vec.x && y == -vec.y && z == -vec.z;
        }

        public bool isDifferentDirection(Vector3 vec)
        {
            return x != vec.x || y != vec.y || z != vec.z;
        }

    }
}
