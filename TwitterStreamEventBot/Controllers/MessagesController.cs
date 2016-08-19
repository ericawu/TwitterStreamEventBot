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
using TwitterStreamEventBot.Domain;
using System.Collections.Generic;
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

        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
             var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {

                await Conversation.SendAsync(activity, () => new EventBotDialogue());
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

            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                var reply = message.CreateReply();
                reply.Text = $"Hey {message.From.Name}! Tell me what you want to subscribe to, and I'll let you know if anything interesting is happening!";
                connector.Conversations.ReplyToActivity(reply);
            }
            else if (message.Type == ActivityTypes.Ping)
            {

            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {

            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
           
            return null;
        }
    }
}