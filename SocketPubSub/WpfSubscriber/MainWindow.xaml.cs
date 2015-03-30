using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Configuration;
using System.Collections;

namespace WpfSubscriber
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Socket _client;
        EndPoint _remoteEndPoint;
        byte[] _data;
        int _recv;
        Boolean _isReceivingStarted = false;
        int refresh = 0;
        string serverIP;

        public class EventItem
        {
            public string Date { get; set; }
            public string Topic { get; set; }
            public string Data { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            serverIP = ConfigurationSettings.AppSettings["ServerIP"];
            IPAddress serverIPAddress = IPAddress.Parse(serverIP);
            int serverPort = Convert.ToInt32(ConfigurationSettings.AppSettings["ServerPort"]);
            try
            {
                _client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _remoteEndPoint = new IPEndPoint(serverIPAddress, serverPort);
            }
            catch (Exception e1)
            {
                MessageBox.Show("Server not available");
            }
            txtIp.Text = serverIP;
            txtTopicName.Text = "Topic1";
        }
 

        private void WindowClosing()
        {
            //MessageBox.Show("FormClosing Event");
            _client.Close();
        }



        void ReceiveDataFromServer()
        {
            EndPoint publisherEndPoint = _client.LocalEndPoint;
            while (true)
            {
                try
                {
                    _recv = _client.ReceiveFrom(_data, ref publisherEndPoint);
                    string msg = Encoding.ASCII.GetString(_data, 0, _recv) + "," + publisherEndPoint.ToString();
                    AddToListBox(msg);
                    --refresh;
                    if (refresh <= 0)
                    {
                        String refreshLimit = ConfigurationSettings.AppSettings["RefreshLimit"];
                        string[] messageParts = msg.Split(",".ToCharArray());
                        string Command = "Refresh";
                        string message = Command + "," + messageParts[1] + "," + refreshLimit;
                        _client.SendTo(Encoding.ASCII.GetBytes(message), _remoteEndPoint);
                        refresh = RefreshLimit();
                    }

                }
                catch (Exception e1)
                {
                    MessageBox.Show("Connection failure");
                } 
            }
        }

        public void AddToListBox(string message)
        {
            string[] messageParts = message.Split(",".ToCharArray());
            EventItem item = new EventItem();
            item.Date = messageParts[0];
            item.Topic = messageParts[1];
            item.Data = messageParts[2];

            Dispatcher.Invoke(() =>
            {
                lstEvents.Items.Add(item);
                if (lstEvents.Items.Count != 0)
                {
                    lstEvents.ScrollIntoView(
                        lstEvents.Items[lstEvents.Items.Count - 1]);
                }
            });
            
        }


        private void btnClearListView_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                lstEvents.Items.Clear();
            }); 
        }

        private void unSubcribe_Click(object sender, RoutedEventArgs e)
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
                unSubcribe.Visibility = Visibility.Collapsed;
                subcribe.Visibility = Visibility.Visible;
            }
            catch
            {

            }

        }

        private void subcribe_Click(object sender, RoutedEventArgs e)
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

            subcribe.Visibility = Visibility.Collapsed;
            unSubcribe.Visibility = Visibility.Visible;
            Dispatcher.Invoke(() =>
            {
                lstEvents.Items.Clear();
            }); 
            
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

        private void buttonClearHistory_Click(object sender, RoutedEventArgs e)
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
