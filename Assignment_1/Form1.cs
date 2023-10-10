using System;
using System.Windows.Forms;
using ClassLibrary;
namespace Assignment_1
{
    public partial class Form1 : Form
    {
        public Client Publisher;
        public Form1()
        {
            InitializeComponent();
           
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string message = textBox1.Text;
            listBox1.Items.Add($"S: {message}");
            await Publisher.SendMessage(message);

        }

        //Connect to server/client
        private void correctToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Determine whether is gonna connect as a server or client | Client(str IpAddress) for Client
            Publisher = new Client(); // Server for now
            Publisher.MessageReceived += HandleMessageReceived;
            listBox1.Items.Add("Waiting for connection...");

        }
        public void HandleMessageReceived(string message)
        {
            // Do something with the received message
            this.Invoke(new MethodInvoker(delegate ()
            {
                listBox1.Items.Add(message);
            }));
        }

        //Disconnect the server
        private void disconnectToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Publisher.Close();
        }

        //Close the form
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}


    

