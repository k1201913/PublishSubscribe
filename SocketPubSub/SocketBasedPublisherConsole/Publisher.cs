using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Configuration;
using System.IO;
using System.Threading;

namespace SocketBasedPublisherConsole
{
    class Publisher
    {
        static Socket _client;
        EndPoint _remoteEndPoint;
        string _serverIP;
        string _topicName;
        int eventCount = 0;
            
        public Publisher(string serverIP, string topicName)
        {
            this._serverIP = serverIP;
            this._topicName = topicName;
            if (serverIP == "")
            {
                serverIP =ConfigurationSettings.AppSettings["ServerIP"];
            }

            IPAddress serverIPAddress = IPAddress.Parse(serverIP);
            int serverPort = Convert.ToInt32(ConfigurationSettings.AppSettings["ServerPort"]);

            Console.WriteLine("Publisher Client starting " + serverIP + ":" + serverPort);

            _client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _remoteEndPoint = new IPEndPoint(serverIPAddress, serverPort);
        }

        public void SendFromFile()
        {
            string _command = "Publish";
            string eventData;
            string topicName = _topicName;
            string date;
            string message;
            
            while (true)
            {
                if (File.Exists(@"exit.txt"))
                {
                    File.Delete(@"exit.txt");
                    break;
                }
                if (File.Exists(@"event.txt"))
                {
                    try
                    {
                        using (StreamReader r = File.OpenText(@"event.txt"))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                date = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
                                message = _command + "," + date + "," + topicName + "," + line;
                                _client.SendTo(Encoding.ASCII.GetBytes(message), _remoteEndPoint);
                                ++eventCount;

                                Console.WriteLine("Events send " + eventCount + " " + message);
                            }
                        }
                    }
                    catch (FileNotFoundException e)
                    { }
                    File.Delete(@"event.txt");
                }
                Thread.Sleep(100);
            }
            _client.Close();
        }

        ~Publisher()
        {
            _client.Close();
            System.Console.WriteLine("closing...");
        }
    }
}
