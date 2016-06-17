using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace RoboServer.lib
{
    internal class RobotClientProxy : IRobotClient
    {
        ConnectionInfo connection;
        public event ReceiveMessagehandler ReceiveMessage;
        byte[] recvBuffer;
        RoboLab.DataAccumulator dataAccumulator;

        public event DisconnectedHandler Disconnected;

        public string StreamURL { get; private set; }
        public SortedSet<int> Users
        {
            get;
            set;
        }

        public RobotClientProxy(ConnectionInfo con)
        {
            Users = new SortedSet<int>();
            connection = con;
            dataAccumulator = new RoboLab.DataAccumulator();
            recvBuffer = new byte[1024];
            StreamURL = "";
            connection.clientSock.BeginReceive(recvBuffer, 0, 1024, System.Net.Sockets.SocketFlags.None, new AsyncCallback(receiveCallback), connection.clientSock);
            dataAccumulator.DataReceived += DataAccumulator_DataReceived;
        }
        private void onReceiveMessage(int userID, string message)
        {
            if (ReceiveMessage != null)
                ReceiveMessage(this, new MessageEventArgs(userID, message));
        }
        private void DataAccumulator_DataReceived(object sender, RoboLab.MessageReceivedEventArgs e)
        {
            string[] parts = e.Message.Split('#');
            int userID;
            if (parts.Length >= 2 && int.TryParse(parts[0], out userID))
            {
                if (userID != -1)
                    onReceiveMessage(userID, String.Join("#", parts.Skip(1)));
                else
                {
                    foreach (int user in Users)
                        onReceiveMessage(user, String.Join("#", parts.Skip(1)));
                    if (parts[1] == "videoStream")
                        StreamURL = String.Join("#", parts.Skip(2));
                }
            }
        }

        private void receiveCallback(IAsyncResult ar)
        {
            try
            {
                int bytesRead = connection.clientSock.EndReceive(ar);

                if (bytesRead > 0)
                {
                    string fullMessage = Encoding.UTF8.GetString(recvBuffer, 0, bytesRead);
                    RoboLab.Logger.Log("\""+fullMessage + "\" received by " + connection.selfID.ToString());
                    dataAccumulator.AcceptData(fullMessage);
                    connection.clientSock.BeginReceive(recvBuffer, 0, 1024, System.Net.Sockets.SocketFlags.None, new AsyncCallback(receiveCallback), connection.clientSock);
                }
            }
            catch(Exception e)
            {
                RoboLab.Logger.Log(e.Message, this);
            }
            connection.clientSock.BeginReceive(recvBuffer, 0, 1024, System.Net.Sockets.SocketFlags.None, new AsyncCallback(receiveCallback), connection.clientSock);
        }

        private void sendMessage(string message)
        {
            CheckConnection();
            if (IsConnected())
            {
                string msg = message.Length.ToString() + "#" + message;
                byte[] bytes = Encoding.UTF8.GetBytes(msg);
                connection.clientSock.BeginSend(bytes, 0, bytes.Length, System.Net.Sockets.SocketFlags.None, new AsyncCallback(EndSendCallback), connection.clientSock);
            }
        }

        private void EndSendCallback(IAsyncResult ar)
        {
            connection.clientSock.EndSend(ar);
        }

        public void BindUserRobot(int UserID, string Robot)
        {
            Users.Add(UserID);
            sendMessage("bindUser#" + UserID.ToString() + "#" + Robot);
        }

        public void UnbindUser(int UserID)
        {
            Users.Remove(UserID);
            sendMessage("unbindUser#" + UserID.ToString());
        }

        public void CommandRobot(int UserID, string Command)
        {
            sendMessage("messageRobot#" + UserID.ToString() + "#" + Command);
        }

        public void GetRobots(int UserID)
        {
            sendMessage("sendRobots#"+UserID.ToString());
        }

        public void SendSource(int UserID, string Source, string MainClass)
        {
            sendMessage("sendSource#" + UserID.ToString() + "#" + MainClass+"#"+Source);
            
        }

        public void CheckConnection()
        {
            if (!IsConnected() && Disconnected != null)
                Disconnected(this, new DisconnectedEventArgs(this));
        }

        public bool IsConnected()
        {
            return connection.clientSock.Connected;
        }

        public int GetID()
        {
            return connection.selfID;
        }

        public void ManualControl(int UserID)
        {
            sendMessage("manualControl#" + UserID.ToString());
        }
    }

    internal class DisconnectedEventArgs : EventArgs
    {
        public RobotClientProxy Client { get; private set; }

        public DisconnectedEventArgs(RobotClientProxy client)
        {
            Client = client;
        }
    }

    internal delegate void DisconnectedHandler(object sender, DisconnectedEventArgs e);
}
