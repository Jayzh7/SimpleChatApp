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

namespace sClient
{
    public partial class Form1 : Form
    {
        Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        public Form1()
        {
            InitializeComponent();

            txtIP.Text = "127.0.0.1";
            txtPort.Text = "12345";

            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(txtIP.Text);
            IPEndPoint point = new IPEndPoint(ip, int.Parse(txtPort.Text));

            try
            {
                client.Connect(point);

                recLV.Items.Add("Connected!");
                recLV.Items.Add("Server" + client.LocalEndPoint.ToString());
                recLV.Items.Add("Client" + client.RemoteEndPoint.ToString());

                Thread th = new Thread(ReceiveMsg);
                th.IsBackground = true;
                th.Start();
            }catch(Exception ex)
            {
                recLV.Items.Add(ex.Message);
            }


        }

        private void ReceiveMsg()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024 * 1024];
                    int n = client.Receive(buffer);
                    string s = Encoding.UTF8.GetString(buffer, 0, n);
                    recLV.Items.Add(client.RemoteEndPoint.ToString() + ":" + s);

                }catch(Exception ex)
                {
                    // recLV.Items.Add(ex.Message);
                    break;
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if(client != null)
            {
                try
                {
                    recLV.Items.Add(sendBox.Text);
                    byte[] buffer = Encoding.UTF8.GetBytes(sendBox.Text);
                    client.Send(buffer);
                }catch(Exception ex)
                {
                    recLV.Items.Add(ex.Message);
                }
            }
        }
    }
}
