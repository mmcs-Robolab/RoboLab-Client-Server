using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboLab;

namespace RoboServer.lib
{
    class VirtualClient : IRobotClient
    {
        public event ReceiveMessagehandler ReceiveMessage;

        RobotDispatcher dispatcher;
        SimulationController simulation;

        public VirtualClient()
        {
            dispatcher = new RobotDispatcher();
            simulation = new SimulationController();
            dispatcher.AddBaseRobot("simulated", simulation.getRobot());
        }

        public void BindUserRobot(int UserID, string Robot)
        {
            dispatcher.BindUser(UserID, Robot);
        }

        public void CommandRobot(int UserID, string Command)
        {
            
        }

        public void GetRobots()
        {
            
        }

        public void SendSource(int UserID, string Source, string MainClass)
        {
            dispatcher.RunRobot(dispatcher.GetUserRobotName(UserID), Source, MainClass);
        }

        public void StartSimulation()
        {
            simulation.StartSimulation();
        }

        public void StopSimulation()
        {
            simulation.StopSimulation();
        }
    }
}
