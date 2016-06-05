using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using RoboLab;
using Newtonsoft.Json.Linq;

namespace RoboClient
{

    class RobolabConnection
    {
        private IPHostEntry ipHost;
        private IPAddress ipAddr;
        private IPEndPoint endPoint;
        private Socket sClient;

        //  Флажок установленного соединения
        public bool IsConnected
        {
            get; private set;
        } = false;
        public bool IsAuthorised
        {
            get; private set;
        } = false;
        private string pointName;
        private string userName;
        //private List<Device> deviceList;
        public delegate void ConnectionEventHandler(Object sender, EventArgs e);
        public event ConnectionEventHandler Connected;
        public event ConnectionEventHandler ConnectFailed;
        public event ConnectionEventHandler Disconnected;
        public event ConnectionEventHandler Authorised;
        public event ConnectionEventHandler AuthoriseFailed;

        public delegate void DataReceivedEventHandler(Object sender, DataReceivedEventArgs e);
        public event DataReceivedEventHandler DataReceived;

        public RobolabConnection()
        {

        }
        
        private void onConnected()
        {
            if (Connected != null)
                Connected(this, new EventArgs());
            IsConnected = true;
        }
        private void onConnectFailed()
        {
            if (ConnectFailed != null)
                ConnectFailed(this, new EventArgs());
            IsConnected = false;
        }
        private void onDisconnected()
        {
            if (Disconnected != null)
                Disconnected(this, new EventArgs());
            IsConnected = false;
        }
        private void onAuthorised()
        {
            IsAuthorised = true;
            if (Authorised != null)
                Authorised(this, new EventArgs());
        }

        private void onAuthoriseFailed()
        {
            IsAuthorised = false;
            if (AuthoriseFailed != null)
                AuthoriseFailed(this, new EventArgs());
        }
        private void onDataReceived(String msg)
        {
            if (DataReceived != null)
                DataReceived(this, new DataReceivedEventArgs(msg));
        }

        public void Connect(String ip, int port, String pointName)
        {
            try
            {
                Logger.Log(String.Format("Подключение к {0} {1}:{2}...", pointName, ip, port), this);
                ipHost = Dns.Resolve(ip);//Dns.GetHostEntry(ip);//Dns.Resolve(ip);//("192.168.0.174");
                ipAddr = ipHost.AddressList[0];
                endPoint = new IPEndPoint(ipAddr, port);
                this.pointName = pointName;
                sClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sClient.BeginConnect(endPoint, new AsyncCallback(ConnectCallback), sClient);
            }
            catch(SocketException e)
            {
                Logger.Log(e.Message,this);
                onConnectFailed();
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            //  Получаем сокет
            Socket sClient = (Socket)ar.AsyncState;
            try
            {
                sClient.EndConnect(ar);
            }
            catch(Exception e)
            {
                Logger.Log(e.Message, this);
                onConnectFailed();
                return;
            }
            onConnected();
            byte[] receiveBuffer = new byte[1024];
            //  тут начать слушание сервера пока не отключились
            sClient.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, 0, new AsyncCallback(ReceiveCallback), 0);

            JObject userInfo = new JObject();
            userInfo.Add("name", pointName);
            /*if (deviceList != null)
                userInfo.Add("deviceList", JToken.FromObject(deviceList));
            else
                userInfo.Add("deviceList", "");*/

            byte[] sendBuffer = Encoding.UTF8.GetBytes("setUserInfo" + "#" + userInfo.ToString() + Environment.NewLine);
            sClient.BeginSend(sendBuffer, 0, sendBuffer.Length, 0, new AsyncCallback(SendCallback), sClient);

        }
        private void SendCallback(IAsyncResult ar)
        {
            Logger.Log("Данные отправлены",this);
            //  тут начать слушание сервера пока не отключились
            sClient.EndSend(ar);

        }


        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                byte[] receiveBuffer = ar.AsyncState as byte[];
                int BytesRead = sClient.EndReceive(ar);
                if (BytesRead > 0)
                {
                    string Response = Encoding.UTF8.GetString(receiveBuffer, 0, BytesRead);
                    onDataReceived(Response);
                    sClient.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, 0, new AsyncCallback(ReceiveCallback), receiveBuffer);
                }
            }
            catch(SocketException e)
            {
                Logger.Log(e.Message, this);
            }
        }
        

        public void Send(String message)
        {
            byte[] sendBuffer = Encoding.UTF8.GetBytes(message);
            sClient.BeginSend(sendBuffer, 0, sendBuffer.Length, 0, new AsyncCallback(SendCallback), sClient);
        }

        // авторизация на robolab, получение данных о пользователе
        public void Authorise(String name, String pass)
        {
            string URLAuth = "http://195.208.237.193:3000/auth";// - это боевой сервер, потом заменить на него. "http://192.168.0.125:3000/auth"; 
            string postString = string.Format("login={0}&pass={1}", name, pass);

            const string contentType = "application/x-www-form-urlencoded";
            System.Net.ServicePointManager.Expect100Continue = false;

            CookieContainer cookies = new CookieContainer();
            HttpWebRequest webRequest = WebRequest.Create(URLAuth) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = contentType;
            webRequest.CookieContainer = cookies;
            webRequest.ContentLength = postString.Length;
            webRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.1) Gecko/2008070208 Firefox/3.0.1";
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            webRequest.Referer = "195.208.237.193:3000";

            webRequest.GetRequestStreamAsync().ContinueWith(s =>
            {
                StreamWriter requestWriter = new StreamWriter(s.Result);
                requestWriter.Write(postString);
                requestWriter.Close();

                try
                {
                    using (WebResponse response = webRequest.GetResponse())
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://195.208.237.193:3000/auth/userInfo");//"http://192.168.0.125:3000/auth/userInfo"); // и тут тоже
                        request.CookieContainer = cookies;
                        HttpWebResponse responseUserInfo = (HttpWebResponse)request.GetResponse();
                        if (responseUserInfo.StatusCode == HttpStatusCode.Forbidden)
                        {
                            Logger.Log("Невозможно получить данные пользователя", this);
                        }
                        else if (responseUserInfo.StatusCode == HttpStatusCode.OK)
                        {
                            var encoding = Encoding.UTF8;
                            using (var reader = new System.IO.StreamReader(responseUserInfo.GetResponseStream(), encoding))
                            {
                                string responseText = reader.ReadToEnd();

                                JObject json = JObject.Parse(responseText);
                                userName = (string)json.GetValue("username");

                            }
                            responseUserInfo.Close();
                            Logger.Log("Вы вошли как: " + userName, this);
                            onAuthorised();
                            return;
                        }
                    }

                }
                catch (WebException exp)
                {
                    /*using (WebResponse response = exp.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        if (httpResponse != null)
                        {
                            switch ((int)httpResponse.StatusCode)
                            {
                                case 401:
                                    Logger.Log("Неверный логин или пароль", this);
                                    break;
                            }
                        }
                    }*/
                    Logger.Log(exp.Message, this);
                }
                onAuthoriseFailed();
            });
        }
    }

    public class DataReceivedEventArgs : EventArgs
    {
        public String Message { private set; get; }
        public DataReceivedEventArgs(String msg = "")
        {
            Message = msg;
        }
    }
}
