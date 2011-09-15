using System;
using System.Configuration;

namespace eaep.servicehost
{
	public class Configuration
	{
		public static multicast.MulticastSettings MulticastSettings = new multicast.MulticastSettings("239.192.1.1", 60601, 1);
		public static int ServicePort = 8085;
        public static string MonitorStorePath = null;
        public static string MonitorStoreConnectionString = null;
        public static int DefaultQueryTimeout = 15;

        static Configuration()
        {
            AppSettingsReader reader = new AppSettingsReader();
            try
            {
                MulticastSettings = new eaep.multicast.MulticastSettings(
                    (string)reader.GetValue("MulticastGroupAddress", typeof(string)),
                    (int)reader.GetValue("MulticastPortNumber", typeof(int)),
                    (int)reader.GetValue("MulticastTTL", typeof(int))
                    );
            }
            catch (Exception)
            {
                // if any errors reading the config file then leave settings as they are.
            }

            try
            {
                ServicePort = (int)reader.GetValue("HTTPInterfacePort", typeof(int));
            }
            catch (Exception)
            {
                // as above leave values if errors reading file
            }

            try
            {
                MonitorStorePath = (string)reader.GetValue("MonitorStorePath", typeof(string));
            }
            catch (Exception)
            {
                // as above leave values if errors reading file
            }

            try
            {
                MonitorStoreConnectionString = (string)reader.GetValue("MonitorStoreConnectionString", typeof(string));
            }
            catch (Exception)
            {
                // as above leave values if errors reading file
            }

            try
            {
                DefaultQueryTimeout = (int)reader.GetValue("DefaultQueryTimeout", typeof(int));
            }
            catch (Exception)
            {
                // as above leave values if errors reading file
            }

        }
	}
}
