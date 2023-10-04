using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{   

    public class Server
    {
        public delegate string MyDelegate(string message);
        public event MyDelegate MyEvent;

        private TcpListener server;
        private TcpClient client;
        private NetworkStream stream;

        public Server()
        {
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            server = new TcpListener(localAddr, 8888);
            server.Start();
        }

        public async Task StartServerAsync()
        {
            while (true)
            {
                client = await server.AcceptTcpClientAsync();
                HandleClient(client);
            }
        }
        public async Task HandleClient(TcpClient client)
        {
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
}
