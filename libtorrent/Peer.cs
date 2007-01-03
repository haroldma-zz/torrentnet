using System;
using System.Net.Sockets;

namespace torrent.libtorrent
{
    public class PeerEventArgs : EventArgs
    {
        public Exception Exception;
        public HandshakeMessage Handshake;

        public PeerEventArgs(HandshakeMessage message, Exception exception)
        {
            Exception = exception;
            Handshake = message;
        }
    }

    public class Peer
    {
        private ClientSocket socket;
        private Torrent torrent;
        private PeerId myPeerId;
        private PeerInfo info;

        public event EventHandler Connected;
        public event EventHandler<PeerEventArgs> Error;
        public event EventHandler<PeerEventArgs> HandshakeReceived;
        
        public Peer(PeerInfo remotePeerData, Torrent torrent, PeerId myId) : this(new AsyncClientSocket(remotePeerData.IpAddress.ToString(), remotePeerData.Port), torrent, myId)
        {
            info = remotePeerData;
        }

        internal Peer(ClientSocket socket, Torrent torrent, PeerId myId)
        {
            this.socket = socket;
            this.torrent = torrent;
            myPeerId = myId;
            this.socket.SocketError += new SocketErrorHandler(OnSocketError);
        }

        private void OnSocketError(object sender, SocketException se)
        {
            FireEvent(Error, new PeerEventArgs(null, se));
        }

        internal Peer(ClientSocket socket, PeerId myId)
        {
            this.socket = socket;
            myPeerId = myId;
        }

        public Torrent Torrent
        {
            set { torrent = value; }
        }

        public PeerInfo Info
        {
            get { return info; }
        }

        public void Connect()
        {
            socket.Connect(delegate
            {
                FireEvent(Connected, new EventArgs());
                HandshakeMessage message = new HandshakeMessage(torrent.InfoHash, myPeerId);
                socket.Send(message.ToBytes(), delegate
                    {
                        socket.Receive(68, delegate(ReceiveEventArgs e)
                        {
                            FireEvent(HandshakeReceived, new PeerEventArgs(new HandshakeMessage(e.Message.ToBytes()), null));
                        });
                    });
            });
        }

        private void SendHandshake()
        {
            HandshakeMessage message = new HandshakeMessage(torrent.InfoHash, myPeerId);
            socket.Send(message.ToBytes());
        }

        private void FireEvent(Delegate handler, object args)
        {
            if (handler != null)
            {
                handler.DynamicInvoke(this, args);
            }
        }

        public void AcceptHandshake()
        {
            ReceiveHandshake();
            SendHandshake();
        }

        private void ReceiveHandshake()
        {
            socket.MessageReceived += delegate(object sender, ReceiveEventArgs e)
                {
                    FireEvent(HandshakeReceived, new PeerEventArgs(new HandshakeMessage(e.Message.ToBytes()), null));
                };
            socket.Receive(68);
        }
    }
}