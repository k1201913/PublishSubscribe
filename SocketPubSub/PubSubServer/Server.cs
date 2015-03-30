using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PubSubServer
{
    public partial class Server : Form
    {

        public Server()
        {
            InitializeComponent();
            HostPublishSubscribeServices();
            ip.Text = PublisherService.ReturnMachineIP().ToString();
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
