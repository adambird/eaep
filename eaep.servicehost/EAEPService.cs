using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using eaep;

namespace eaep.servicehost
{
    public partial class EAEPService : ServiceBase
    {
        private EAEPMonitor monitor;

        public EAEPService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            monitor = new EAEPMonitor();
            monitor.Stopped += new EventHandler(monitor_Stopped);
            monitor.Start();
        }

        void monitor_Stopped(object sender, EventArgs e)
        {
            base.Stop();
        }

        protected override void OnStop()
        {
            monitor.Stop();
        }
    }
}
