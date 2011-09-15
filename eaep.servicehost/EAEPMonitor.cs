using System;
using eaep.multicast;
using eaep.servicehost.store;
using log4net;

namespace eaep.servicehost
{
    public class EAEPMonitor : IDisposable
	{
        protected static ILog log = LogManager.GetLogger(typeof(SQLMonitorStore));
        
        protected IEAEPNode eaepNode;
		protected IEAEPMonitorStore monitorStore;
		protected EAEPMonitorService monitorService;

        public event EventHandler Stopped;

        protected bool stopping;

		public EAEPMonitor()
			: this (Configuration.MulticastSettings, Configuration.MonitorStorePath)
		{ 
		}

        public EAEPMonitor(string storePath)
        {
            Initialise(Configuration.MulticastSettings, storePath);
        }

		public EAEPMonitor(MulticastSettings multicastSettings)
		{
			Initialise(multicastSettings, null);
		}

		public EAEPMonitor(MulticastSettings multicastSettings, string storePath)
		{
			Initialise(multicastSettings, storePath);
		}

		protected void Initialise(MulticastSettings multicastSettings, string storePath)
		{
            monitorStore = new SQLMonitorStore(Configuration.MonitorStoreConnectionString);

            eaepNode = new EAEPNode(multicastSettings);
			eaepNode.MessageReceived += new MessageReceivedHandler(eaepNode_MessageReceived);

			monitorService = new EAEPMonitorService(this);
            monitorService.Stopped += new EventHandler(monitorService_Stopped);
		}

        void monitorService_Stopped(object sender, EventArgs e)
        {
            log.Debug("Stopped event received from MonitorService");
            this.Stop(true);
        }

		void eaepNode_MessageReceived(EAEPMessage message)
		{
            monitorStore.PushMessage(message);
		}

		public void Start()
		{
            log.Debug("Monitor Store starting");

            eaepNode.Start();
			monitorService.Start();

            log.Info("Monitor Store started");
        }

		public void Stop()
		{
            Stop(false);
        }

        public void Stop(bool raiseEvent)
        {
            if (!stopping)
            {
                stopping = true;

                log.Debug("Monitor Store stopping");

                if (monitorStore != null) monitorStore.Close();
                if (monitorService != null) monitorService.Stop();
                if (eaepNode != null) eaepNode.Stop();

                if (raiseEvent && this.Stopped != null)
                {
                    this.Stopped(this, null);
                }

                stopping = false;
                log.Info("Monitor Store stopped");
            }
		}

		#region IDisposable Members

		public void Dispose()
		{
			Stop();
			eaepNode.Dispose();
			monitorService.Dispose();
            monitorStore.Dispose();
		}

		#endregion

        public IEAEPMonitorStore Store
        {
            get { return this.monitorStore; }
        }

		public EAEPMessages Search(string query)
		{
            return monitorStore.GetMessages(query);
		}

		public EAEPMessages Search(DateTime since, string query)
		{
            return monitorStore.GetMessages(since, query);
        }
	}
}
