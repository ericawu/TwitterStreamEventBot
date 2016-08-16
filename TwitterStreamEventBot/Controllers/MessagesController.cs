using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using System.Diagnostics;
using TwitterStreamEventBot.DBManager;
using TwitterStreamEventBot.Domain;
using System.Collections.Generic;
using TwitterStreamEventBot.Dialogs;
using TwitterStreamEventBot.Services;
using TwitterStreamEventBot.Controllers;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;

namespace TwitterStreamEventBot
{
    [BotAuthentication]

    public class MessagesController : ApiController
    {

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        /// 

        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
             var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                var testMessage = activity.CreateReply("test");
                connector.Conversations.ReplyToActivity(testMessage);

                if (UserInfo.topicList == null)
                {
                    UserInfo.topicList = new List<Topic>();
                    UserInfo.topicNames = new HashSet<string>();
                }
                ConfigurationManager.AppSettings["BotId"] = "test";
                // await Conversation.SendAsync(activity, () => new NotificationDialog("test"));
                var test = new EventBotDialogue();
                await Conversation.SendAsync(activity, () => test);
                //await Conversation.SendAsync(activity, () => new EventBotDialogue(activity.Recipient));
            }
            else if (activity.GetActivityType() == ActivityTypes.ConversationUpdate)
            {                
                //SelectRows();
                //DBQueries dbconnection = new DBQueries();
                //Task.Run(() => DBQueries.GetLastCount("ALLTWEETS"));
                //DBQueries.GetLastCount("ALLTWEETS");
                // SAManager m = new SAManager();
                //m.test();
                //var replyMessage = message.CreateReply("should only run once");
                //connector.Conversations.ReplyToActivityAsync(replyMessage);
                //await Conversation.SendAsync(activity, () => new NotificationDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }

        private async Task<Activity> HandleSystemMessage(Activity message)
        {
            var connector = new ConnectorClient(new Uri(message.ServiceUrl));
            var url = message.ServiceUrl;
            Debug.WriteLine(url);
            var recipient = message.Recipient;
            Debug.WriteLine(recipient);
            var from = message.From;
            Debug.WriteLine(from);

            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.Ping)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels

                // await Conversation.SendAsync(Activity, () =)
                /*
                Uri notificationUri = new Uri("http://localhost:4999/api/Notifications");
                var client = new HttpClient();
                await client.GetAsync(notificationUri);
                */
                var notificationController = new NotificationController();
                //await notificationController.SendMessage(url, recipient, from);
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened

                //TODO: Start new looping dialog that runs in a Task.Run()
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
           

            return null;
        }
    }
}