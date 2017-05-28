using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    public partial class Form1 : Form
    {
        private Socket tSocket;
        private Socket socket;
        public Form1()
        {
            InitializeComponent();

            txtIP.Text = "127.0.0.1";
            txtPort.Text = "12345";

            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(txtIP.Text);
            IPEndPoint point = new IPEndPoint(ip, int.Parse(txtPort.Text));

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Bind(point);
                socket.Listen(10);
                recLV.Items.Add("Server starts listening..");
                Thread thread = new Thread(Receive);

                thread.IsBackground = true;
                thread.Start(socket);
            }catch(Exception ex)
            {
               // recLV.Items.Add("exception caught!" + ex.Message);
            }
        }

        private void Receive(object o)
        {
            Socket socket = o as Socket;

            while (true)
            {
                try
                {
                    tSocket = socket.Accept();
                    string point = tSocket.RemoteEndPoint.ToString();

                    recLV.Items.Add(point + "connected!");

                    Thread th = new Thread(ReceiveMsg);
                    th.IsBackground = true;
                    th.Start(tSocket);
                }catch(Exception ex)
                {
                    //recLV.Items.Add(ex.Message);
                    break;
                }
            }
        }

        private void ReceiveMsg(object o)
        {
            Socket client = o as Socket;

            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024 * 1024];
                    int n = client.Receive(buffer);

                    string words = Encoding.UTF8.GetString(buffer, 0, n);

                    recLV.Items.Add(client.RemoteEndPoint.ToString() + ":" + words);

                }
                catch (Exception ex)
                {
                   // recLV.Items.Add(ex.Message);
                    break;
                }
            }
        }


        private void IPtext_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                recLV.Items.Add("I:" + sendBox.Text);
                byte[] buffer = Encoding.UTF8.GetBytes(sendBox.Text);
                tSocket.Send(buffer);
            }catch(Exception ex){
                //recLV.Items.Add(ex.Message);
            }
        }
    }
}
