using System;
using System.Collections.Generic;
using System.Text;

namespace eaep
{
    public class EAEPMessageSerializer : IEAEPMessageSerializer
    {
        public EAEPMessage Deserialize(byte[] data)
        {
            try
            {
                return new EAEPMessage(Encoding.UTF8.GetString(data));
            }
            catch
            {
                return null;
            }            
        }
    }
}
