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

namespace RoboClient
{
    public partial class Form1 : Form
    {
        RobolabConnection connection;
        Dictionary<string, Robot> robots;
        public Form1()
        {
            InitializeComponent();
            connection = new RobolabConnection();
            Logger.LogUpdated += Logger_LogUpdated;
            Logger.Log("Starting", this);
            robots = new Dictionary<string, Robot>();
        }

        private void addRobot(String name, BaseRobot baseRobot)
        {
            
        }

        private void Logger_LogUpdated(object sender, LogUpdateEventArgs e)
        {
            logText.AppendText(e.Message);
        }

        private void authBtn_Click(object sender, EventArgs e)
        {
            connection.Authorise(nameText.Text, passText.Text);
        }
    }
}
