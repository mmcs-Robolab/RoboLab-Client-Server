using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboLab.MathLib;

namespace RoboLab.Objects
{
    public class Barrier : SceneObject
    {
        public Rect boundRect;

        public Barrier()
        {
            name = "";
            position = new Vector3();
            id = -1;
            boundRect = new Rect();
        }

        public Barrier(string name, Vector3 position, double width, double height, double depth)
        {
            this.name = name;
            this.id = id;
            this.position = position;
            this.width = width;
            this.height = height;
            this.depth = depth;

            createRectBound();
        }

        public void createRectBound()
        {
            boundRect = new Rect(position,
                                 new Vector3(position.x - width / 2, position.y, position.z - depth / 2),
                                 new Vector3(position.x + width / 2, position.y, position.z + depth / 2));
        }

    }
}
