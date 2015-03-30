using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.IO;

namespace PubSubServer
{
    class HistoryFile
    {
        public static void ReadHistory(string topicName, EndPoint subscriberForThisTopic, Socket server)
        {
            string folder = ConfigurationSettings.AppSettings["Folder"];
            List<EndPoint> subscriberListForThisTopic = new List<EndPoint>();
            subscriberListForThisTopic.Add(subscriberForThisTopic);
            try
            {
            using (StreamReader r = File.OpenText(folder + topicName + ".txt"))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    WorkerThreadParameters workerThreadParameters = new WorkerThreadParameters();
                    workerThreadParameters.Server = server;
                    workerThreadParameters.Message = line;
                    workerThreadParameters.SubscriberListForThisTopic = subscriberListForThisTopic;

                    ThreadPool.QueueUserWorkItem(
                        new WaitCallback(PublisherService.Publish), workerThreadParameters);
                }
            }
            }
            catch (FileNotFoundException e)
            {}
        }

        public static void CleanHistory(string topicName)
        {
            string folder = ConfigurationSettings.AppSettings["Folder"];

            string filename = folder + topicName + ".txt";
            if (File.Exists(@filename))
            {
                File.Delete(@filename);
            }
        }

        public static void UpdateHistory(string message)
        {
            string folder = ConfigurationSettings.AppSettings["Folder"];
            string[] messageParts = message.Split(",".ToCharArray());
            String topicName = messageParts[1];

            using (StreamWriter w = File.AppendText(folder + topicName + ".txt"))
            {
                w.WriteLine("{0}", message);
            }
        }
    }
}
