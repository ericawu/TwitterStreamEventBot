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

            Task.Run(() => startTweetStream("olympics"));
            Task.Run(() => startTimer());
        }

        private static void startTimer()
        {
            Timer t = new Timer();
            t.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            t.Interval = 15 * 1000;
            t.Enabled = true;

            while (true) { };
        }

        private static async void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            List<string> temp = new List<string>(groupedTweets);
            groupedTweets.Clear();
            count = 0;
            List<string> topics = await GetKeywords.MakeRequestAsync(temp);

            foreach (string t in temp)
            {
                Debug.WriteLine(t);
            }
            if (topics != null && topics.Count > 0)
            {
                countWords(topics);
                CheckTrends.Check();
                CheckTrends.Check2();
            }
        }

        private static void countWords(List<string> list)
        {
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

            TrendingTopics.trendingList = allKeys;

            Debug.WriteLine(TrendingTopics.trendingList);
            foreach (KeyValuePair<string, int> word in top)
            {
                Debug.WriteLine(word.Key + " " + word.Value);
            }
        }

        private static void startTweetStream(string searchString)
        {
            stream.AddTrack(searchString);
            stream.MatchingTweetReceived += (sender, arg) =>
            {
                try
                {
                    var jsonData = JsonConvert.SerializeObject(arg.Tweet.TweetDTO);

                    groupedTweets.Add(arg.Tweet.Text.ToLower());
                    count++;
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
        }

    }
}