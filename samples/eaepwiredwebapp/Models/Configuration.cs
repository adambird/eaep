using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;

namespace eaepwiredwebapp.Models
{
    public class Configuration
    {
        public static string ApplicationName;
        public static string EAEPMonitorURI;
        public static int EAEPHttpClientTimeout;

        public static bool EAEPEnabled
        {
            get
            {
                return EAEPMonitorURI != null;
            }
        }

        static Configuration()
        {
            AppSettingsReader reader = new AppSettingsReader();
            try
            {
                ApplicationName = (string)reader.GetValue("ApplicationName", typeof(string));
            }
            catch (Exception)
            {
                ApplicationName = Assembly.GetExecutingAssembly().FullName;
            }

            try
            {
                EAEPMonitorURI = (string)reader.GetValue("EAEPMonitorURI", typeof(string));
            }
            catch (Exception)
            {
                EAEPMonitorURI = null;
            }

            try
            {
                EAEPHttpClientTimeout = (int)reader.GetValue("EAEPHttpClientTimeout", typeof(int));
            }
            catch (Exception)
            {
                EAEPHttpClientTimeout = 100;
            }

        }
    }
}
