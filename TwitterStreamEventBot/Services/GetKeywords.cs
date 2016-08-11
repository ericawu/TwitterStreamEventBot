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
                int count = 1;

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

                //put into body format
                var body = new Dictionary<string, List<Dictionary<string, string>>>()
                {
                    {"documents", jsonTweets},
                };

                string json = JsonConvert.SerializeObject(body);

                // +querystring?
                var uri = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/keyPhrases?";

                HttpResponseMessage response;

                byte[] byteData = Encoding.UTF8.GetBytes(json);

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await client.PostAsync(uri, content);
                }

                var c = await response.Content.ReadAsStringAsync();

                JObject data = JObject.Parse(c);

                List<string> topicList = new List<string>();
                //var data2 = data["documents"];

                foreach (JObject j in data["documents"])
                {
                    foreach (string topic in j["keyPhrases"])
                    {
                        //Debug.WriteLine(topic);
                        topicList.Add(topic);
                    }
                }
                //var x1 = data2[0];
                //var x = data2[0].GetType();
                //Debug.WriteLine("test");
                /*
                var data2 = data["documents"][0];
                var type = data2.GetType();
                var data3 = data["documents"][1];
                foreach(JObject j in data["documents"])
                {
                    j
                }


                var documents = data["documents"][0]["keyPhrases"];
                foreach (string topic in documents)
                {
                    Debug.WriteLine("topic: " + topic);
                }
                var documents2 = data["documents"][1]["keyPhrases"];
                foreach (string topic in documents2)
                {
                    Debug.WriteLine("topic: " + topic);
                }
                */
                Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!! COG SERVICES YEAHHHHHHH !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                return topicList;
            }
            else
            {
                return null;
            }


        }
    }
}