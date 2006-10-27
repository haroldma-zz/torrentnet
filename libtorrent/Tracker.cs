using System;
using System.Net.Sockets;

namespace torrent.libtorrent
{
    public class Tracker
    {
        private Torrent file;
        private ClientSocket socket;
        private TrackerResponse lastResponse;
        
        public delegate void TrackerErrorHandler(object sender, Exception e);

        public event EventHandler Connected;
        public event EventHandler Updated;
        public event TrackerErrorHandler Error;

        public Tracker(Torrent file, ClientSocket socket)
        {
            this.file = file;
            this.socket = socket;
            this.socket.ConnectionEstablished += OnConnectionEstablished;
            this.socket.MessageSent += OnMessageSent;
            this.socket.MessageReceived += OnMessageReceived;
            this.socket.SocketError += OnError;
        }
        
        public Tracker(Torrent file):this(file, new AsyncClientSocket(file.AnnounceUri))
        {
            
        }

        private void OnError(object sender, Exception se)
        {
            if(Error != null)
            {
                Error(this, se);
            }
        }

        private void OnMessageReceived(object sender, ReceiveEventArgs e)
        {
            try
            {
                DecodeResponse(e.Message);
                FireEvent(Updated);
            }
            catch (BenDecoderException bde)
            {
                OnError(this, bde);
            }
        }

        private void FireEvent(EventHandler eventHandler)
        {
            if (eventHandler != null)
            {
                eventHandler(this, new EventArgs());
            }
        }

        private void DecodeResponse(ByteString message)
        {
            lastResponse = new TrackerResponse(message);
        }

        private void OnMessageSent(object sender, EventArgs e)
        {
            ReceiveResponse();
        }

        private void ReceiveResponse()
        {
            socket.Receive(1024);
        }

        public TrackerResponse LastResponse
        {
            get { return lastResponse; }
        }

        private void OnConnectionEstablished(object sender, EventArgs e)
        {
            FireEvent(Connected);
            socket.Send(string.Format("GET {0}?info_hash={1}&peer_id={2}&port=6881&" +
                                      "compact=1&uploaded=0&downloaded=0&left=3706908089&event=started HTTP/1.1\r\n\r\n", file.AnnounceUri.AbsolutePath, file.Hash.ToUrlString(), PeerId.GetId()));
        }

        public void Start()
        {
            socket.Connect();
        }

        public void Close()
        {
            socket.Close();
        }
    }
}