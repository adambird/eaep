using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eaep.servicehost;
using log4net;
using eaep;

namespace eaep.cmdhost
{
    class Program
    {
        static EAEPMonitor monitor;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            try
            {
                log4net.Config.XmlConfigurator.Configure();

                //monitor = new EAEPMonitor(@"C:\dev\data\eaepstoreindex3\");
                monitor = new EAEPMonitor();
                monitor.Start();
                Console.WriteLine("Monitor Started");
                Console.ReadLine();
                monitor.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }

            Console.WriteLine("Closing");
        }
    }
}
