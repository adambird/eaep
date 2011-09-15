using System;
using System.Collections.Generic;
using eaep.multicast;
using System.Text;
using System.Net.Sockets;
using log4net;

namespace eaep
{
    public class EAEPBroadcaster : IEAEPEventLogger
    {
        private readonly ILog _logger = LogManager.GetLogger("eaep");
        private IMulticastSender _multicastSender;
        private string _application;
        private string _host;

        public EAEPBroadcaster(string host, string application, MulticastSettings multicastSettings) 
        {
            if(multicastSettings == null)
                throw new ArgumentNullException("multicastSettings");

            ConstructorLogic(host, application, new MulticastSender(multicastSettings));
        }

        internal EAEPBroadcaster(string host, string application, IMulticastSender multicast)
        {
            ConstructorLogic(host, application, multicast);
        }

        private void ConstructorLogic(string host, string application, IMulticastSender multicast)
        {
            if(host == null)
                throw new ArgumentNullException("host");

            if(application == null)
                throw new ArgumentNullException("application");

            if(host == "")
                throw new ArgumentException("host cannot be empty", "host");

            if(application == "")
                throw new ArgumentException("application cannot be empty", "application");

            _multicastSender = multicast;
            _host = host;
            _application = application;
        }

        public void LogEvent(string eventName, params EventParameter[] parameters)
        {
            LogEvent(DateTime.Now, eventName, parameters);
        }

        public void LogEvent(DateTime timestamp, string eventName, params EventParameter[] parameters)
        {
            if(eventName == null) 
                throw new ArgumentNullException("eventName");

            if(eventName == "")
                throw new ArgumentException("eventName cannot be empty", "eventName");

            EAEPMessage message = ConstructMessage(eventName, timestamp, parameters);
            BroadcastMessage(message);
        }

        private void BroadcastMessage(EAEPMessage message)
        {
            try
            {
                var messageData = EncodeMessage(message);
                _multicastSender.Send(messageData);
            }
            catch(SocketException e)
            {
                _logger.Error(String.Format("message [{0}] cannot be sent", message), e);
            }
            catch(ObjectDisposedException e)
            {
                _logger.Error(String.Format("message [{0}] cannot be sent", message), e);
            }
        }

        private static byte[] EncodeMessage(EAEPMessage message)
        {
            return Encoding.UTF8.GetBytes(message.ToString());
        }

        private EAEPMessage ConstructMessage(string eventName, DateTime timestamp, IEnumerable<EventParameter> parameters)
        {
            var message = new EAEPMessage(timestamp, _host, _application, eventName);

            foreach(var parameter in parameters)
            {
                message[parameter.Name] = parameter.Value;
            }
            return message;
        }
    }
}