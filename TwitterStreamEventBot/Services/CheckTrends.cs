using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using TwitterStreamEventBot.Controllers;
using TwitterStreamEventBot.Domain;
using TwitterStreamEventBot.Test;

namespace TwitterStreamEventBot.Service
{
    public static class CheckTrends
    {
        public static void Check()
        {
            var notificationController = new NotificationController();
            var url = "http://localhost:9000/";
            var bot = new ChannelAccount("56800324", "Bot1");

            List<ChannelAccount> userList;

            if (TrendingTopics.trendingList != null)
            {
                foreach (string t in TrendingTopics.trendingList)
                {
                    if (UserInfo.topicDict.TryGetValue(t, out userList))
                    {
                        foreach (ChannelAccount user in userList)
                        {
                            notificationController.SendMessage(url, bot, user, t);
                        }
                    }
                }
            }
        }
    }
}