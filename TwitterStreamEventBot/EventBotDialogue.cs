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
using TwitterStreamEventBot.DBManager;
using TwitterStreamEventBot.Domain;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace TwitterStreamEventBot
{
    
    [Serializable]
    [LuisModel(Constants.LUISAppId, Constants.LUISSubscriptionId)]
    public class EventBotDialogue : LuisDialog<object>
    {

        //[NonSerialized]
        //private ChannelAccount recipient;
        //public string test;

        //public EventBotDialogue(string r)
        //{
            //this.recipient = r;
          //  test = r;
       // }
        [LuisIntent("subscribe")]
        public async Task Subscribe(IDialogContext context, LuisResult result)
        {
           
            var m = context.MakeMessage();
            var from = m.From;
            ChannelAccount recipient = m.Recipient;
            string serviceurl = m.ServiceUrl;
            //TODO: space inefficiency
            //TODO: dialog chains for already followed topics? ask if they want to unsubscribe?
            foreach (EntityRecommendation e in result.Entities)
            {
                string entity = e.Entity;
                //if (UserInfo.topicList.Count == 0)
                //{
                    Topic t = new Topic();
                    t.title = entity;
                    //UserInfo.topicList.Add(t);
                    //UserInfo.topicNames.Add(entity);

                    Dictionary<ChannelAccount, DateTime> userList;

                BotUserChannel newChannel = new BotUserChannel();
                newChannel.recipient = recipient;
                newChannel.from = from;
                
                    if (!UserInfo.topicDict.TryGetValue(t.title, out userList)) 
                    {
                    UserInfo.topicDict.Add(t.title, new Dictionary<ChannelAccount, DateTime>() { { recipient, DateTime.Now.AddHours(-2) } });
                    UserInfo.topicDict2.Add(t.title, new Dictionary<BotUserChannel, DateTime>() { { newChannel, DateTime.Now.AddHours(-2) } });
                    await context.PostAsync($"your url is: {serviceurl}");
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
                        await context.PostAsync($"You are now following the topic {entity}");
                    }

                //}
                //else
                /* {
                    //can't add things to list while enumerating
                    List<string> tempList = new List<string>();

                    if (!UserInfo.topicNames.Contains(entity))
                    {
                        Topic t = new Topic();
                        t.title = entity;
                        UserInfo.topicList.Add(t);
                        tempList.Add(entity);
                        await context.PostAsync($"You are now following the topic {entity}");
                    }
                    else
                    {
                        await context.PostAsync($"You are already following the topic {entity}");
                    }


                    foreach (string topic in tempList)
                    {
                        UserInfo.topicNames.Add(topic);
                    }
                } */
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