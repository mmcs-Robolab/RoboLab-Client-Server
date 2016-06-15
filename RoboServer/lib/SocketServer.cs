using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
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
        
        private TcpListener server;
        private int port;

        public event ConnectionEventHadler Connected;
        public event ConnectionEventHadler Disconnected;
        public event ConnectionEventHadler MessageReceived;
        public event ConnectionEventHadler MessageSent;

        
        private List<ConnectionInfo> connections = new List<ConnectionInfo>();

        private int idCounter;
        private Queue<int> freeIds;

        public SocketServer(string ip, int newPort) 
        {
            port = newPort;
            freeIds = new Queue<int>();
            idCounter = 0;
        }

        public void Start()
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            AsyncCallback aCallback = new AsyncCallback(AcceptTcpClientCallback);
            server.Server.BeginAccept(aCallback, server.Server);
            
        }

        public int getNextId()
        {
            if (freeIds.Count > 0)
                return freeIds.Dequeue();
            else
                return idCounter++;
        }

        public void returnId(int id)
        {
            freeIds.Enqueue(id);
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
            connection.selfID = getNextId();

            //form.Invoke(new Action(() => form.appendSockLogBox("\nNew connection!\n")));
            if (Connected != null)
                Connected(this, new ConnectionEventArgs(connection));

            lock (connection) connections.Add(connection);
            connection.clientSock.BeginReceive(connection.buffer, 0, connection.buffer.Length, 0, new AsyncCallback(ReceiveCallback), connection.clientSock);

        }


       // вызывается когда получаем сообщение
        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket handler = (Socket)ar.AsyncState;
            ConnectionInfo clientInfo = GetConnectionBySock(handler);
            if (clientInfo == null)
                throw new IndexOutOfRangeException("Dead man's letter");
            try
            {
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    clientInfo.command += Encoding.UTF8.GetString(clientInfo.buffer, 0, bytesRead);
                    //ProcessCommand(clientInfo);
                    if (MessageReceived != null)
                        MessageReceived(this, new ConnectionEventArgs(clientInfo));
                    clientInfo.clientSock.BeginReceive(clientInfo.buffer, 0, clientInfo.buffer.Length, 0, new AsyncCallback(ReceiveCallback), handler);
                }
            }
            catch(Exception e)
            {
                RoboLab.Logger.Log(e.Message, this);
            }
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
        private ConnectionInfo getConnectionByID(int selfID)
        {
            lock (connections) connections.Find(item => item.selfID == selfID);
            return null;
        }
        
        // отправка сообщения всем клиентам
        public void SendToAll(string msg)
        {
            
            byte[] byteData = Encoding.UTF8.GetBytes(msg);

            lock (connections) foreach (ConnectionInfo client in connections)
                    client.clientSock.Send(byteData, 0, byteData.Length, 0);
            
        }
        
        public void sendToID(int remoteID, String message)
        {
            ConnectionInfo ci;
            lock (connections) ci = connections.Find(item => item.selfID == remoteID);

            byte[] byteData = Encoding.UTF8.GetBytes(message);

            ci.clientSock.Send(byteData, 0, byteData.Length, 0);

            if (MessageSent != null)
                MessageSent(this, new ConnectionEventArgs(ci));
        }
    }

    public class ConnectionInfo
    {
        public Socket clientSock;
        public string name;
        public int selfID;
        public string command;
        public byte[] buffer = new byte[1024];
    }
}
