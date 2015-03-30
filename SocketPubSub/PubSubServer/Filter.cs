using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace PubSubServer
{
    class Filter
    {
        static Dictionary<string, List<EndPoint>> _subscribersList = new Dictionary<string, List<EndPoint>>();

        static public Dictionary<string, List<EndPoint>> SubscribersList
        {
            get
            {
                lock (typeof(Filter))
                {
                    return _subscribersList;
                }
            }

        }


        static public List<EndPoint> GetSubscribers(String topicName)
        {
            lock (typeof(Filter))
            {
                if (SubscribersList.ContainsKey(topicName))
                {
                    CheckIfSubscriberLive(SubscribersList[topicName],topicName);
                    return SubscribersList[topicName];
                }
                else
                    return null;
            }
        }

        static public void AddSubscriber(String topicName, EndPoint subscriberEndPoint)
        {
            lock (typeof(Filter))
            {
                if (SubscribersList.ContainsKey(topicName))
                {
                    if (!SubscribersList[topicName].Contains(subscriberEndPoint))
                    {
                        SubscribersList[topicName].Add(subscriberEndPoint);
                    }
                }
                else
                {
                    List<EndPoint> newSubscribersList = new List<EndPoint>();
                    newSubscribersList.Add(subscriberEndPoint);
                    SubscribersList.Add(topicName, newSubscribersList);
                }
            }
        }


        static public void RemoveSubscriber(String topicName, EndPoint subscriberEndPoint)
        {
            lock (typeof(Filter))
            {
                if (SubscribersList.ContainsKey(topicName))
                {
                    if (SubscribersList[topicName].Contains(subscriberEndPoint))
                    {
                        SubscribersList[topicName].Remove(subscriberEndPoint);
                    }
                }
            }
        }


        private static void CheckIfSubscriberLive(List<EndPoint> list, String topicName)
        {
            foreach (EndPoint endpoint in list)
            {
                if (Refresh.SubscribersLive.ContainsKey(endpoint))
                {
                    int limit = Refresh.SubscribersLive[endpoint];
                    --limit;
                    System.Console.WriteLine("check " + endpoint.ToString() + " " + limit);
                    Refresh.SubscribersLive[endpoint] = limit;

                    if (limit == 0)
                    {
                        System.Console.WriteLine("removing " + endpoint.ToString());
                        SubscribersList[topicName].Remove(endpoint);
                        Refresh.RemoveSubscriber(endpoint);
                    }
                }
            } 
        }
    }
}
