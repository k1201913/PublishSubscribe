using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PubSubServerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(
                "PubSubServer running on " + PublisherService.ReturnMachineIP().ToString()
                );
            HostPublishSubscribeServices();
            Console.ReadLine();
            Console.WriteLine("PubSubServer stopped");
        }

        private static void HostPublishSubscribeServices()
        {
            SubscriberService subscriberService = new SubscriberService();
            subscriberService.StartSubscriberService();

            PublisherService publisherService = new PublisherService();
            publisherService.StartPublisherService();
        }
    }
}
