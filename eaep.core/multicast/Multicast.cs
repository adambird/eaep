using System;
using System.Net;
using System.Net.Sockets;
using log4net;

namespace eaep.multicast
{
    public class Multicast : IMulticast
    {
        protected static ILog log = LogManager.GetLogger(typeof(Multicast));
        public MulticastSettings Settings { get; private set; }
        private readonly object runLock = new object();
        private bool isRunning;
        protected Socket multicastSocket;

        public Multicast(MulticastSettings settings)
        {
            if(settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Settings = settings;
        }

        public event ReceiveHandler DataReceived;

        public void Start()
        {
            lock(runLock)
            {
                if(isRunning)
                {
                    return;
                }

                var ipEndPoint = new IPEndPoint(IPAddress.Any, Settings.Port);
                var listenEndPoint = (EndPoint) ipEndPoint;

                multicastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                multicastSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                multicastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, Settings.TimeToLive);
                multicastSocket.Bind(ipEndPoint);

                var multicastOption = new MulticastOption(Settings.MulticastGroupAddress, IPAddress.Any);
                multicastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);

                var stateObject = new StateObject { Socket = multicastSocket };

                multicastSocket.BeginReceiveFrom(stateObject.Buffer, 0, stateObject.Buffer.Length, SocketFlags.None, ref listenEndPoint, OnReceiveSocketData, stateObject);

                isRunning = true;
            }
        }

        public void Stop()
        {
            lock(runLock)
            {
                if(isRunning == false)
                {
                    return;
                }

                var option = new MulticastOption(Settings.MulticastGroupAddress);
                multicastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DropMembership, option);
                multicastSocket.Close(5);
                multicastSocket = null;

                isRunning = false;
            }
        }

        private void OnReceiveSocketData(IAsyncResult asyncResult)
        {
            if(isRunning == false)
            {
                return;
            }

            EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);

            var stateObject = (StateObject) asyncResult.AsyncState;
            var socket = stateObject.Socket;

            try
            {
                socket.EndReceiveFrom(asyncResult, ref endPoint);

                DataReceived(stateObject.Buffer);

                socket.BeginReceiveFrom(stateObject.Buffer, 0, stateObject.Buffer.Length, SocketFlags.None, ref endPoint, OnReceiveSocketData, stateObject);
            }
            catch(SocketException e)
            {
                log.Error("Error receiving socket data", e);
            }
            catch(ObjectDisposedException e)
            {
                log.Info("Socket has been disposed of, most likely closed", e);
            }
        }
        
        public void Broadcast(byte[] data)
        {
            if(isRunning)
            {
                try
                {
                    var remoteEndPoint = new IPEndPoint(Settings.MulticastGroupAddress, Settings.Port);
                    multicastSocket.SendTo(data, remoteEndPoint);
                }
                catch(Exception e)
                {
                    log.Error(e);
                }
            }
        }
    }
}
