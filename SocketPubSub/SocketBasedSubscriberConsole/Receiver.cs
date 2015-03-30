using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Configuration;

namespace SocketBasedSubscriberConsole
{
    class Receiver
    {
        public Socket _client;
        public EndPoint _remoteEndPoint;

        int _recv;
        byte[] _data;
        int eventCount = 0;
        int refresh = 10;

        public void ReceiveDataFromServer()
        {
            _data = new byte[1024];
            EndPoint publisherEndPoint = _client.LocalEndPoint;
            while (true)
            {
                _recv = _client.ReceiveFrom(_data, ref publisherEndPoint);
                string msg = Encoding.ASCII.GetString(_data, 0, _recv) + "," + publisherEndPoint.ToString();

                string[] messageParts = msg.Split(",".ToCharArray());
                Console.WriteLine(++eventCount + " " + messageParts[0] + "," + messageParts[1] + "," + messageParts[2]);

                --refresh;
                if (refresh <= 2)
                {
                    refresh = 10; 
                    string Command = "Refresh";
                    string message = Command + "," + messageParts[1] + "," + refresh;
                    _client.SendTo(Encoding.ASCII.GetBytes(message), _remoteEndPoint);
                }
            }
        }

        ~Receiver()
        {
            _client.Close();
            System.Console.WriteLine("closing...");
        }
    }
}
