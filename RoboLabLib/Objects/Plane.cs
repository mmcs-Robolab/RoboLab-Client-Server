using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboLab.MathLib;

namespace RoboLab.Objects
{
    public class Plane : SceneObject
    {
        public Plane()
        {
            name = "";
            position = new Vector3();
        }

        public Plane(string name, Vector3 position, double width, double height)
        {
            this.name = name;
            this.position = position;
            this.width = width;
            this.height = height;
        }
    }
}
