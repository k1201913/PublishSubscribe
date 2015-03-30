using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace PubSubServerConsole
{
    class Refresh
    {
        static Dictionary<EndPoint, int> _subscribersLive = new Dictionary<EndPoint, int>();


        static public Dictionary<EndPoint, int> SubscribersLive
        {
            get
            {
                lock (typeof(Refresh))
                {
                    return _subscribersLive;
                }
            }

        }


        static public void AddSubscriber(EndPoint subscriberEndPoint, String refresh )
        {
            lock (typeof(Refresh))
            {
                int limit = Convert.ToInt32(refresh);
                if (!SubscribersLive.ContainsKey(subscriberEndPoint))
                {
                    SubscribersLive.Add(subscriberEndPoint, limit);
                }
                else
                {
                    RefreshSubscriber(refresh, subscriberEndPoint);
                }
            }


        }

        static public void RemoveSubscriber(EndPoint subscriberEndPoint)
        {
            lock (typeof(Refresh))
            {
                if (SubscribersLive.ContainsKey(subscriberEndPoint))
                {
                    SubscribersLive.Remove(subscriberEndPoint);
                }
            }

        }


        static public void RefreshSubscriber(String refresh, EndPoint subscriberEndPoint)
        {
            lock (typeof(Refresh))
            {
                int limit = Convert.ToInt32(refresh);
                if (SubscribersLive.ContainsKey(subscriberEndPoint))
                {
                    SubscribersLive[subscriberEndPoint] = limit;
                }
            }

        }


        private static void CheckIfSubscriberLive(List<EndPoint> list, String topicName)
        {
            foreach (EndPoint endpoint in list)
            {
                if (SubscribersLive.ContainsKey(endpoint))
                {
                    int limit = SubscribersLive[endpoint];
                    --limit;
                    System.Console.WriteLine("check " + endpoint.ToString() + " " + limit);
                    SubscribersLive[endpoint] = limit;

                    if (limit <= 0)
                    {
                        System.Console.WriteLine("removing " + endpoint.ToString());
                        Filter.RemoveSubscriber(topicName, endpoint);
                        SubscribersLive.Remove(endpoint);
                    }
                }
            } 
        }
    }
}
