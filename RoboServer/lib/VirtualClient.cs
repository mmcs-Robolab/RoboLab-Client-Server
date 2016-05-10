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



        private void onReceiveMessage(int UserID, string message)
        {
            if (ReceiveMessage != null)
                ReceiveMessage(this, new MessageEventArgs(UserID, message));
        }

        public VirtualClient()
        {
            dispatcher = new RobotDispatcher();
            simulation = new SimulationController();
            dispatcher.AddBaseRobot("simulated", simulation.getRobot());
        }

        public void BindUserRobot(int UserID, string Robot)
        {
            if (dispatcher.BindUser(UserID, Robot))
                onReceiveMessage(UserID, "bindingResult#Success");
            else
                onReceiveMessage(UserID, "bindingResult#Failure");
        }

        public void CommandRobot(int UserID, string Command)
        {
            dispatcher.GetUserRobot(UserID).Receive(Command);
        }

        public void GetRobots(int UserID)
        {
            onReceiveMessage(UserID, String.Join("#", dispatcher.GetRobots()));
        }

        public void SendSource(int UserID, string Source, string MainClass)
        {
            onReceiveMessage(UserID, "compilationResult#"+dispatcher.RunRobot(dispatcher.GetUserRobotName(UserID), Source, MainClass));
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
