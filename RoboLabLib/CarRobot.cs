using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace RoboLab
{
    public class CarRobot : BaseRobot
    {
        public SerialPort port { get; private set; }
        public bool IsConnected { get; private set; }

        public CarRobot()
        {
            port = new SerialPort();
            IsConnected = false;
            port.DataReceived += Port_DataReceived;
        }

        public CarRobot(string portName)
        {
            IsConnected = false;
            port = new SerialPort();
            port.DataReceived += Port_DataReceived;
            OpenConnection(portName);
        }

        public void OpenConnection(string portName)
        {
            if (port.IsOpen)
            {
                port.Close();
            }
            port.PortName = portName;
            port.BaudRate = 9600;
            port.DataBits = 8;
            port.Parity = Parity.None;
            port.StopBits = StopBits.One;
            port.Open();
            port.ReadTimeout = 1000;
            port.WriteLine("STATUS");
            
        }

        
        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string readstring = port.ReadLine();
                IsConnected = true;
            }
            catch (TimeoutException)
            {
            }
        }
        
        
    }

    public class CarMotor : IMotor
    {
        SerialPort port;
        public LogicalMotorType MotorType
        {
            get
            {
                return LogicalMotorType.Forward;
            }
        }

        public CarMotor(SerialPort port)
        {
            this.port = port;
        }

        public void Brake()
        {
            if (port.IsOpen)
            {
                port.WriteLine("L" + 50.ToString());
                //port.WriteLine("HALT");
            }
        }

        public void Run(double power)
        {
            if (port.IsOpen)
            {
                port.WriteLine("L" + ((int)(50 + power * 50)).ToString());
                //port.WriteLine("RUN");
            }
        }
    }

    public class CarSteerMotor : IMotor
    {
        SerialPort port;
        public LogicalMotorType MotorType
        {
            get
            {
                return LogicalMotorType.Steer;
            }
        }

        public CarSteerMotor(SerialPort port)
        {
            this.port = port;
        }

        public void Brake()
        {
            if (port.IsOpen)
            {
                port.WriteLine("S" + 50.ToString());
                //port.WriteLine("HALT");
            }
        }

        public void Run(double power)
        {
            if (port.IsOpen)
            {
                port.WriteLine("S" + ((int)(50 + power * 50)).ToString());
                //port.WriteLine("RUN");
            }
        }
    }
}
