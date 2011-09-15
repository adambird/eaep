using System.Text;
using eaep.multicast;

namespace eaep
{
	public class EAEPNode : IEAEPNode
	{
	    private readonly IMulticast _multicaster;
	    private readonly IEAEPMessageSerializer _eaepMessageSerializer;
	    public MulticastSettings MulticastSettings { get; private set; }

		public event MessageReceivedHandler MessageReceived;

		public EAEPNode()
			: this(Configuration.MulticastSettings)
		{
		}

		public EAEPNode(MulticastSettings multicastSettings)
            : this(multicastSettings, new Multicast(multicastSettings), new EAEPMessageSerializer())
		{
			
		}

        public EAEPNode(MulticastSettings multicastSettings, IMulticast multicaster, IEAEPMessageSerializer eaepMessageSerializer)
        {
            _multicaster = multicaster;
            _eaepMessageSerializer = eaepMessageSerializer;
            Initialise(multicastSettings);
        }

	    private void Initialise(MulticastSettings multicastSettings)
		{
			MulticastSettings = multicastSettings;
            _multicaster.DataReceived += new ReceiveHandler(Multicaster_DataReceived);
		}

		void Multicaster_DataReceived(byte[] data)
		{
            if (MessageReceived != null) 
            {
                var message = _eaepMessageSerializer.Deserialize(data);

                if (message != null) MessageReceived(message);
			}
		}

	    
	    public void Start()
		{
            _multicaster.Start();
		}

		public void Stop()
		{
            _multicaster.Stop();
		}

		public void SendMessage(string host, string application, string eventName)
		{
			SendMessage(new EAEPMessage(host, application, eventName));
		}

		public void SendMessage(EAEPMessage message)
		{
			byte[] data = Encoding.UTF8.GetBytes(message.ToString());
            _multicaster.Broadcast(data);
		}

		public void Dispose()
		{
			Stop();
		}

	}
}
