using System;
using System.Configuration;

namespace eaep
{
	public class Configuration
	{
		public static multicast.MulticastSettings MulticastSettings = new multicast.MulticastSettings("239.192.1.1", 60601, 1);		

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
        }
	}
}
