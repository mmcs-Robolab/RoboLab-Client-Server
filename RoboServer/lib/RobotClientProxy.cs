using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace RoboServer.lib
{
    internal class RobotClientProxy : RobotClient
    {
        ConnectionInfo connection;
        public override event ReceiveMessagehandler ReceiveMessage;
        byte[] recvBuffer;

        public RobotClientProxy(ConnectionInfo con)
        {
            connection = con;
            recvBuffer = new byte[1024];
            connection.clientSock.BeginReceive(recvBuffer, 0, 1024, System.Net.Sockets.SocketFlags.None, new AsyncCallback(receiveCallback), connection.clientSock);
        }

        private void receiveCallback(IAsyncResult ar)
        {
            
            int bytesRead = connection.clientSock.EndReceive(ar);
            
            if (bytesRead > 0)
            {
                string fullMessage = Encoding.UTF8.GetString(recvBuffer, 0, bytesRead);
                string[] parts = fullMessage.Split('#');
                int userID;
                if(parts.Length >= 2 && int.TryParse(parts[0], out userID))
                    if (ReceiveMessage != null)
                        ReceiveMessage(this, new MessageEventArgs(userID, String.Join("#", parts.Skip(1))));
                connection.clientSock.BeginReceive(recvBuffer, 0, 1024, System.Net.Sockets.SocketFlags.None, new AsyncCallback(receiveCallback), connection.clientSock);
            }
        }

        private void sendMessage(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            connection.clientSock.BeginSend(bytes, 0, bytes.Length, System.Net.Sockets.SocketFlags.None, new AsyncCallback(EndSendCallback), connection.clientSock);
        }

        private void EndSendCallback(IAsyncResult ar)
        {
            connection.clientSock.EndSend(ar);
        }

        public override void BindUserRobot(int UserID, string Robot)
        {
            sendMessage("bindUser#" + UserID.ToString() + "#" + Robot);
        }

        public void UnbindUser(int UserID)
        {
            sendMessage("unbindUser#" + UserID.ToString());
        }

        public override void CommandRobot(int UserID, string Command)
        {
            sendMessage("messageRobot#" + UserID.ToString() + "#" + Command);
        }

        public override void GetRobots(int UserID)
        {
            sendMessage("sendRobots#"+UserID.ToString());
        }

        public override void SendSource(int UserID, string Source, string MainClass)
        {
            ////
            string header = "sendSource#" + UserID.ToString() + "#";
            byte[] sourceBytes = Encoding.UTF8.GetBytes(Source);
            byte[] headerBytes = Encoding.UTF8.GetBytes(header);
            int partSize = 1024 - headerBytes.Length;
            int parts = (int)Math.Ceiling((double)sourceBytes.Length / partSize);
            sendMessage("beginSendSource#" + UserID.ToString() + "#" + MainClass+"#"+parts.ToString());
            
            for (int i = 0; i < parts; ++i)
            {
                //sendMessage(header + sourceBytes.Skip(1024 * i));
                byte[] bytes = headerBytes.Concat(sourceBytes.Skip(partSize * i)).ToArray();
                connection.clientSock.BeginSend(bytes, 0, bytes.Length, System.Net.Sockets.SocketFlags.None, new AsyncCallback(EndSendCallback), connection.clientSock);
            }
            
        }
    }
}
