using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eaep;
using eaep.store;

namespace echomonitor.Models
{
    public interface IEAEPMonitorClient
    {
        EAEPMessages Query(string query);
        EAEPMessages Query(string query, DateTime since);
        EAEPMessages Query(string query, DateTime from, DateTime to);
        CountResult[] Count(string query, DateTime rangeFrom, DateTime rangeTo, int timeSlices);
        CountResult[] Count(string query, DateTime rangeFrom, DateTime rangeTo, int timeSlices, string groupBy);
        string[] Distinct(string query, DateTime rangeFrom, DateTime rangeTo, string field);
    }
}
