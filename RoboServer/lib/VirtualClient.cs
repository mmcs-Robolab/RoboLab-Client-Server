using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using RoboLab;

namespace RoboServer.lib
{
    class VirtualClient : RobotClient
    {
        public override event ReceiveMessagehandler ReceiveMessage;

        RobotDispatcher dispatcher;
        SimulationController simulation;

        Timer updateTimer;

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
            dispatcher.DispatcherPrint += Dispatcher_DispatcherPrint;
            updateTimer = new Timer(1000);
            updateTimer.AutoReset = true;
            updateTimer.Elapsed += UpdateTimer_Elapsed;
            updateTimer.Start();
        }

        public void ImportScene(string scene)
        {
            simulation.importSceneFromJson(scene);
        }

        private void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string message = "simulationPoints#"+String.Join("#", simulation.getPoints().Select(p => String.Format("{0}#{1}#{2}#{3}", p.point.x, p.point.y, p.point.z, p.moveType)));
            foreach (int user in Users)
                onReceiveMessage(user, message);
        }

        private void Dispatcher_DispatcherPrint(object sender, DispatcherPrintEventArgs args)
        {
            onReceiveMessage(args.UserID, "messageFromRobot#"+args.Message);
        }

        public override void BindUserRobot(int UserID, string Robot)
        {
            if (dispatcher.BindUser(UserID, Robot))
                onReceiveMessage(UserID, "bindingResult#Success");
            else
                onReceiveMessage(UserID, "bindingResult#Failure");
        }

        public override void CommandRobot(int UserID, string Command)
        {
            dispatcher.GetUserRobot(UserID).Receive(Command);
        }

        public override void GetRobots(int UserID)
        {
            onReceiveMessage(UserID, String.Join("#", dispatcher.GetRobots()));
        }

        public override void SendSource(int UserID, string Source, string MainClass)
        {
            string result = dispatcher.RunRobot(dispatcher.GetUserRobotName(UserID), Source, MainClass);
            onReceiveMessage(UserID, "compilationResult#"+result);
            Logger.Log(result, this);
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
