using System;
using System.IO;
using System.Net;
using log4net;

namespace eaep.servicehost.http
{
    public delegate void EndSendEAEPMessageDelegate(EAEPMessage message, HttpStatusCode statusCode, string statusDescription);

    public class EAEPHttpClient : IEAEPHttpClient
    {
        static ILog log = LogManager.GetLogger(typeof(EAEPMonitorService));

        private int Timeout { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout">Request Timeout in milliseconds</param>
        public EAEPHttpClient(int timeout)
        {
            Timeout = timeout;
        }

        public void SendMessage(string uri, EAEPMessage message)
        {
            SendMessage(uri, message, null);
        }

        public void SendMessage(string uri, EAEPMessage message, EndSendEAEPMessageDelegate onwardDelegate)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Timeout = Timeout;
            request.Method = "POST";

            request.BeginGetRequestStream(GetRequestStreamCallback, new RequestState(request, onwardDelegate, message));
        }

        protected void GetRequestStreamCallback(IAsyncResult result)
        {
            try
            {
                RequestState state = result.AsyncState as RequestState;
                Stream requestStream = state.Request.EndGetRequestStream(result);

                using (StreamWriter writer = new StreamWriter(requestStream))
                {
                    writer.Write(state.Message);
                }

                state.Request.BeginGetResponse(GetResponseCallback, state);
            }
            catch (ObjectDisposedException)
            {

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        protected void GetResponseCallback(IAsyncResult result)
        {
            try
            {
                RequestState state = result.AsyncState as RequestState;
                HttpWebResponse response = (HttpWebResponse)state.Request.EndGetResponse(result);

                if (state.Delegate != null)
                {
                    state.Delegate(state.Message, response.StatusCode, response.StatusDescription);
                }
            }
            catch (ObjectDisposedException)
            { 
            
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private class RequestState
        {
            public HttpWebRequest Request { get; set; }
            public EndSendEAEPMessageDelegate Delegate { get; set; }
            public EAEPMessage Message { get; set; }

            public RequestState(HttpWebRequest request, EndSendEAEPMessageDelegate onwardDelegate, EAEPMessage message)
            {
                this.Request = request;
                this.Delegate = onwardDelegate;
                this.Message = message;
            }
        }
    }
}
