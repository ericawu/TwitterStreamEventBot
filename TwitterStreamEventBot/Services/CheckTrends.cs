using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using TwitterStreamEventBot.Controllers;
using TwitterStreamEventBot.Domain;

namespace TwitterStreamEventBot.Service
{
    public static class CheckTrends
    {
        /*
        public static void Check()
        {
            var notificationController = new NotificationController();
            var url = "http://localhost:9000/";
            var bot = new ChannelAccount("56800324", "Bot1");

            Dictionary<ChannelAccount, DateTime> userList;
            Dictionary<ChannelAccount, DateTime> userListNew;

            if (TrendingTopics.trendingList != null)
            {
                foreach (string t in TrendingTopics.trendingList)
                {
                    if (UserInfo.topicDict.TryGetValue(t, out userList))
                    {
                        userListNew = new Dictionary<ChannelAccount, DateTime>(userList);

                        foreach (KeyValuePair<ChannelAccount, DateTime> user in userList)
                        {
                            if (DateTime.Now > user.Value.AddHours(1))
                            {
                                userListNew[user.Key] = DateTime.Now;
                                notificationController.SendMessage(url, bot, user.Key, t);
                            }
                        }
                        UserInfo.topicDict[t] = userListNew;
                    }
                }
            }
        }
        */

        public static void Check2()
        {
            var notificationController = new NotificationController();
            var url = "https://twitterstreameventbot.azurewebsites.net/api/messages";
            Dictionary<BotUserChannel, DateTime> userList;
            Dictionary<BotUserChannel, DateTime> userListNew;

            if (TrendingTopics.trendingList != null)
            {
                foreach (string t in TrendingTopics.trendingList)
                {
                    if (UserInfo.topicDict2.TryGetValue(t, out userList))
                    {
                        userListNew = new Dictionary<BotUserChannel, DateTime>(userList);

                        foreach (KeyValuePair<BotUserChannel, DateTime> user in userList)
                        {
                            if (DateTime.Now > user.Value.AddHours(1))
                            {
                                userListNew[user.Key] = DateTime.Now;
                                UserInfo.topicDict2[t] = userListNew;

                                BotUserChannel u = user.Key;
                                notificationController.SendMessage(u.url, u.from, u.recipient, t);
                            }
                        }
                        
                    }
                }
            }
        }
    }
}