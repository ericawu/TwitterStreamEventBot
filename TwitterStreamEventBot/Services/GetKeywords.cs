using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TwitterStreamEventBot.Resource;

namespace TwitterStreamEventBot.Services
{
    public static class GetKeywords
    {
        public static async Task<List<string>> MakeRequestAsync(List<string> tweets)
        {
            if (tweets.Count > 0)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Constants.TextAnalyticsSubscriptionKey);

                List<Dictionary<string, string>> jsonTweets = new List<Dictionary<string, string>>();
                int count = 0;

                foreach (string t in tweets)
                {
                    Dictionary<string, string> input = new Dictionary<string, string>()
                {
                    {"language", "en"},
                    {"id", $"{count}" },
                    {"text", $"{t}"}
                };
                    jsonTweets.Add(input);
                    count++;
                }

                var body = new Dictionary<string, List<Dictionary<string, string>>>()
                {
                    {"documents", jsonTweets},
                };

                string json = JsonConvert.SerializeObject(body);

                var uri = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/keyPhrases?";

                HttpResponseMessage response;
               
                byte[] byteData = Encoding.UTF8.GetBytes(json);

                using (var content = new ByteArrayContent(byteData))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        response = await client.PostAsync(uri, content);
                    }
                List<string> topicList = new List<string>();
                if (response.IsSuccessStatusCode)
                {
                    var c = await response.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(c);

                    foreach (JObject j in data["documents"])
                    {
                        foreach (string topic in j["keyPhrases"])
                        {
                            topicList.Add(topic);
                        }
                    }

                    
                }
                return topicList;
            }
            else
            {
                return null;
            }


        }
    }
}