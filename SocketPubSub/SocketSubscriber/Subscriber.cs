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
using System.Collections;

namespace SocketSubscriber
{
    public partial class Subscriber : Form
    {
        Socket _client;
        EndPoint _remoteEndPoint;
        byte[] _data;
        int _recv;
        Boolean _isReceivingStarted = false;
        int refresh = 0;
        string serverIP;

        public Subscriber()
        {
            InitializeComponent();

            serverIP = ConfigurationSettings.AppSettings["ServerIP"];
            IPAddress serverIPAddress = IPAddress.Parse(serverIP);
            int serverPort = Convert.ToInt32(ConfigurationSettings.AppSettings["ServerPort"]);

            _client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _remoteEndPoint = new IPEndPoint(serverIPAddress, serverPort);

            txtIp.Text = serverIP;
            txtTopicName.Text = "Topic1";

        }

        private void Subscriber_FormClosing(Object sender, FormClosingEventArgs e)
        {
            //MessageBox.Show("FormClosing Event");
            _client.Close();
        }



        void ReceiveDataFromServer()
        {
            EndPoint publisherEndPoint = _client.LocalEndPoint;
            while (true)
            {
                _recv = _client.ReceiveFrom(_data, ref publisherEndPoint);
                string msg = Encoding.ASCII.GetString(_data, 0, _recv) + "," + publisherEndPoint.ToString();
                lstEvents.Invoke(new AddToTextBoxDelegate(AddToTextBox), msg);
                --refresh;
                if (refresh <=0)
                {
                    String refreshLimit = ConfigurationSettings.AppSettings["RefreshLimit"];
                    string[] messageParts = msg.Split(",".ToCharArray());
                    string Command = "Refresh";
                    string message = Command + "," + messageParts[1] + "," + refreshLimit;
                    _client.SendTo(Encoding.ASCII.GetBytes(message), _remoteEndPoint);
                    refresh = RefreshLimit();
                }

            }
        }

        public delegate void AddToTextBoxDelegate(string message);
        public void AddToTextBox(string message)
        {
            string[] messageParts = message.Split(",".ToCharArray());
            int itemNum = (lstEvents.Items.Count < 1) ? 0 : lstEvents.Items.Count;
            lstEvents.Items.Add(messageParts[0]);
            lstEvents.Items[itemNum].SubItems.AddRange(new string[] { messageParts[1], CombineData(messageParts) });
        }
        private string CombineData(string[] messageParts)
        {
            string data = messageParts[2];
            for (int i = 3; i < messageParts.Length-1; i++)
            {
                data += ",";
                data += messageParts[i];
            }
            return data;
        }

        private void btnClearAstaListView_Click(object sender, EventArgs e)
        {
            lstEvents.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string topicName = txtTopicName.Text.Trim();
                if (string.IsNullOrEmpty(topicName))
                {
                    MessageBox.Show("Please Enter a Topic Name");
                    return;
                }
                string command = "UnSubscribe";

                string message = command + "," + topicName;
                _client.SendTo(Encoding.ASCII.GetBytes(message), _remoteEndPoint);
                ((Button)sender).Visible = false;
                button3.Visible = true;
            }
            catch
            {

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string topicName = txtTopicName.Text.Trim();
            if (string.IsNullOrEmpty(topicName))
            {
                MessageBox.Show("Please Enter a Topic Name");
                return;
            }

            if (!CheckIp())
            {
                return; // invalid IP
            }

            ((Button)sender).Visible = false;
            button2.Visible = true;
            lstEvents.Items.Clear();

            String refreshLimit = ConfigurationSettings.AppSettings["RefreshLimit"];
            string Command = "Subscribe";

            string message = Command + "," + topicName + "," + refreshLimit;

            _client.SendTo(Encoding.ASCII.GetBytes(message), _remoteEndPoint);

            refresh = RefreshLimit();

            if (_isReceivingStarted == false)
            {
                _isReceivingStarted = true;
                _data = new byte[1024];
                Thread thread1 = new Thread(new ThreadStart(ReceiveDataFromServer));
                thread1.IsBackground = true;
                thread1.Start();
            }
        }

        private int RefreshLimit()
        {
            return (Convert.ToInt32(ConfigurationSettings.AppSettings["RefreshLimit"])
                - Convert.ToInt32(ConfigurationSettings.AppSettings["RefreshTrigger"]));
        }

        private void buttonClearHistory_Click(object sender, EventArgs e)
        {
            string topicName = txtTopicName.Text.Trim();
            if (string.IsNullOrEmpty(topicName))
            {
                MessageBox.Show("Please Enter a Topic Name");
                return;
            }

            string Command = "Clear";

            string message = Command + "," + topicName;
            _client.SendTo(Encoding.ASCII.GetBytes(message), _remoteEndPoint);

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
                return false;
            }
        }


    }


}
