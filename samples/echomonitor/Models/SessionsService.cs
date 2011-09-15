using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eaep;
using eaep.store;

namespace echomonitor.Models
{
    public class SessionsService : ISessionsService
    {
        protected IEAEPMonitorClient eaepMonitorClient;

        public SessionsService()
            : this(new EAEPMonitorClient())
        {
        }

        public SessionsService(IEAEPMonitorClient eaepMonitorClient)
        {
            this.eaepMonitorClient = eaepMonitorClient;
        }

        public SessionsSummary GetDaySummary(string application, DateTime date)
        {
            return GetDaySummary(application, date, 96);
        }

        public SessionsSummary GetDaySummary(string application, DateTime date, int timeslices)
        {
            SessionsSummary summary = new SessionsSummary();

            CountResult[] results = eaepMonitorClient.Count(
                string.Format("{0}:{1}", EAEPMessage.FIELD_APPLICATION, application),
                date.Date,
                date.Date.AddDays(1),
                timeslices,
                EAEPMessage.PARAM_USER
                );

            // construct framework array
            List<int[]> xycoord = new List<int[]>();

            for (int i = 0; i < timeslices; i++)
            {
                xycoord.Add(new int[] { i, 0 });
            }

            // populate with values
            // in this case we're just counting the instances of each 
            // separate user name in each time slice
            foreach (CountResult result in results)
            {
                xycoord[result.TimeSlice][1]++;
            }

            summary.data = xycoord.ToArray() ;

            return summary;
        }

        public string[] ActiveUsers(string application)
        {
            string[] users = eaepMonitorClient
                .Distinct(string.Format("{0}:{1}", EAEPMessage.FIELD_APPLICATION, application), DateTime.Now.AddHours(-1), DateTime.Now, EAEPMessage.PARAM_USER);

            return users;
        }
    }
}
