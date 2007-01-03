using System;
using System.Net.Sockets;

namespace torrent.libtorrent
{
    internal delegate void HandshakeEventHandler(object sender, HandshakeMessage message);

    internal class PeerEventArgs : EventArgs
    {
        public Exception Exception;

        public PeerEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }

    internal class Peer
    {
        private ClientSocket socket;
        private Torrent torrent;
        private PeerId myPeerId;

        public event EventHandler Connected;
        public EventHandler<PeerEventArgs> Error;
        public event HandshakeEventHandler HandshakeReceived;

        public Peer(ClientSocket socket, Torrent torrent, PeerId myId)
        {
            this.socket = socket;
            this.torrent = torrent;
            myPeerId = myId;
            this.socket.SocketError += new SocketErrorHandler(OnSocketError);
        }

        private void OnSocketError(object sender, SocketException se)
        {
            FireEvent(Error, new PeerEventArgs(se));
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
            socket.Connect(delegate
            {
                FireEvent(Connected, new EventArgs());
                HandshakeMessage message = new HandshakeMessage(torrent.InfoHash, myPeerId);
                socket.Send(message.ToBytes(), delegate
                    {
                        socket.Receive(68, delegate(ReceiveEventArgs e)
                        {
                            FireEvent(HandshakeReceived, new HandshakeMessage(e.Message.ToBytes()));
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
                    FireEvent(HandshakeReceived, new HandshakeMessage(e.Message.ToBytes()));
                };
            socket.Receive(68);
        }
    }
}