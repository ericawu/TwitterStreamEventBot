using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using TwitterStreamEventBot.Domain;
using TwitterStreamEventBot.Test;

namespace TwitterStreamEventBot.Service
{
    public static class CheckTrends
    {
        public static void Check()
        {
            //might change topicList to a hashset instead
            foreach (Topic t in UserInfo.topicList)
            {
                if(Trending.trendList.Contains(t.title)) {
                    if (DateTime.Now > t.messagedTime.AddMinutes(1)) {
                        Debug.WriteLine("check");

                        //TODO: proactive message to user
                        t.messagedTime = DateTime.Now;
                    }
                }
            }
        }
    }
}