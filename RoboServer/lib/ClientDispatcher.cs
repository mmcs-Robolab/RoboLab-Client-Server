using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboServer.lib
{
    /*class ClientDispatcher
    {
        SocketServer socketServer;
        WebSockServer webSocketServer;
        Dictionary<int, int> userBindings;

        Dictionary<int, IRobotClient> robotClients;

        public ClientDispatcher()
        {
            userBindings = new Dictionary<int, int>();
            robotClients = new Dictionary<int, IRobotClient>();

        }

        private void processMessage(int userID, string message)
        {
            string[] messageParts = message.Split('#');
            if (messageParts.Length == 0)
                return;
            switch (messageParts[0])
            {
                case "listServers":
                    webSocketServer.MessageUser(userID, String.Join("#", getServerList()));
                    break;
                case "chooseServer":
                    int server;
                    if (messageParts.Length < 2 || int.TryParse(messageParts[1], out server))
                        webSocketServer.MessageUser(userID, "chosenServer#Failure");
                    else
                    {
                        userBindings[userID] = server;
                        robotClients[server].Users.Add(userID);
                        webSocketServer.MessageUser(userID, "chosenServer#Success");
                    }
                    break;
                case "createSimulation":
                    int id = socketServer.getNextId();
                    robotClients[id] = new VirtualClient();
                    robotClients[id].ReceiveMessage += MainForm_ReceiveMessage;
                    userBindings[userID] = id;
                    webSocketServer.MessageUser(userID, "creationResult#..."); //
                    robotClients[id].BindUserRobot(userID, "simulated");
                    robotClients[id].Users.Add(userID);
                    ((VirtualClient)robotClients[id]).ImportScene(String.Concat(messageParts.Skip(1)));
                    ((VirtualClient)robotClients[id]).StartSimulation();
                    break;
                case "listRobots":
                    if (!userBindings.ContainsKey(userID) || !robotClients.ContainsKey(userBindings[userID]))
                        webSocketServer.MessageUser(userID, "robots#Failure");
                    else
                        robotClients[userBindings[userID]].GetRobots(userID);
                    break;
                case "bindToRobot":
                    if (!userBindings.ContainsKey(userID) || !robotClients.ContainsKey(userBindings[userID]) || messageParts.Length < 2)
                        webSocketServer.MessageUser(userID, "bindingResult#Failure");
                    else
                        robotClients[userBindings[userID]].BindUserRobot(userID, messageParts[1]);
                    break;
                case "messageRobot":
                    if (!userBindings.ContainsKey(userID) || !robotClients.ContainsKey(userBindings[userID]) || messageParts.Length < 2)
                        webSocketServer.MessageUser(userID, "messageRobot#Failure");
                    else
                        robotClients[userBindings[userID]].CommandRobot(userID, String.Join("#", messageParts.Skip(1)));
                    break;
                case "compileRobot":
                    if (!userBindings.ContainsKey(userID) || !robotClients.ContainsKey(userBindings[userID]) || messageParts.Length < 3)
                        webSocketServer.MessageUser(userID, "compilationResult#Failure");
                    else
                        robotClients[userBindings[userID]].SendSource(userID, String.Concat(messageParts.Skip(2)), messageParts[1]);
                    break;
                case "pauseSimulation":
                    if (!userBindings.ContainsKey(userID) || !robotClients.ContainsKey(userBindings[userID]) || !(robotClients[userBindings[userID]] is VirtualClient))
                        webSocketServer.MessageUser(userID, "pauseSimulation#Failure");
                    else
                        (robotClients[userBindings[userID]] as VirtualClient).StopSimulation();
                    break;
                case "resumeSimulation":
                    if (!userBindings.ContainsKey(userID) || !robotClients.ContainsKey(userBindings[userID]) || !(robotClients[userBindings[userID]] is VirtualClient))
                        webSocketServer.MessageUser(userID, "resumeSimulation#Failure");
                    else
                        (robotClients[userBindings[userID]] as VirtualClient).StartSimulation();
                    break;
            }
        }*/
}
