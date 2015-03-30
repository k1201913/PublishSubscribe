using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Configuration;

namespace SocketPublisher
{
    public partial class Form1 : Form
    {
        Socket _client;
        EndPoint _remoteEndPoint;
        int _noOfEventsFired = 0;
        
        string _command = "Publish";
        string serverIP;

        public Form1()
        {
            InitializeComponent();
            txtEventData.Text = "Topic1 data";

            serverIP = ConfigurationSettings.AppSettings["ServerIP"];
            IPAddress serverIPAddress = IPAddress.Parse(serverIP);
            int serverPort = Convert.ToInt32(ConfigurationSettings.AppSettings["ServerPort"]);
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _remoteEndPoint = new IPEndPoint(serverIPAddress, serverPort);

            txtIp.Text = serverIP;
            txtTopicName.Text = "Topic1";
            txtEventCount.Text = "0";

        }

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show("FormClosing Event");
            _client.Close();
        }




        private void button3_Click(object sender, EventArgs e)
        {
            string topicName = txtTopicName.Text.Trim();
            if (string.IsNullOrEmpty(topicName))
            {
                MessageBox.Show("Please Enter a Topic Name");
                return;
            }
            if (CheckIp())
            {
                SendASingleEvent();
            }
        }

        private void SendASingleEvent()
        {
            String topicName = txtTopicName.Text;
            string eventData = txtEventData.Text;
            string date = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
            string message = _command + "," + date + "," + topicName + "," + eventData;
            _client.SendTo(Encoding.ASCII.GetBytes(message), _remoteEndPoint);
            _noOfEventsFired++;
            txtEventCount.Text = _noOfEventsFired.ToString();
        }

        private void tmrEvent_Tick(object sender, EventArgs e)
        {
            SendASingleEvent();
        }

        private void btnFireAutoStop_Click(object sender, EventArgs e)
        {
            if (tmrEvent.Enabled)
                tmrEvent.Enabled = false;
        }

        private void btnFireAuto_Click(object sender, EventArgs e)
        {
            string topicName = txtTopicName.Text.Trim();
            if (string.IsNullOrEmpty(topicName))
            {
                MessageBox.Show("Please Enter a Topic Name");
                return;
            }
            if (CheckIp())
            {
                int interval = 1000;
                tmrEvent.Interval = interval;
                tmrEvent.Enabled = true;
            }

        }

        private Boolean CheckIp()
        {
            try
            {
                if (serverIP != txtIp.Text)
                {
                    IPAddress serverIPAddress = IPAddress.Parse(txtIp.Text);
                    serverIP = txtIp.Text;
                    int serverPort = Convert.ToInt32(ConfigurationSettings.AppSettings["ServerPort"]);
                    _client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    _remoteEndPoint = new IPEndPoint(serverIPAddress, serverPort);

                }
                return true;
            }
            catch (Exception e1)
            {
                MessageBox.Show("Please Enter a valid IP");
                if (tmrEvent.Enabled)
                    tmrEvent.Enabled = false;
                return false;
            }


        }

    }
}
