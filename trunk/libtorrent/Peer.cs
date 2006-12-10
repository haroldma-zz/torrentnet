using System;

namespace torrent.libtorrent
{
    internal delegate void HandshakeEventHandler(object sender, HandshakeMessage message);
    
    internal class Peer
    {
        private ClientSocket socket;
        private Torrent torrent;
        private PeerId myPeerId;

        public event EventHandler Connected;
        public event HandshakeEventHandler HandshakeReceived;

        public Peer(ClientSocket socket, Torrent torrent, PeerId myId)
        {
            this.socket = socket;
            this.torrent = torrent;
            myPeerId = myId;
        }

        public Peer(ClientSocket socket, PeerId myId)
        {
            this.socket = socket;
            myPeerId = myId;
        }

        public Torrent Torrent
        {
            set { torrent = value; }
        }

        public void Connect()
        {
            socket.ConnectionEstablished += delegate
                {
                    FireEvent(Connected, new EventArgs());
                    SendHandshake();
                    ReceiveHandshake();
                };
            socket.Connect();
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
                    FireEvent(HandshakeReceived, new HandshakeMessage(e.Message.ToBytes()));
                };
            socket.Receive(68);
        }
    }
}