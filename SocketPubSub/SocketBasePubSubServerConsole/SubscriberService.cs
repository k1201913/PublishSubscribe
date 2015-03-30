using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Configuration;
using System.IO;

namespace PubSubServerConsole
{
    class SubscriberService
    {
        static Socket server;

        public void StartSubscriberService()
        {
            Thread th = new Thread(new ThreadStart(HostSubscriberService));
            th.IsBackground = true;
            th.Start();
        }

        ~SubscriberService()
        {
            //System.Console.WriteLine("Closing SubscriberService");
            server.Close();
        }

        private void HostSubscriberService()
        {
            //IPAddress ipV4 = IPAddress.Parse("127.0.0.1");// ReturnMachineIP(); if you need machine ip then use this method.The method is available in PublishService.cs            
            IPAddress ipV4 = PublisherService.ReturnMachineIP();

            IPEndPoint localEP = new IPEndPoint(ipV4, 10001);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.Bind(localEP);

            StartListening(server);
        }

        private static void StartListening(Socket server)
        {
            EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            int recv = 0;
            byte[] data = new byte[1024];
            while (true)
            {

                recv = 0;
                data = new byte[1024];
                recv = server.ReceiveFrom(data, ref remoteEP);
                string messageSendFromClient = Encoding.ASCII.GetString(data, 0, recv);
                string[] messageParts = messageSendFromClient.Split(",".ToCharArray());

                if (!string.IsNullOrEmpty(messageParts[0]))
                {
                    System.Console.WriteLine(messageParts[0] + " " + messageParts[1]);
                    switch (messageParts[0])
                    {
                        case "Subscribe":

                            if (!string.IsNullOrEmpty(messageParts[1]))
                            {
                                if (messageParts.Length >2)
                                {
                                    Filter.AddSubscriber(messageParts[1], remoteEP);
                                    Refresh.AddSubscriber(remoteEP, messageParts[2]);
                                }
                                else
                                {
                                    Filter.AddSubscriber(messageParts[1], remoteEP);
                                }
                            }
                            HistoryFile.ReadHistory(messageParts[1], remoteEP, server);

                            break;
                        case "UnSubscribe":

                            if (!string.IsNullOrEmpty(messageParts[1]))
                            {
                                Filter.RemoveSubscriber(messageParts[1], remoteEP);
                                Refresh.RemoveSubscriber(remoteEP);
                            }
                            break;

                        case "Clear":
                            if (!string.IsNullOrEmpty(messageParts[1]))
                            {
                                HistoryFile.CleanHistory(messageParts[1]);
                            }
                            break;
                        case "Refresh":

                            if (!string.IsNullOrEmpty(messageParts[1]) &&
                                !string.IsNullOrEmpty(messageParts[2]))
                            {
                                Refresh.RefreshSubscriber(messageParts[2], remoteEP);
                            }

                            break;

                    }
                }

            }
        }

    }
}
