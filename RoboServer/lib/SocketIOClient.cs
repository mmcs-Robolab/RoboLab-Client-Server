using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quobject.SocketIoClientDotNet.Client;
using System.Drawing;

// нужен для того, чтобы сообщать мехматовскому серверу, о том, что ip изменился (пока не обязательно, в перспективе - для переносимости)

namespace RoboServer.lib
{
    class SocketIOClient
    {
        private Socket socket;
        private string address;
        private MainForm frm;

        public SocketIOClient(string addr, MainForm mainForm)
        {
            frm = mainForm;
            address = addr;
            socket = IO.Socket(addr);

            socket.On(Socket.EVENT_CONNECT, () =>
            {
                frm.Invoke(new Action(() => frm.appendWebSockLogBox("Connected to " + address + "...\n")));
                socket.Emit("newAccessPoint", frm.getWebSocketPort());
            });

            socket.On("sayIP", (data) =>
            {
                frm.Invoke(new Action(() => frm.setIp(data.ToString())));
                frm.Invoke(new Action(() => frm.appendWebSockLogBox("Message: " + data + "\n")));
                
            });

        }

        public void disconnect()
        {
            frm.Invoke(new Action(() => frm.appendWebSockLogBox("Disconnected...\n")));
            socket.Disconnect();
        }
    }
}
