using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using TwitterStreamEventBot.App_Start;
using TwitterStreamEventBot.Domain;

namespace TwitterStreamEventBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static int testInt = 15;

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            TweetStream.initializeStream();
            //TwitterConfig.StartStream();
            Application["ApplicationTest"] = "Test of application state";
           UserInfo.topicDict = new Dictionary<string, Dictionary<Microsoft.Bot.Connector.ChannelAccount, DateTime>>();
            UserInfo.topicDict2 = new Dictionary<string, Dictionary<BotUserChannel, DateTime>>();
        }        
    }

}
