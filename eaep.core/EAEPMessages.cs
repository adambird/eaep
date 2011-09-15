using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace eaep
{
	public class EAEPMessages : List<EAEPMessage>
	{
        public EAEPMessages()
        {
        }

        public EAEPMessages(string messages)
        {
            Load(messages);
        }			

		public void Load(string messages)
		{
			StringReader reader = new StringReader(messages);
			while (reader.Peek() != -1)
			{
				EAEPMessage message = new EAEPMessage();
				message.Load(reader);
				Add(message);
			}
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			StringWriter writer = new StringWriter(builder);

			foreach (EAEPMessage message in this)
			{
				builder.Append(message);
			}

			return builder.ToString();
		}        
    }
}
