using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quobject.SocketIoClientDotNet.Client;

using RoboServer.lib;

namespace RoboServer
{
    public partial class MainForm : Form
    {
        private string ip;
        SocketServer socketServer;
        WebSockServer webSocketServer;
        Dictionary<int, int> userBindings;

        Dictionary<int, IRobotClient> robotClients;

        public MainForm()
        {
            InitializeComponent();
        }


        // создает socket server
        //private void createServerBtn_Click(object sender, EventArgs e)
        //{
        //    WebSockServer socketServer = new WebSockServer(ip, getWebSocketPort(), this);
        //    socketServer.Start();
        //}

        private void createSockServerBtn_Click(object sender, EventArgs e)
        {
            userBindings = new Dictionary<int, int>();
            robotClients = new Dictionary<int, IRobotClient>();

            webSocketServer = new WebSockServer(ip, getWebSocketPort());
            webSocketServer.Start();

            socketServer = new SocketServer(ip, getSocketPort());
            socketServer.Start();

            webSocketServer.Connected += WebSocketServer_Connected;
            webSocketServer.Disconnected += WebSocketServer_Disconnected;
            webSocketServer.MessageReceived += WebSocketServer_MessageReceived;
            webSocketServer.MessageSent += WebSocketServer_MessageSent;

            socketServer.Connected += SocketServer_Connected;
            socketServer.Disconnected += SocketServer_Disconnected;
            socketServer.MessageReceived += SocketServer_MessageReceived;
            socketServer.MessageSent += SocketServer_MessageSent;
        }

        private void SocketServer_MessageSent(object sender, ConnectionEventArgs args)
        {
            appendSockLogBox("Message sent from "+args.Connection.selfID);
        }

        private void SocketServer_MessageReceived(object sender, ConnectionEventArgs args)
        {
            appendSockLogBox("Message \"" + args.Connection.command + "\" received by " + args.Connection.selfID);
        }

        private void SocketServer_Disconnected(object sender, ConnectionEventArgs args)
        {
            appendSockLogBox(args.Connection.selfID + " disconnected");
        }

        private void SocketServer_Connected(object sender, ConnectionEventArgs args)
        {
            appendSockLogBox(args.Connection.selfID + " connected");
            robotClients[args.Connection.selfID] = new RobotClientProxy();
        }

        private void WebSocketServer_MessageSent(object sender, WebConnectionEventArgs args)
        {
            appendWebSockLogBox("Message sent to " + args.Connection.userID);
        }

        private void WebSocketServer_MessageReceived(object sender, WebConnectionEventArgs args)
        {
            appendWebSockLogBox("Message \"" + args.Connection.message + "\" received by " + args.Connection.userID);
            string[] parts = args.Connection.message.Split('#');
            if (parts.Count() == 0)
                return;
            switch(parts[0])
            {
                case "bindToClient":
                    int clientID;
                    if(int.TryParse(parts[1], out clientID))
                    {
                        
                    }
                    break;
            }
        }

        private void WebSocketServer_Disconnected(object sender, WebConnectionEventArgs args)
        {
            appendWebSockLogBox(args.Connection.userID + " disconnected");
        }

        private void WebSocketServer_Connected(object sender, WebConnectionEventArgs args)
        {
            appendWebSockLogBox(args.Connection.userID + " connected");
        }

        public void appendWebSockLogBox(string txt)
        {
            logWebSocketText.AppendText(txt);
        }

        public void appendSockLogBox(string txt)
        {
            logSocketText.AppendText(txt);
        }

        public void changeLogBoxColor(Color clr)
        {
            logWebSocketText.ForeColor = clr;
        }

        public int getWebSocketPort()
        {
            return int.Parse(webSockPortText.Text);
        }

        public int getSocketPort()
        {
            return int.Parse(sockPortText.Text);
        }

        public void setIp(string newIp)
        {
            ip = newIp;
            webSockIpText.Text = ip;
        }
    }
}
