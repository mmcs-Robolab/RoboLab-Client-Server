using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLab.MathLib
{
    class Rect
    {
        public Vector3 center;
        public Vector3 point1;
        public Vector3 point2;


        public Rect()
        {
            center = new Vector3();
            point1 = new Vector3();
            point2 = new Vector3();

        }

        public Rect(Vector3 center, Vector3 point1, Vector3 point2)
        {
            this.center = center;
            this.point1 = point1;
            this.point2 = point2;

        }

        public void rotate(double angle)
        {
            point1 = rotateCorner(point1, angle);
            point2 = rotateCorner(point2, angle);
        }

        private Vector3 rotateCorner(Vector3 corner, double angle)
        {

            // translate point to origin
            double tempX = corner.x - center.x;
            double tempZ = corner.z - center.z;

            // apply rotation
            double rotatedX = tempX * Math.Cos(angle) - tempZ * Math.Sin(angle);
            double rotatedZ = tempX * Math.Sin(angle) + tempZ * Math.Cos(angle);

            Vector3 res = new Vector3(rotatedX + center.x, corner.y, rotatedZ + center.z);

            return res;

        }

       
    }
}
