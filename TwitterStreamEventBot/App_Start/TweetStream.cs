using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using TwitterStreamEventBot.Resource;
using Tweetinvi;
using Tweetinvi.Models;
using Microsoft.ServiceBus.Messaging;
using Tweetinvi.Streaming;
using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using TwitterStreamEventBot.Domain;
using TwitterStreamEventBot.Service;
using System.Web.Http;
using TwitterStreamEventBot.Services;
using TwitterStreamEventBot.Controllers;
using Microsoft.Bot.Connector;

namespace TwitterStreamEventBot.App_Start
{
    public static class TweetStream
    {
        private static IFilteredStream stream;
        private static EventHubClient eventHubClient = EventHubClient.CreateFromConnectionString(Constants.ConnectionString, Constants.EventHubName);
        private static List<string> groupedTweets = new List<string>();

        private static List<string> ignoredWords = new List<string>() { "olympic", "olympics", "rt", "rio", "amp" };
        private static int count = 0;
        public static void initializeStream()
        {
            Auth.ApplicationCredentials = new TwitterCredentials(Constants.TConsumerKey, Constants.TConsumerKeySecret, Constants.TAccessToken, Constants.TAcessTokenSecret);
            stream = Stream.CreateFilteredStream();
            Debug.WriteLine("***** INITIALIZED TWEETSTREAM (should happen only once) *****");
            //var x = @HttpContext.Current.Application["ApplicationTest"];
            //Debug.WriteLine(x);
            //Debug.WriteLine($"{0}", UserInfo.topics[0]);
            Task.Run(() => startTweetStream("olympics"));
            Task.Run(() => startTimer());
        }

        private static void startTimer()
        {

            //Debug.WriteLine("start startTimer");
            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            t.Interval = 15 * 1000;
            t.Enabled = true;

            //Debug.WriteLine("end startTimer");
            while (true) { };
        }

        private static async void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            //Debug.WriteLine("start ontimedevent");
            List<string> temp = new List<string>(groupedTweets);
            groupedTweets.Clear();
            count = 0;
            List<string> topics = await GetKeywords.MakeRequestAsync(temp);

            foreach (string t in temp)
            {
                //var keywords = Task.Run(() => GetKeywords.MakeRequest(t));
                Debug.WriteLine(t);
            }
            if (topics != null && topics.Count > 0)
            {
                countWords(topics);
                //outreach();
                CheckTrends.Check();
                CheckTrends.Check2();
            }
            //Debug.WriteLine("end ontimedevent");
        }

        private static void outreach()
        {            //change
            var notificationController = new NotificationController();
            var url = "http://localhost:9000/";
            var recipient = new ChannelAccount("56800324", "Bot1");
            var from = new ChannelAccount("2c1c7fa4", "User2");
            notificationController.SendMessage(url, recipient, from, "outreach");
            var from2 = new ChannelAccount("2c1c7fa5", "User3");
            notificationController.SendMessage(url, recipient, from2, "outreach");
        }

        private static void countWords(List<string> list)
        {
            //Debug.WriteLine("start countwords");
            Dictionary<string, int> dict = new Dictionary<string, int>();
            foreach (string s in list)
            {
                string lower = s.ToLower();
                string[] words = lower.Split(' ');
                foreach (string w in words)
                {
                    if (!ignoredWords.Contains(w))
                    {
                        if (dict.ContainsKey(w))
                        {
                            dict[w]++;
                        }
                        else
                        {
                            dict.Add(w, 1);
                        }
                    }
                }
            }

            //Grab top x topics
            var top = dict.OrderByDescending(x => x.Value).Take(15);

            var z = dict.OrderByDescending(x => x.Value).Take(15);
            List<string> allKeys = (from key in z select key.Key).ToList();

            //var l = dict.Keys.ToList();
            TrendingTopics.trendingList = allKeys;

            Debug.WriteLine(TrendingTopics.trendingList);
            foreach (KeyValuePair<string, int> word in top)
            {
                Debug.WriteLine(word.Key + " " + word.Value);
            }
            //Debug.WriteLine("end countwords");
        }

        private static void startTweetStream(string searchString)
        {
            //Debug.WriteLine("start TweetStream");
            stream.AddTrack(searchString);
            stream.MatchingTweetReceived += (sender, arg) =>
            {
                try
                {

                    var jsonData = JsonConvert.SerializeObject(arg.Tweet.TweetDTO);
                    //Debug.WriteLine(arg.Tweet.Text);

                    //  if (count < 150)
                    // {
                    groupedTweets.Add(arg.Tweet.Text.ToLower());
                    count++;
                    //  }


                }

                catch (Exception exception)
                {
                    Debug.WriteLine("# CAUGHT AN EXCEPTION" + searchString);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0} > Exception: {1}", DateTime.Now, exception.Message);
                    Console.ResetColor();
                }
            };

            //Prevent crashing at unexpected EOF or 0 bytes
            stream.StreamStopped += (sender, args) =>
            {
                Debug.WriteLine("# STREAM STOPPED");
                if (args.Exception != null)
                {
                    stream.StartStreamMatchingAllConditions();
                }
            };
            stream.StartStreamMatchingAllConditions();
            // Debug.WriteLine("end tweetstream");
        }

    }
}