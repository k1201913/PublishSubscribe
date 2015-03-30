using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Configuration;
using System.Threading;

namespace SocketBasedSubscriberConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket _client;
            EndPoint _remoteEndPoint;
            string topicName = "Topic1";
            int refresh = 10;
            
            if (args.Length == 2)
            {
                topicName = args[1];
            }

            string serverIP = ConfigurationSettings.AppSettings["ServerIP"];
            if (args.Length >= 1)
            {
                serverIP = args[0];
            } 
            
            IPAddress serverIPAddress = IPAddress.Parse(serverIP);
            int serverPort = Convert.ToInt32(ConfigurationSettings.AppSettings["ServerPort"]);

            Console.WriteLine("Subsriber Client starting " + serverIP + ":" + serverPort);
            
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _remoteEndPoint = new IPEndPoint(serverIPAddress, serverPort);

            string Command = "Subscribe";

            string message = Command + "," + topicName + "," + refresh;
            _client.SendTo(Encoding.ASCII.GetBytes(message), _remoteEndPoint);

            Receiver receiver = new Receiver();
            receiver._client = _client;
            receiver._remoteEndPoint = _remoteEndPoint;

            Thread thread1 = new Thread(new ThreadStart(receiver.ReceiveDataFromServer));
            thread1.IsBackground = true;
            thread1.Start();

            Console.WriteLine("Client listening, press Enter to stop");
            Console.ReadLine();

            string command = "UnSubscribe";

            message = command + "," + topicName;
            _client.SendTo(Encoding.ASCII.GetBytes(message), _remoteEndPoint);
            _client.Close();
            Console.WriteLine("Client closed");

        }
    }
}
