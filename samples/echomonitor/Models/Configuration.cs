using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace echomonitor.Models
{
    public class Configuration
    {
        public static string MonitorURI;
        public static int DefaultPollInterval;
        public static int DefaultEAEPClientTimeout;

        static Configuration()
        {
            AppSettingsReader reader = new AppSettingsReader();

            try
            {
                MonitorURI = (string)reader.GetValue("EAEPMonitorURI", typeof(string));
            }
            catch
            {
                MonitorURI = null;
            }

            try
            {
                DefaultPollInterval = (int)reader.GetValue("DefaultPollInterval", typeof(int));
            }
            catch
            {
                DefaultPollInterval = 30;
            }

            try
            {
                DefaultEAEPClientTimeout = (int)reader.GetValue("DefaultEAEPClientTimeout", typeof(int)) * 1000;
            }
            catch
            {
                DefaultEAEPClientTimeout = 30000;
            }
        }
    }
}
