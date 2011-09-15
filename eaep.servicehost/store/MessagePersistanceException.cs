using System;

namespace eaep.servicehost.store
{
    public class MessagePersistanceException : ApplicationException
    {
        public MessagePersistanceException(string message)
            : base(message)
        {
        }
    }
}
