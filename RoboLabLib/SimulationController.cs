﻿using System;
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
    public class SimulationController
    {
        private static System.Timers.Timer aTimer;
        

        // Data for save
        public struct PosPoint
        {
            public Vector3 point;
            public string moveType;
            public double angle;
        }

        private List<PosPoint> pointList = new List<PosPoint>();

        private Vector3 oldDirection = new Vector3();
        private Vector3 lastPos = new Vector3();
        // Scene
        private List<Barrier> barrierList;
        private Plane plane;
        private VirtualRobot robot;



        public SimulationController()
        {
            robot = new VirtualRobot();
            barrierList = new List<Barrier>();
        }

        public void setRobot(VirtualRobot robot)
        {
            this.robot = robot;
        }

        public VirtualRobot getRobot()
        {
            return robot;
        }

        public void StartSimulation()
        {
            oldDirection = robot.direction.Clone();
            lastPos = robot.position.Clone();
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
            collisionTest();
        }

        public void updatePosition()
        {
            foreach(IMotor m in robot.GetMotors())
            {
                VirtualMotor vm = m as VirtualMotor;
                if (vm != null)
                    vm.SimulationTick();
            }
            if (robot.lateralVelocity != 0)
            {
                robot.direction.rotateOnY(robot.lateralVelocity);
                robot.direction.normalize();
            }
            Vector3 v = robot.direction.Clone();
            v.multiplyScalar(robot.velocity);
            robot.position.add(v);

            saveCurPosition();
        }

        public List<PosPoint> getPoints()
        {
            return pointList.ToList();
        }

        public void clearPoints()
        {
            pointList.Clear();
        }

        public void saveCurPosition()
        {
            if (robot.direction.isDifferentDirection(oldDirection) || Vector3.Distance(robot.position, lastPos) > 10)
            {
                PosPoint point = new PosPoint();

                string moveType = "";

                if (robot.velocity < 0)//robot.velocity.isReverseDirection(robot.direction))
                {
                    moveType = "backward";
                }
                else if (robot.velocity > 0)
                {
                    moveType = "forward";
                }

                if (Vector3.Distance(robot.position, lastPos) < 0.001)
                {
                    if (robot.lateralVelocity > 0)
                    {
                        moveType = "turnRight";
                    }
                    else if(robot.lateralVelocity < 0)
                    {
                        moveType = "turnLeft";
                    }

                    point.angle = Vector3.getAngleBetweenVectors(robot.direction, oldDirection);
                }

                if (moveType != "")
                {
                    point.moveType = moveType;
                    point.point = robot.position;

                    pointList.Add(point);

                    oldDirection = robot.direction.Clone();
                    lastPos = robot.position.Clone();
                }
                

            }
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

        public Barrier intersection()
        {
            double x11 = robot.boundRect.point1.x;
            double x12 = robot.boundRect.point2.x;
            double y11 = robot.boundRect.point1.y;
            double y12 = robot.boundRect.point2.y;
            bool res = false;

            foreach (Barrier barrier in barrierList)
            {
                double x21 = barrier.boundRect.point1.x;
                double x22 = barrier.boundRect.point2.x;
                double y21 = barrier.boundRect.point1.y;
                double y22 = barrier.boundRect.point2.y;

                res = x21 < x12 && x22 > x12 && y22 > y11 && y21 < y12 ||
                      x11 < x22 && x12 > x21 && y12 > y21 && y11 < y22;

                if (res)
                {
                    return barrier;
                }
                     
            }

            return null;
        }

        public void collisionTest()
        {
            Barrier barrier = intersection();

            if (barrier != null)
            {
                // do something
            }
        }

    }
}
