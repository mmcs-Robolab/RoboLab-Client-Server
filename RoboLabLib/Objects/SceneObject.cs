using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboLab.MathLib;

namespace RoboLab.Objects
{
    public class SceneObject
    {
        public string name;
        public int id;
        public Vector3 position;
        public double width;
        public double height;
        public double depth;

        public SceneObject()
        {
            name = "";
            id = -1;
            position = new Vector3();
            width = 0;
            height = 0;
            depth = 0;
        }

        public void updateCoordinates(double x, double y, double z)
        {
            position.x = x;
            position.y = y;
            position.z = z;
        }

        public void updateCoordinates(Vector3 newCoord)
        {
            position = newCoord;
        }


        public void updateSize(double width, double height, double depth)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
        }

    }
}
