using System;
using System.Collections.Generic;
using System.Linq;
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

        public event EventHandler<MessageEventArgs> RaiseCustomEvent;

        private TcpListener server;
        private TcpClient client;
        private NetworkStream stream;


        
        public Client()
        {
            try
            {
                client = new TcpClient();
                client.Connect("127.0.0.1", 8888);
                stream = client.GetStream();

                Task.Run(() => ReceiveMessagesAsync());
            }
            catch (Exception e)
            {   
                string msg = e.Message;
                //CustomEvent?.Invoke(this, new MessageEventArgs(msg));
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
