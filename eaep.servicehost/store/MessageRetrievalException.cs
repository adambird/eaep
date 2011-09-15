using System;

namespace eaep.servicehost.store
{
    public class MessageRetrievalException : ApplicationException
    {
        public MessageRetrievalException(string message)
            : base(message)
        {
        }
    }
}
