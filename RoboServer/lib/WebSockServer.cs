using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fleck;

using Newtonsoft.Json.Linq;

namespace RoboServer.lib
{

    // ========================================================================================
    //              Socket server. Нужен для подключение c# клиента.
    // ========================================================================================
    class WebSockServer
    {
        private WebSocketServer server;

        private string ip;
        private int port;
        private MainForm form;

        private SocketServer sockServer;

        private class WebConnectionInfo
        {
            public IWebSocketConnection clientWebSock;
            public int userID;
        }

        private List<WebConnectionInfo> connections = new List<WebConnectionInfo>();

        public WebSockServer(string newIp, int newPort, MainForm frm)
        {
            ip = "0.0.0.0"; // newIp;
            port = newPort;
            form = frm;
            server = new WebSocketServer("ws://" + ip + ":" + port);
            form.Invoke(new Action(() => frm.appendWebSockLogBox("\nWebSocket server created: ip = " + ip + " port = " + port + "\n")));
        }

        public void Start()
        {
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    form.Invoke(new Action(() => form.appendWebSockLogBox("\nOpen\n")));

                    WebConnectionInfo connection = new WebConnectionInfo();
                    connection.clientWebSock = socket;
                  
                    connections.Add(connection);
                };
                socket.OnClose = () =>
                {
                    form.Invoke(new Action(() => form.appendWebSockLogBox("\nClose\n")));

                    WebConnectionInfo elt = connections.Find(item => item.clientWebSock == socket);
                    connections.Remove(elt);
                };
                socket.OnMessage = message =>
                {
                    form.Invoke(new Action(() => form.appendWebSockLogBox("\nMessage: " + message + "\n")));

                    parseMessage(message, socket);
                   // allSockets.ToList().ForEach(s => s.Send("Echo: " + message));
                };
            });
        }

        public void setSocketServer(SocketServer serv)
        {
            sockServer = serv;
        }

        public void parseMessage(String message, IWebSocketConnection socket)
        {
            switch (message[0])
            {
                case '0':
                    //recieveFile();
                    break;
                case '1':
                    recieveCommand(message, socket);
                    break;
                case '3':
                    parseInfoMessage(message.Replace("3",""), socket);
                    break;
            }
        }

        public void parseInfoMessage(String message, IWebSocketConnection socket)
        {
            int ind = connections.FindIndex(item => item.clientWebSock == socket);
            string[] words = message.Split(' ');
            switch(words[0])
            {
                case "setUserID":
                    connections[ind].userID = int.Parse(words[1]);
                    socket.Send("3setUserID");
                    break;

                case "getClients":
                    JObject sockClient = sockServer.getClientsByUserID(connections[ind].userID);

                    String res = sockClient.ToString();
                    socket.Send("3setClientsList" + "#" + res);
                    break;
            }
        }

        public void recieveCommand(String command, IWebSocketConnection socket)
        {
            String[] arrMess = command.Split(' ');
            int ind = connections.FindIndex(item => item.clientWebSock == socket);

           // if(command == "goForward" || command == "goLeft" || command == "goRight" || command == "goDown" ||)
            sockServer.sendToID(connections[ind].userID, int.Parse(arrMess[1]), int.Parse(arrMess[2]), arrMess[3]);
        }
    }
}
