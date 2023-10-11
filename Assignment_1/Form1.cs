using System;
using System.Net;
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

        private void clientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string IPAddress = Prompt.ShowDialog("Enter IP Address", "IP Address");
            Publisher = new Client(IPAddress);
            Publisher.MessageReceived += HandleMessageReceived;
        }

        private void serverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Publisher = new Client();
            listBox1.Items.Add("Waiting for connection...");
            Publisher.MessageReceived += HandleMessageReceived;
        }

        public static class Prompt
        {
            public static string ShowDialog(string text, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 500,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };
                Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
                TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
                Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;
                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
            }
        }
    }
}


    

