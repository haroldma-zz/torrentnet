using System;
using System.Net.Sockets;

namespace torrent.libtorrent
{
    internal class AsyncClientSocket : ClientSocket
    {
        public event EventHandler ConnectionEstablished;
        public event EventHandler MessageSent;
        public event EventHandler Disconnected;
        public event ReceiveEventHandler MessageReceived;
        public event SocketErrorHandler SocketError;


        private Socket socket;
        private int port;
        private string host;

        public AsyncClientSocket(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public AsyncClientSocket(Uri uri): this(uri.Host, uri.Port)
        {
            
        }

        public void Connect()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.BeginConnect(host, port, delegate(IAsyncResult ar)
                {
                    try
                    {
                        socket.EndConnect(ar);
                        FireEvent(ConnectionEstablished);
                    }
                    catch (SocketException se)
                    {
                        OnSocketError(se);
                    }
                }, null);
        }

        private void OnSocketError(SocketException se)
        {
            if(SocketError != null)
            {
                SocketError(this, se);
            }
        }

        private void FireEvent(EventHandler eventHandler)
        {
            if (eventHandler != null)
            {
                eventHandler(this, new EventArgs());
            }
        }

        public void Close()
        {
            socket.Close();
        }

        public void Send(string message)
        {
            ByteString messageBuffer = new ByteString(message);
            Send(messageBuffer.ToBytes());
        }

        public void Send(byte[] messageBuffer)
        {
            int offset = 0;
            int length = messageBuffer.Length;
            AsyncCallback callback = null;
            callback = delegate(IAsyncResult ar)
                {
                    int sent = socket.EndSend(ar);
                    offset += sent;
                    length -= sent;
                    if (length == 0)
                    {
                        FireEvent(MessageSent);
                    }
                    else
                    {
                        try
                        {
                            socket.BeginSend(messageBuffer, offset, length, SocketFlags.None, callback, null);
                        }
                        catch (SocketException se)
                        {
                            OnSocketError(se);
                        }
                    }
                };
            try
            {
                socket.BeginSend(messageBuffer, offset, length, SocketFlags.None, callback, null);
            }
            catch (SocketException se)
            {
                OnSocketError(se);
            }
        }

        public void Receive(int messageSize)
        {
            byte[] buffer = new byte[messageSize];
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, delegate(IAsyncResult ar)
                {
                    try
                    {
                        int received = socket.EndReceive(ar);
                        if (received == 0)
                        {
                            FireEvent(Disconnected);
                            return;
                        }
                        if (MessageReceived != null)
                        {
                            MessageReceived(this, new ReceiveEventArgs(new ByteString(buffer)));
                        }
                    }
                    catch (SocketException se)
                    {
                       OnSocketError(se);
                    }
                }, null);
        }
    }

    public class ReceiveEventArgs : EventArgs
    {
        public ByteString Message;

        public ReceiveEventArgs(ByteString message)
        {
            Message = message;
        }
    }
}