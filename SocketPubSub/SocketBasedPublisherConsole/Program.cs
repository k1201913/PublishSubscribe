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

    class Program
    {
        static void Main(string[] args)
        {
            string topicName = "Topic1";
            string eventData = "data for topic1";
            
            if (args.Length == 3)
            {
                topicName = args[1];
                eventData = args[2];
            }
            
            string serverIP = ConfigurationSettings.AppSettings["ServerIP"];
            if (args.Length >= 1)
            {
                serverIP = args[0];
            }

            string _command = "Publish";

            IPAddress serverIPAddress = IPAddress.Parse(serverIP);
            int serverPort = Convert.ToInt32(ConfigurationSettings.AppSettings["ServerPort"]);

            Console.WriteLine("Publishing one event " + serverIP + ":" + serverPort);

            Socket _client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            EndPoint _remoteEndPoint = new IPEndPoint(serverIPAddress, serverPort);

            string date = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
            string message = _command + "," + date + "," + topicName + "," + eventData;
            int result = _client.SendTo(Encoding.ASCII.GetBytes(message), _remoteEndPoint);
            Console.WriteLine("Event send " + "0" + " " + message + " " + result);

            _client.Close();


            Console.WriteLine("Publisher file reader starting");
            
            Publisher publisher = new Publisher(serverIP, topicName);
            Thread thread1 = new Thread(new ThreadStart(publisher.SendFromFile));
            thread1.IsBackground = true;
            thread1.Start();

            Console.WriteLine("Client waiting events, press Enter to stop");
            Console.ReadLine();
            System.Console.WriteLine("stopping ...");

        }
    }


}
