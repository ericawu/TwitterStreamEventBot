using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Tweetinvi.Streaming;
using TwitterStreamEventBot.Resource;

namespace TwitterStreamEventBot.App_Start
{
    public class TwitterConfig
    {
        /*
        private IFilteredStream stream;
        private EventHubClient eventHubClient = EventHubClient.CreateFromConnectionString(Constants.ConnectionString, Constants.EventHubName);
        private List<string> groupedTweets = new List<string>();
        */

        public static void StartStream()
        {
            Task.Run(() => testMethod());            
        }        

        private static void testMethod()
        {
            while (true)
            {
                Debug.WriteLine("test");
                Thread.Sleep(1000);
            }
        }
    }
}