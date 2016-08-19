using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Tweetinvi.Streaming;
using Tweetinvi;
using Tweetinvi.Models;
using TwitterStreamEventBot.Resource;
using System.Diagnostics;
using TwitterStreamEventBot.Domain;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace TwitterStreamEventBot
{

    [Serializable]
    [LuisModel(Constants.LUISAppId, Constants.LUISSubscriptionId)]
    public class EventBotDialogue : LuisDialog<object>
    {

        [LuisIntent("subscribe")]
        public async Task Subscribe(IDialogContext context, LuisResult result)
        {
            var m = context.MakeMessage();
            var from = m.From;
            ChannelAccount recipient = m.Recipient;
            string serviceurl = m.ServiceUrl;
            

            foreach (EntityRecommendation e in result.Entities)
            {
                string entity = e.Entity;

                Dictionary<ChannelAccount, DateTime> userList;
                Dictionary<BotUserChannel, DateTime> userList2;
                BotUserChannel newChannel = new BotUserChannel();
                newChannel.recipient = recipient;
                newChannel.from = from;
                newChannel.url = serviceurl;

                if (!UserInfo.topicDict.TryGetValue(entity, out userList))
                {
                    UserInfo.topicDict.Add(entity, new Dictionary<ChannelAccount, DateTime>() { { recipient, DateTime.Now.AddHours(-2) } });
                    UserInfo.topicDict2.Add(entity, new Dictionary<BotUserChannel, DateTime>() { { newChannel, DateTime.Now.AddHours(-2) } });
                    await context.PostAsync($"You are now following the topic {entity}");
                }
                else if (userList.Any(user => user.Key.Id == recipient.Id))
                {
                    await context.PostAsync($"I gotchu, you're already following the topic {entity}");
                }
                else
                {
                    userList.Add(recipient, DateTime.Now.AddHours(-2));
                    UserInfo.topicDict[entity] = userList;
                    userList2 = UserInfo.topicDict2[entity];
                    userList2.Add(newChannel, DateTime.Now.AddHours(-2));
                    UserInfo.topicDict2[entity] = userList2;
                    await context.PostAsync($"You are now following the topic {entity}");
                }

            }
            context.Done(0);
        }

        [LuisIntent("Unsubscribe")]
        public async Task Unsubscribe(IDialogContext context, LuisResult result)
        {
            var m = context.MakeMessage();
            var from = m.From;
            ChannelAccount recipient = m.Recipient;
            foreach (EntityRecommendation e in result.Entities)
            {
                string entity = e.Entity;
                Dictionary<ChannelAccount, DateTime> userList;

                BotUserChannel newChannel = new BotUserChannel();
                newChannel.recipient = recipient;
                newChannel.from = from;

                if (!UserInfo.topicDict.TryGetValue(entity, out userList))
                {
                    context.PostAsync($"You aren't subscribed to {entity} yet.");
                }
                else if (userList.Any(user => user.Key.Id == recipient.Id))
                {
                    var item = userList.First(u => u.Key.Id == recipient.Id).Key;
                    userList.Remove(item);
                    
                    UserInfo.topicDict[entity] = userList;

                    Dictionary<BotUserChannel, DateTime> userList2 = UserInfo.topicDict2[entity];

                    var item2 = userList2.First(u => u.Key.recipient.Id == newChannel.recipient.Id).Key;
                    userList2.Remove(item2);
                    UserInfo.topicDict2[entity] = userList2;
                    context.PostAsync($"You are no longer susbscribed to {entity}.");
                }
                else
                {
                    context.PostAsync($"You aren't subscribed to this {entity} yet.");
                }

            }
            context.Done(0);

        }

        [LuisIntent("ListTrending")]
        public async Task ListTrending(IDialogContext context, LuisResult result)
        {   
            if (TrendingTopics.trendingList == null)
            {
                await context.PostAsync("Still compiling the list, please ask again in 30 seconds!");
            }
            else
            {
                await context.PostAsync("The current list of trending topics is: " + string.Join(", ", TrendingTopics.trendingList));
            }
            context.Done(0);
        }

        [LuisIntent("None")]
        public async Task NoIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry, I'm not sure I understand.");
            context.Done(0);
        }

    }
}