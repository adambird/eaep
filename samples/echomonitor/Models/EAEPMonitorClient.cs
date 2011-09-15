using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using eaep.http;
using eaep;
using eaep.store;

namespace echomonitor.Models
{
    public class EAEPMonitorClient : IEAEPMonitorClient
    {
        #region IEAEPMonitorClient Members

        public CountResult[] Count(string query, DateTime rangeFrom, DateTime rangeTo, int timeSlices)
        {
            return Count(query, rangeFrom, rangeTo, timeSlices, null);
        }

        public CountResult[] Count(string query, DateTime rangeFrom, DateTime rangeTo, int timeSlices, string groupBy)
        {
            WebRequest request = HttpWebRequest.Create(ConstructCountURI(query, rangeFrom, rangeTo, timeSlices, groupBy));
            request.Timeout = Configuration.DefaultEAEPClientTimeout;
            WebResponse response = request.GetResponse();

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return JsonConvert.DeserializeObject<CountResult[]>(reader.ReadToEnd());
            }
        }

        public static string ConstructCountURI(string query, DateTime rangeFrom, DateTime rangeTo, int timeSlices, string groupBy)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(Configuration.MonitorURI);
            builder.Append("count.json");
            builder.AppendFormat("?{0}={1}", Constants.QUERY_STRING_QUERY, query);
            builder.AppendFormat("&{0}={1}", Constants.QUERY_STRING_FROM, rangeFrom.ToString(Constants.FORMAT_DATETIME));
            builder.AppendFormat("&{0}={1}", Constants.QUERY_STRING_TO, rangeTo.ToString(Constants.FORMAT_DATETIME));
            builder.AppendFormat("&{0}={1}", Constants.QUERY_STRING_TIMESLICES, timeSlices.ToString("0"));
            if (groupBy != null)
            {
                builder.AppendFormat("&{0}={1}", Constants.QUERY_STRING_GROUPBY, groupBy);
            }

            return builder.ToString() ;
        }

        public EAEPMessages Query(string query)
        {
            return DoQuery(ConstructSearchURI(query));
        }

        public EAEPMessages Query(string query, DateTime since)
        {
            return Query(query, since, DateTime.Now);
        }

        public EAEPMessages Query(string query, DateTime from, DateTime to)
        {
            StringBuilder builder = new StringBuilder(ConstructSearchURI(query));
            builder.AppendFormat("&{0}={1}", Constants.QUERY_STRING_FROM, from.ToString(Constants.FORMAT_DATETIME));
            builder.AppendFormat("&{0}={1}", Constants.QUERY_STRING_TO, to.ToString(Constants.FORMAT_DATETIME));

            return DoQuery(builder.ToString());
        }

        protected EAEPMessages DoQuery(string uri)
        {
            WebRequest request = HttpWebRequest.Create(uri);
            request.Timeout = Configuration.DefaultEAEPClientTimeout;
            WebResponse response = request.GetResponse();

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<EAEPMessages>(json);
            }
        }

        public string[] Distinct(string query, DateTime rangeFrom, DateTime rangeTo, string field)
        {
            WebRequest request = HttpWebRequest.Create(ConstructDistinctURI(query, rangeFrom, rangeTo, field));
            request.Timeout = Configuration.DefaultEAEPClientTimeout;

            WebResponse response = request.GetResponse();

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return JsonConvert.DeserializeObject<string[]>(reader.ReadToEnd());
            }
        }

        public static string ConstructSearchURI(string query)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(Configuration.MonitorURI);
            builder.Append("search.json");
            builder.AppendFormat("?{0}={1}", Constants.QUERY_STRING_QUERY, query);

            return builder.ToString();
        }

        public static string ConstructDistinctURI(string query, DateTime from, DateTime to, string field)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(Configuration.MonitorURI);
            builder.Append("distinct.json");
            builder.AppendFormat("?{0}={1}", Constants.QUERY_STRING_QUERY, query);
            builder.AppendFormat("&{0}={1}", Constants.QUERY_STRING_FROM, from.ToString(Constants.FORMAT_DATETIME));
            builder.AppendFormat("&{0}={1}", Constants.QUERY_STRING_TO, to.ToString(Constants.FORMAT_DATETIME));
            builder.AppendFormat("&{0}={1}", Constants.QUERY_STRING_FIELD, field);

            return builder.ToString();
        }

        #endregion
    }
}
