using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoboLab;

namespace RoboClient
{
    class RobotClient
    {
        public RobolabConnection Connection
        {
            get; private set;
        }
        RobotDispatcher dispatcher;
        DataAccumulator userMessages;
        public RobotClient()
        {
            Connection = new RobolabConnection();
            dispatcher = new RobotDispatcher();
            userMessages = new DataAccumulator();
            Connection.DataReceived += Connection_DataReceived;
            userMessages.DataReceived += UserMessages_DataReceived;
            dispatcher.DispatcherPrint += Dispatcher_DispatcherPrint;
        }

        private void UserMessages_DataReceived(object sender, MessageReceivedEventArgs e)
        {
            string[] parts = e.Message.Split('#');
            int userID;

            if (e.Message.Length == 0 || parts.Length < 2 || !int.TryParse(parts[1], out userID))
                return;

            switch (parts[0])
            {
                case "bindUser":
                    if (dispatcher.BindUser(userID, parts[2]))
                        sendToUser(userID, "bindingResult#Success");
                    else
                        sendToUser(userID, "bindingResult#Failure");
                    break;
                case "unbindUser":
                    dispatcher.UnbindUser(userID);
                    break;
                case "messageRobot":
                    dispatcher.GetUserRobot(userID).Receive(String.Join("#", parts.Skip(2)));
                    break;
                case "sendRobots":
                    sendToUser(userID, "robots#" + String.Join("#", dispatcher.GetRobots()));
                    break;
                case "sendSource":
                    if (parts.Length < 4)
                        return;
                    
                    string result = dispatcher.RunRobot(dispatcher.GetUserRobotName(userID), String.Concat(parts.Skip(3)), parts[2]);
                    sendToUser(userID, "compilationResult#" + result);
                    Logger.Log(result, this);
                    
                    break;
            }
        }

        private void Dispatcher_DispatcherPrint(object sender, DispatcherPrintEventArgs args)
        {
            sendToUser(args.UserID, "messageFromRobot#"+args.Message);
        }

        private void sendToUser(int UserID, string message)
        {
            string fullMessage = UserID.ToString() + "#" + message;
            Connection.Send(fullMessage.Length.ToString() + "#" + fullMessage);
        }

        private void Connection_DataReceived(object sender, DataReceivedEventArgs e)
        {
            userMessages.AcceptData(e.Message);
        }

        public void addRobot(String name, BaseRobot baseRobot)
        {
            dispatcher.AddBaseRobot(name, baseRobot);
        }

        public IEnumerable<string> GetRobots()
        {
            return dispatcher.GetRobots();
        }

    }
    
}
