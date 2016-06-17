using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using RoboLab;

namespace RoboServer.lib
{
    class VirtualClient : IRobotClient
    {
        public event ReceiveMessagehandler ReceiveMessage;

        RobotDispatcher dispatcher;
        SimulationController simulation;

        Timer updateTimer;

        public SortedSet<int> Users
        {
            get;
            set;
        }

        private void onReceiveMessage(int UserID, string message)
        {
            if (ReceiveMessage != null)
                ReceiveMessage(this, new MessageEventArgs(UserID, message));
        }

        public VirtualClient()
        {
            Users = new SortedSet<int>();
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
            string message = "simulationPoints#"+ string.Join("#", simulation.getPoints().Select(p => string.Format("{0}#{1}#{2}#{3}", p.point.x, p.point.y, p.point.z, p.moveType)));
            foreach (int user in Users)
                onReceiveMessage(user, message);
        }

        private void Dispatcher_DispatcherPrint(object sender, DispatcherPrintEventArgs args)
        {
            onReceiveMessage(args.UserID, "messageFromRobot#"+args.Message);
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
            onReceiveMessage(UserID, "robots#"+String.Join("#", dispatcher.GetRobots()));
        }

        public void SendSource(int UserID, string Source, string MainClass)
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
            updateTimer.Stop();
            foreach (string robot in dispatcher.GetRobots().ToArray())
                dispatcher.StopRobot(robot);
        }

        public void ManualControl(int UserID)
        {
            dispatcher.ManualControl(UserID);
        }

        public void UnbindUser(int UserID)
        {
            Users.Remove(UserID);
            dispatcher.UnbindUser(UserID);
        }
    }
}
