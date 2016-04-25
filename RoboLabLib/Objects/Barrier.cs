using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboLab.MathLib;

namespace RoboLab.Objects
{
    class Barrier : SceneObject
    {
        public Barrier()
        {
            name = "";
            position = new Vector3();
            id = -1;
        }

        public Barrier(string name, Vector3 position, double width, double height, double depth)
        {
            this.name = name;
            this.id = id;
            this.position = position;
            this.width = width;
            this.height = height;
            this.depth = depth;
        }

    }
}
