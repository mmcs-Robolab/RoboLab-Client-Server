using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RoboLab;
using System.Reflection;
using System.IO;

namespace RoboClient
{
    public partial class Form1 : Form
    {
        
        RobotClient client;
        public Form1()
        {
            InitializeComponent();
            client = new RobotClient();
        }

        private void Connection_ConnectFailed(object sender, EventArgs e)
        {
            this.Invoke(() => serverPanel.Enabled = true);
        }

        private void Connection_AuthoriseFailed(object sender, EventArgs e)
        {
            this.Invoke(() =>
            {
                nameText.Enabled = true;
                passText.Enabled = true;
                authBtn.Enabled = true;
            });
        }

        private void Connection_Disconnected(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Connection_DataReceived(object sender, DataReceivedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Connection_Connected(object sender, EventArgs e)
        {
            
        }

        private void Connection_Authorised(object sender, EventArgs e)
        {
            this.Invoke(() => serverPanel.Enabled = true);
        }

        private void Logger_LogUpdated(object sender, LogUpdateEventArgs e)
        {
            this.Invoke(()=>logText.AppendText(e.Message+Environment.NewLine));
            
        }

        private void Invoke(System.Action p)
        {
            this.Invoke((Delegate)p);
        }

        private void authBtn_Click(object sender, EventArgs e)
        {

            nameText.Enabled = false;
            passText.Enabled = false;
            authBtn.Enabled = false;
            client.Connection.Authorise(nameText.Text, passText.Text);
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            int port;
            if (int.TryParse(portText.Text, out port))
            {
                serverPanel.Enabled = false;
                //this.BeginInvoke(()=> connection.Connect(ipText.Text, port, pointNameText.Text));
                //Task.Run(() => connection.Connect(ipText.Text, port, pointNameText.Text));
                client.Connection.Connect(ipText.Text, port, pointNameText.Text);
            }
            else
                Logger.Log("Порт должен быть числом.");
        }

        private void BeginInvoke(System.Action p)
        {
            this.Invoke((Delegate)p);
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            Logger.LogUpdated += Logger_LogUpdated;
            Logger.Log("Starting", this);

            client.Connection.Authorised += Connection_Authorised;
            client.Connection.Connected += Connection_Connected;
            client.Connection.DataReceived += Connection_DataReceived;
            client.Connection.Disconnected += Connection_Disconnected;
            client.Connection.AuthoriseFailed += Connection_AuthoriseFailed;
            client.Connection.ConnectFailed += Connection_ConnectFailed;
            
            /*dispatcher.AddBaseRobot("test", new DummyRobot());

            string s = File.ReadAllText("../../../RoboLabLib/TestRobot.cs");
            dispatcher.RunRobot("test", s, "RoboLab.TestRobot");

            Timer t = new Timer();
            t.Interval = 1000;
            t.Tick += T_Tick;
           
            t.Start();*/
        }
        /*
        private void T_Tick(object sender, EventArgs e)
        {
            Logger.Log("Stop");
            dispatcher.StopRobot("test");
            ((Timer)sender).Stop();
        }*/
    }
}
