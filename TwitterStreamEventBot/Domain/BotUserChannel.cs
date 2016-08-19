using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterStreamEventBot.Domain
{
    public class BotUserChannel
    {
        public ChannelAccount from { get; set; }
        public ChannelAccount recipient { get; set; }
        public string url { get; set; }
    }
}