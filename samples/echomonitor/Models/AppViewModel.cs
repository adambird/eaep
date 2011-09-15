using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace echomonitor.Models
{
    public class AppViewModel
    {
        public string Name { get; set; }
        public int PollInterval { get; set; }

        public AppViewModel()
        {
            this.PollInterval = Configuration.DefaultPollInterval;
        }
    }
}
