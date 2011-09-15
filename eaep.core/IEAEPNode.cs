using System;

namespace eaep
{
	public delegate void MessageReceivedHandler(EAEPMessage message);

	public interface IEAEPNode : IDisposable
	{
		event MessageReceivedHandler MessageReceived;
		void SendMessage(EAEPMessage message);
		void Start();
		void Stop();
	}
}
