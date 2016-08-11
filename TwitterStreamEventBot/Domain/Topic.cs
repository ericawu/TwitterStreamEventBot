using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterStreamEventBot.Domain
{
    public class Topic
    {
        public string title { get; set; }
        //public bool alreadyMessaged { get; set; }
        public DateTime messagedTime { get; set; }
    }
}