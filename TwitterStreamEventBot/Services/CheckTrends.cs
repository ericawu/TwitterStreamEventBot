using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwitterStreamEventBot.Controllers;
using TwitterStreamEventBot.Domain;

namespace TwitterStreamEventBot.Service
{
    public static class CheckTrends
    {
        public static void Check2()
        {
            var notificationController = new NotificationController();
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
                                // notificationController.SendMessage(u.url, u.from, u.recipient, t);
                                sendMessage(u.url, u.from, u.recipient, t);
                            }
                        }
                        
                    }
                }
            }
        }

        private static async Task sendMessage(string url, ChannelAccount recipient, ChannelAccount from, string topic)
        {
            var connector = new ConnectorClient(new Uri(url));
            IMessageActivity newMessage = Activity.CreateMessageActivity();
            newMessage.Type = ActivityTypes.Message;
            newMessage.From = from;
            newMessage.Recipient = recipient;
            newMessage.Text = $"Hey, something interesting's happening with {topic}!";
            newMessage.Locale = "en-Us";
            if (url == "http://localhost:9000/")
            {
                newMessage.ChannelId = "emulator";
            }
            else
            {
                newMessage.ChannelId = "skype";
            }
            var conversation = await connector.Conversations.CreateDirectConversationAsync(recipient, from);

            newMessage.Conversation = new ConversationAccount(id: conversation.Id);

            await connector.Conversations.SendToConversationAsync((Activity)newMessage);
        }
    }
}