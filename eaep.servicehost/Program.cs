using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace eaep.servicehost
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new EAEPService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
