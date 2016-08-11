using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterStreamEventBot.Resource
{
    public class Constants
    {
        public const string EventHubName = "TwitterChatBot";

        public const string ConnectionString = "Endpoint=sb://twitterchatbot-ns.servicebus.windows.net/;SharedAccessKeyName=manage;SharedAccessKey=Re2ZHMqjt2MAwol24xIPKODwLqO6bnM9vmz1FUQ0saU=";
        //public const string ConnectionString = "Endpoint=sb://twitterchatbot-ns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=KTM5jRth/r/+dks9ZPOAS+0u/B94ZShEHDBC47RcYQk=";
        public const string TConsumerKey = "NrA3wEjj4hETgbJ8TvoVe2W13";
        public const string TConsumerKeySecret = "8T2iEvSUv41iFflkk0VmZkOOE6k2jgRawle7hIKTC0tiNqxsfj";
        public const string TAccessToken = "2826933075-M458c5nQzeqOIhZa45P5mktmIR6VNxcrhALEdMu";
        public const string TAcessTokenSecret = "9aUlBjqrYbeTfzipz28R0RVCV6SokMXsNXMDqVHNOuIab";
        public const string DBConnection = "Server=tcp:twitterstreamanalytics-server.database.windows.net,1433;Initial Catalog=TwitterBotStreamAnalyticsDB;Persist Security Info=False;User ID=t-eriwu@microsoft.com@twitterstreamanalytics-server;Password=Plumpy1235!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public const string ServerName = "twitterstreamanalytics-server";
        public const string DBName = "TwitterBotStreamAnalyticsDB";
        public const string DBUsername = "t-eriwu@microsoft.com@twitterstreamanalytics-server";
        public const string PW = "test";
        public const string ActiveDirectoryEndpoint = "https://login.windows.net/";
        public const string ResourceManagerEndpoint = "https://management.azure.com/";
        public const string WindowsManagementUri = "https://management.core.windows.net/";
        public const string AsaClientId = "1950a258-227b-4e31-a9cf-717495945fc2";
        public const string RedirectUri = "urn:ietf:wg:oauth:2.0:oob";
        public const string SubscriptionId = "4470e022-4110-48bb-b4e8-7656b1f6703f";
        public const string ActiveDirectoryTenantId = "72f988bf-86f1-41af-91ab-2d7cd011db47";
        public const string LUISSubscriptionId = "8010b0ad1fa34da798d444a2bc976739";
        public const string LUISAppId = "b5e838e1-5964-4211-bb7f-4b10815e6c22";
        public const string TextAnalyticsSubscriptionKey = "718e153bf0404706aa2c7d974c657daf";
    }
}