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

        public delegate void MessageReceivedEventHandler(string message);
        public event MessageReceivedEventHandler MessageReceived;
        private TcpClient client = null;
        private readonly TcpListener server = null;
        private NetworkStream stream;
        private readonly int port = 8888;

        public Client(string IPAdress)
        {   
            // Acting as a Client
            try
            {   
                client = new TcpClient(IPAdress, port);
                stream = client.GetStream();
                Task.Run(() => ReceiveMessagesAsync());
            }
            catch (Exception)
            {

            }
        }

        public Client()
        {
            // Acting as a server
            IPEndPoint localIPAddress = new IPEndPoint(IPAddress.Any, port);
            server = new TcpListener(localIPAddress);
            server.Start();
            Task.Run(() => StartServerAsync());
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
                        OnMessageReceived($"R: {message}");
                    }
                }
                catch (Exception)
                {
                    OnMessageReceived("Error");
                }
            }
        }

        public async Task ReceiveMessagesAsync()
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            try
            {
                await SendMessage("Server Connected to Client");
                clientConnected();
                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    OnMessageReceived($"R: {message}");

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task SendMessage(string message)
        {

            if (!string.IsNullOrWhiteSpace(message))
            {
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        protected virtual void OnMessageReceived(string msg)
        {
            if (MessageReceived != null)
            {
                MessageReceived(msg);
            }
            //MessageReceived.Invoke(msg);
        }

        public void Close()
        {
            if (client != null) { client.Close(); }
            if (server != null) { server.Stop(); }
            if (stream != null) { stream.Close(); }
                
        }
        public void clientConnected() {
            if (client.Connected ) {
                OnMessageReceived("Client is Connect to Server");
            }
        }
    }
}
