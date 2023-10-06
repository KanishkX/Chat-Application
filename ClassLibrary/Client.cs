using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static ClassLibrary.Client;

namespace ClassLibrary
{
    public class MessageEventArgs : EventArgs
    {
        public string Message { get; set; }

        public MessageEventArgs(string message)
        {
            Message = message;
        }
    }

    public class Client
    {

        public delegate string MyDelegate(string message);
        public event MyDelegate MyEvent;
        public event EventHandler<MessageEventArgs> RaiseCustomEvent;
        private TcpClient client = new TcpClient();
        private TcpListener server;
        private NetworkStream stream;
        private readonly int port = 8888;

        public Client(string IPAdress)
        {
            try
            {

                client.Connect(IPAdress, port);
                stream = client.GetStream();

                Task.Run(() => ReceiveMessagesAsync());
            }
            catch (Exception e)
            {
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, port);
                server.Start();
                Task.Run(() => StartServerAsync());
                string msg = e.Message;
            }
        }

        public async Task StartServerAsync()
        {
            while (true)
            {
                client = await server.AcceptTcpClientAsync();
                stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                try
                {
                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        //AppendMessage($"Received: {message}");
                        MyEvent.Invoke($"Received: {message}");
                    }
                }
                catch (Exception ex)
                {
                    MyEvent.Invoke("Error");
                }
            }
        }

        public async Task ReceiveMessagesAsync()
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            try
            {
                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    OnMessageReceived(new MessageEventArgs($"Message Recieved:{message}"));

                }
            }
            catch (Exception e)
            {
                OnMessageReceived(new MessageEventArgs("Exception"));
            }
        }

        public async Task SendMessage(string message)
        {

            if (!string.IsNullOrWhiteSpace(message))
            {
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
            //else
            //{
            //    OnMessageReceived("Empty");
            //}
        }

        protected virtual void OnMessageReceived(MessageEventArgs e)
        {
            EventHandler<MessageEventArgs> raiseEvent = RaiseCustomEvent;
            if (raiseEvent != null)
            {

                raiseEvent(this, e);
            }
        }
    }
}
