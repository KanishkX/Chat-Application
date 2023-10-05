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
            await Publisher.SendMessage(message);

        }
        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {   
            this.Close();
        }

        private void correctToolStripMenuItem_Click(object sender, EventArgs e)
        {
             Publisher = new Client();
            Publisher.ReceiveMessagesAsync();
            Publisher.RaiseCustomEvent += HandleMessageReceived;
        }
        private void HandleMessageReceived(object sender, MessageEventArgs e)
        {
            string receivedMessage = e.Message;
            // Do something with the received message
            listBox1.Items.Add(receivedMessage);
        }

    }
}


    

