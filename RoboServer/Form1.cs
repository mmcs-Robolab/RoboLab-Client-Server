﻿using System;
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
            RoboLab.Logger.LogUpdated += Logger_LogUpdated;
        }

        private void Logger_LogUpdated(object sender, RoboLab.LogUpdateEventArgs e)
        {
            this.Invoke(()=>logSocketText.AppendText(e.Message+Environment.NewLine));
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
            this.Invoke(() => appendSockLogBox("Message sent from " + args.Connection.selfID));
        }

        private void SocketServer_MessageReceived(object sender, ConnectionEventArgs args)
        {
            this.Invoke(() => appendSockLogBox("Message \"" + args.Connection.command + "\" received by " + args.Connection.selfID));
        }

        private void SocketServer_Disconnected(object sender, ConnectionEventArgs args)
        {
            this.Invoke(() => appendSockLogBox(args.Connection.selfID + " disconnected"));
        }

        private void SocketServer_Connected(object sender, ConnectionEventArgs args)
        {
            this.Invoke(()=>
            {
                appendSockLogBox(args.Connection.selfID + " connected");
                RobotClientProxy rcp = new RobotClientProxy(args.Connection);
                robotClients[args.Connection.selfID] = rcp;
                rcp.ReceiveMessage += MainForm_ReceiveMessage;
                rcp.Disconnected += Rcp_Disconnected;
            });
        }

        private void Rcp_Disconnected(object sender, DisconnectedEventArgs e)
        {
            robotClients.Remove(e.Client.GetID());
            socketServer.returnId(e.Client.GetID());
            
        }

        private void WebSocketServer_MessageSent(object sender, WebConnectionEventArgs args)
        {
            this.Invoke(()=>appendWebSockLogBox("Message sent to " + args.Connection.userID));
        }
       
        private IEnumerable<string> getServerList()
        {
            foreach(IRobotClient rc in robotClients.Values)
            {
                RobotClientProxy rcp = rc as RobotClientProxy;
                //rcp?.CheckConnection();
                if (rcp != null)
                    rcp.CheckConnection();
            }
            return robotClients.Keys.Select(x => x.ToString());

        }

        private void processMessage(int userID, string message)
        {
            string[] messageParts = message.Split('#');
            if (messageParts.Length == 0)
                return;
            switch (messageParts[0])
            {
                case "listServers":
                    webSocketServer.MessageUser(userID, "servers#" + String.Join("#", getServerList()));
                    break;
                case "chooseServer":
                    int server;
                    if (messageParts.Length < 2 || !int.TryParse(messageParts[1], out server))
                        webSocketServer.MessageUser(userID, "chosenServer#Failure");
                    else
                    {
                        userBindings[userID] = server;
                        robotClients[server].Users.Add(userID);
                        webSocketServer.MessageUser(userID, "chosenServer#Success");
                        RobotClientProxy rcp = robotClients[server] as RobotClientProxy;
                        if (rcp != null)
                            webSocketServer.MessageUser(userID, "videoStream#" + rcp.StreamURL);
                    }
                    break;
                case "createSimulation":
                    int id = socketServer.getNextId();
                    VirtualClient vc = new VirtualClient();
                    robotClients[id] = vc;
                    vc.ReceiveMessage += MainForm_ReceiveMessage;
                    userBindings[userID] = id;
                    webSocketServer.MessageUser(userID, "creationResult#..."); //
                    vc.BindUserRobot(userID, "simulated");
                    vc.Users.Add(userID);
                    vc.ImportScene(String.Concat(messageParts.Skip(1)));
                    vc.StartSimulation();
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
                    if(!userBindings.ContainsKey(userID) || !robotClients.ContainsKey(userBindings[userID]) || !(robotClients[userBindings[userID]] is VirtualClient))
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
                case "manualControl":
                    if (!userBindings.ContainsKey(userID) || !robotClients.ContainsKey(userBindings[userID]))
                        webSocketServer.MessageUser(userID, "manualControl#Failure");
                    else
                        robotClients[userBindings[userID]].ManualControl(userID);
                    break;

            }
        }

        private void MainForm_ReceiveMessage(object sender, MessageEventArgs args)
        {
            webSocketServer.MessageUser(args.UserID, args.Message);
        }

        private void WebSocketServer_MessageReceived(object sender, WebConnectionEventArgs args)
        {
            this.Invoke(() =>
            {
                appendWebSockLogBox("Message \"" + args.Connection.message + "\" received by " + args.Connection.userID);

                processMessage(args.Connection.userID, args.Connection.message);
            });
            
        }

        private void WebSocketServer_Disconnected(object sender, WebConnectionEventArgs args)
        {
            robotClients[args.Connection.userID].UnbindUser(args.Connection.userID);
            this.Invoke(()=>appendWebSockLogBox(args.Connection.userID + " disconnected"));
        }

        private void WebSocketServer_Connected(object sender, WebConnectionEventArgs args)
        {
            this.Invoke(()=>appendWebSockLogBox(args.Connection.userID + " connected"));
        }

        private void Invoke(Action a)
        {
            this.Invoke((Delegate)a);
        }

        public void appendWebSockLogBox(string txt)
        {
            logWebSocketText.AppendText(txt+Environment.NewLine);
        }

        public void appendSockLogBox(string txt)
        {
            logSocketText.AppendText(txt + Environment.NewLine);
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
