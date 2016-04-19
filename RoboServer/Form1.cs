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
            WebSockServer webSocketServer = new WebSockServer(ip, getWebSocketPort(), this);
            webSocketServer.Start();

            SocketServer socketServer = new SocketServer(ip, getSocketPort(), this);
            socketServer.Start();

            socketServer.setWebSockServer(webSocketServer);
            webSocketServer.setSocketServer(socketServer);

            
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
