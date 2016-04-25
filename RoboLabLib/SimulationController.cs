using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboLab.MathLib;
using RoboLab.Objects;

using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace RoboLab
{
    class SimulationController
    {
        private static System.Timers.Timer aTimer;
        

        // Data for save
        struct PosPoint
        {
            public Vector3 point;
            public string moveType;
        }

        private List<PosPoint> pointList = new List<PosPoint>();
        
        // Scene
        private List<Barrier> barrierList;
        private Plane plane;
        private VirtualRobot robot;


        public SimulationController()
        {
            robot = new VirtualRobot();
        }

        public void setRobot(VirtualRobot robot)
        {
            this.robot = robot;
        }

        public void StartSimulation()
        {
            aTimer = new System.Timers.Timer(100);
            aTimer.Elapsed += gameLoop;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        public void StopSimulation()
        {
            aTimer.Enabled = false;
        }

        private void gameLoop(Object source, System.Timers.ElapsedEventArgs e)
        {
            updatePosition();
        }

        public void updatePosition()
        {
            robot.position.add(robot.velocity);
            saveCurPosition();
        }

        public void saveCurPosition()
        {
            string moveType;

            if (robot.velocity.isDifferentDirection(robot.direction))
            {
                moveType = "backward";
            }
            else
            {
                moveType = "forward";
            }
            
            PosPoint point = new PosPoint();
            point.moveType = moveType;
            point.point = robot.position;

            pointList.Add(point);


        }

        public void importSceneFromJson(String json)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<SceneObject> sceneObject = jsSerializer.Deserialize<List<SceneObject>>(json);

            foreach (var elt in sceneObject)
            {

                if (elt.name == "barrier")
                {
                    Barrier newBarrier = new Barrier();
                    newBarrier.position = elt.position;
                    newBarrier.name = elt.name;
                    newBarrier.id = elt.id;
                    newBarrier.updateSize(elt.width, elt.height, elt.depth);

                    barrierList.Add(newBarrier);

                }
                else if (elt.name == "plane")
                {
                    plane = new Plane();
                    plane.name = elt.name;
                    plane.position = elt.position;
                    plane.updateSize(elt.width, elt.height, elt.depth);
                }
                else if (elt.name == "robot")
                {                    
                    robot.updateSize(elt.width, elt.height, elt.depth);
                }
            }

        }

    }
}
