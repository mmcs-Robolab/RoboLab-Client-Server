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
        
        public event WebConnectionEventHadler Connected;
        public event WebConnectionEventHadler Disconnected;
        public event WebConnectionEventHadler MessageReceived;
        public event WebConnectionEventHadler MessageSent;

        
        private List<WebConnectionInfo> connections = new List<WebConnectionInfo>();

        private int idCounter;
        private Queue<int> freeIds;

        public WebSockServer(string newIp, int newPort)
        {
            ip = "0.0.0.0"; // newIp;
            port = newPort;
            idCounter = 0;
            freeIds = new Queue<int>();
            server = new WebSocketServer("ws://" + ip + ":" + port);
            //form.Invoke(new Action(() => frm.appendWebSockLogBox("\nWebSocket server created: ip = " + ip + " port = " + port + "\n")));
        }

        private int getNextId()
        {
            if (freeIds.Count > 0)
                return freeIds.Dequeue();
            else
                return idCounter++;
        }

        public void Start()
        {
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    //form.Invoke(new Action(() => form.appendWebSockLogBox("\nOpen\n")));

                    WebConnectionInfo connection = new WebConnectionInfo();
                    connection.clientWebSock = socket;
                    connection.userID = getNextId();

                    connections.Add(connection);
                    if (Connected != null)
                        Connected(this, new WebConnectionEventArgs(connection));
                };
                socket.OnClose = () =>
                {
                    //form.Invoke(new Action(() => form.appendWebSockLogBox("\nClose\n")));

                    WebConnectionInfo connection = connections.Find(item => item.clientWebSock == socket);
                    connections.Remove(connection);
                    freeIds.Enqueue(connection.userID);
                    if (Disconnected != null)
                        Disconnected(this, new WebConnectionEventArgs(connection));
                };
                socket.OnMessage = message =>
                {
                    //form.Invoke(new Action(() => form.appendWebSockLogBox("\nMessage: " + message + "\n")));
                    WebConnectionInfo connection = connections.Find(item => item.clientWebSock == socket);
                    connection.message = message;
                    //parseMessage(message, socket);
                    // allSockets.ToList().ForEach(s => s.Send("Echo: " + message));
                    if (MessageReceived != null)
                        MessageReceived(this, new WebConnectionEventArgs(connection));
                };
            });
        }
        
        public void MessageUser(int userID, string message)
        {
            WebConnectionInfo connection = connections.Find(item => item.userID == userID);
            connection.clientWebSock.Send(message);

        }
    }

    public class WebConnectionInfo
    {
        public IWebSocketConnection clientWebSock;
        public int userID;
        public string message;
    }

}
