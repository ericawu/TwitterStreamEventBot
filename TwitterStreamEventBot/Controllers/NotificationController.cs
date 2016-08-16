using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace TwitterStreamEventBot.Controllers
{
    [BotAuthentication]
    public class NotificationController : ApiController 
    {
        [HttpGet]
        [Route("api/Notifications")]
        public async Task SendMessage(string url, ChannelAccount recipient, ChannelAccount from, string topic)
        {
            var connector = new ConnectorClient(new Uri(url));
            IMessageActivity newMessage = Activity.CreateMessageActivity();
            var conversation = await connector.Conversations.CreateDirectConversationAsync(recipient, from);
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
            newMessage.Conversation = new ConversationAccount(id: conversation.Id);

            await connector.Conversations.SendToConversationAsync((Activity)newMessage);
        }
    }
}