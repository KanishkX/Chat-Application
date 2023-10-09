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
        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {   
            this.Close();
        }

        private void correctToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Use paramaters defined in new revision of Client constructor
            Publisher = new Client("127.0.0.1");
            Publisher.MessageReceived += HandleMessageReceived;
        }
        public void HandleMessageReceived(string message)
        {
            // Do something with the received message
            this.Invoke(new MethodInvoker(delegate ()
            {
                listBox1.Items.Add(message);
                }));
        }

        private void disconnectToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}


    

