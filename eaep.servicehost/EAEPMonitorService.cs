using System;
using System.Net;
using eaep.servicehost.http;
using log4net;

namespace eaep.servicehost
{
	/// <summary>
	/// Acts as an service interface layer onto an EAEPMonitor
	/// </summary>
	public class EAEPMonitorService : IDisposable
	{
        static ILog log = LogManager.GetLogger(typeof(EAEPMonitorService));

		HttpListener listener;
		EAEPMonitor monitor;

        public event EventHandler Stopped;

		public EAEPMonitorService(EAEPMonitor monitor)
		{
			this.monitor = monitor;
			InitialiseHttpListener();
		}

		protected void InitialiseHttpListener()
		{
			listener = new HttpListener();
			listener.Prefixes.Add(string.Format("http://*:{0}/", Configuration.ServicePort));
		}

		public void Start()
		{
			listener.Start();
			listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
            log.Info("Started");
		}

		public void Stop()
		{
			listener.Stop();
            if (this.Stopped != null)
            {
                this.Stopped(this, null);
            }

            log.Info("Stopped");
        }

		public void ListenerCallback(IAsyncResult result)
		{
            log.Debug("ListenerCallback entered");

            try
            {
                HttpListener listener = (HttpListener)result.AsyncState;
                HttpListenerContext context = listener.EndGetContext(result);

                try
                {
                    ServiceRequest request = new ServiceRequest(context.Request);
                    ServiceResponse response = new ServiceResponse(context.Response);

                    IRequestHandler handler = RequestHandlerFactory.GetHandler(request, monitor.Store);

                    handler.Handle(request, response);

                    context.Response.Close();
                    log.Debug("ListenerCallback completed");

                }
                catch (HttpListenerException ex)
                {
                    /* Ignored as probably client disposing before we write response */
                    log.Warn(ex);
                }
                catch (ObjectDisposedException ex)
                {
                    /* again ignored as probably connectivity issues */
                    log.Warn(ex);
                }
                catch (ApplicationException ex)
                {
                    /* this represents an exception that the application is acknowledged 
                     * and decided to throw anyway so considering it safe to continue */
                    log.Warn(ex);
                }
                finally
                {
                    listener.BeginGetContext(new AsyncCallback(ListenerCallback), listener);
                    log.Debug("Call back wired up to receive next request");
                }
            }
            catch (Exception ex)
            {
                log.Fatal(ex);
                this.Stop();
            }

		}

		#region IDisposable Members

		public void Dispose()
		{
			this.Stop();
			this.listener = null;
			this.monitor = null;
		}

		#endregion
	}
}
