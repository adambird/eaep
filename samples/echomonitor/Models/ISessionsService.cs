using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace echomonitor.Models
{
    public interface ISessionsService
    {
        SessionsSummary GetDaySummary(string application, DateTime date);
        string[] ActiveUsers(string application);
    }
}
