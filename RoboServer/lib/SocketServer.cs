using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

using Newtonsoft.Json.Linq;
using System.Net.WebSockets;

namespace RoboServer.lib
{

    // ========================================================================================
    //              Socket server. Нужен для подключение c# клиента.
    // ========================================================================================

    class SocketServer
    {
        private const int MESSAGE_COMMAND_CODE = 1;  // изменить при составлении нормального протокола 

        private TcpListener server;
        private int port;
        private MainForm form;
        private WebSockServer webSockServer;

        public class ConnectionInfo
        {
            public Socket clientSock;
            public string name;
            public int userID;
            public int selfID;
            public List<Device> deviceList;
            public string command;
            public byte[] buffer = new byte[1024];
        }

       // private Thread acceptThread;
        private List<ConnectionInfo> connections = new List<ConnectionInfo>();

        public SocketServer(string ip, int newPort, MainForm frm) 
        {
            form = frm;
            port = newPort;
            form.Invoke(new Action(() => frm.appendSockLogBox("\nSocket server created: ip = " + ip + " port = " + port + "\n")));
        }

        public void Start()
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            AsyncCallback aCallback = new AsyncCallback(AcceptTcpClientCallback);
            server.Server.BeginAccept(aCallback, server.Server);
        }

        // настраиваем и запускаем сервер
        private void AcceptTcpClientCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            Socket clientSocket = client.EndAccept(ar);
            AcceptConnection(clientSocket);
            AsyncCallback aCallback = new AsyncCallback(AcceptTcpClientCallback);
            server.Server.BeginAccept(aCallback, server.Server);
        }
        

        // ловим подключения
        private void AcceptConnection(Socket newClient)
        {
            ConnectionInfo connection = new ConnectionInfo();
            connection.clientSock = newClient;
            connection.name = "New user";

            form.Invoke(new Action(() => form.appendSockLogBox("\nNew connection!\n")));

            lock (connection) connections.Add(connection);
            connection.clientSock.BeginReceive(connection.buffer, 0, connection.buffer.Length, 0, new AsyncCallback(RecieveCallback), connection.clientSock);

        }


       // вызывается когда получаем сообщение
        private void RecieveCallback(IAsyncResult ar)
        {
            Socket handler = (Socket)ar.AsyncState;
            ConnectionInfo clientInfo = GetConnectionBySock(handler);
            if (clientInfo == null)
                throw new IndexOutOfRangeException("Dead man's letter");

            int bytesRead = handler.EndReceive(ar);

            if(bytesRead > 0)
            {
                clientInfo.command += Encoding.UTF8.GetString(clientInfo.buffer, 0, bytesRead);
                ProcessCommand(clientInfo);
                clientInfo.clientSock.BeginReceive(clientInfo.buffer, 0, clientInfo.buffer.Length, 0, new AsyncCallback(RecieveCallback), handler);
            }
        }

        // парсинг и выполнение пришедшей команды
        private bool ProcessCommand(ConnectionInfo client)
        {
            int endPos = client.command.IndexOf(Environment.NewLine);
            if (endPos > -1)
            {
                string []cmds = client.command.Split('#');  // при передаче json объекта, приходится разделять параметры не пробелом, а решеткой. Может стоить всегда так делать.
                client.command.Remove(0, endPos);

                //form.Invoke(new Action(() => form.appendSockLogBox("\nCommand : " + cmd)));

                switch (cmds[0])
                {
                    case "setUserInfo":
                        JObject userJson = JObject.Parse(cmds[1]);

                        client.userID = (int)userJson.GetValue("userID");
                        client.selfID = (int)userJson.GetValue("selfID");
                        client.name = (string)userJson.GetValue("name");
                        //client.deviceList = userJson.GetValue("deviceList");

                        client.deviceList = new List<Device>();
                        foreach (var d in userJson["deviceList"].Children())
                        {
                            client.deviceList.Add(new Device()
                            {
                                id = (int)d["id"],
                                name = (string)d["name"]
                            });
                        }

                        break;
                }

                return true;
            }
            return false;
        }

        //  Поиск соединения по сокету
        private ConnectionInfo GetConnectionBySock(Socket sock)
        {
            lock (connections) foreach (ConnectionInfo ci in connections)
                {
                    if (ci.clientSock == sock) return ci;
                }
            return null;
        }

        // поиск соединения по id юзера и по id клиента 
        private ConnectionInfo getConnectionByID(int userID, int selfID)
        {
            lock (connections) foreach (ConnectionInfo ci in connections)
                {
                    if (ci.userID == userID && ci.selfID == selfID) return ci;
                }
            return null;
        }

        // поиск соединения по id юзера
        private List<ConnectionInfo> getConnectionsByUserID(int id)
        {
            List<ConnectionInfo> curUserConnections = new List<ConnectionInfo>();

            lock (connections) foreach (ConnectionInfo ci in connections)
                {
                    if (ci.userID == id) curUserConnections.Add(ci);
                }
            return curUserConnections;
        }

        // отправка сообщения всем клиентам
        public void SendToAll(string msg)
        {
            //  Строчку в байты

            byte[] byteData = Encoding.UTF8.GetBytes(msg);

            lock (connections) foreach (ConnectionInfo client in connections)
                    client.clientSock.Send(byteData, 0, byteData.Length, 0);
        }

        public void setWebSockServer(WebSockServer webSockServ)
        {
            webSockServer = webSockServ;
        }

        public JObject getClientsByUserID(int id)
        {
            List<ConnectionInfo> curUserConnections = getConnectionsByUserID(id);
            List<JObject> clients = new List<JObject>();

            foreach (ConnectionInfo ci in curUserConnections)
            {
                dynamic client = new JObject();
                client.id = ci.selfID;
                client.name = ci.name;
                if (ci.deviceList != null)
                    client.deviceList = JToken.FromObject(ci.deviceList);
                else
                    client.deviceList = 0;                                          // ХРЕНЬ! ИСПРАВИТЬ!

                clients.Add(client);
            }

            dynamic list = new JObject();
            list.clients = JToken.FromObject(clients);

            return list;
        }

        public void sendToID(int userID, int remoteID, int deviceID, String message)
        {

            ConnectionInfo ci = getConnectionByID(userID, remoteID);

            byte[] byteData = Encoding.UTF8.GetBytes(MESSAGE_COMMAND_CODE + " " + deviceID + " " + message);

            ci.clientSock.Send(byteData, 0, byteData.Length, 0);
        }


    }
}
