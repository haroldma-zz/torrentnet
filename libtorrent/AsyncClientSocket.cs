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

        public AsyncClientSocket(string host, short port)
        {
            this.host = host;
            this.port = (ushort)port;
        }

        public AsyncClientSocket(Uri uri): this(uri.Host, uri.Port)
        {
            
        }
        
        public AsyncClientSocket(Socket socket)
        {
            this.socket = socket;
        }
        
        private void Connect(Delegate callBack, params object[] args)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.BeginConnect(host, port, delegate(IAsyncResult ar)
                {
                    try
                    {
                        socket.EndConnect(ar);
                        FireEvent(callBack, args);
                    }
                    catch (SocketException se)
                    {
                        FireEvent(SocketError, this, se);
                    }
                }, null);
        }

        public void Connect()
        {
            Connect(ConnectionEstablished, this, new EventArgs());
        }

        public void Connect(SocketCallback nextAction)
        {
            Connect(nextAction as Delegate);
        }

        private void FireEvent(Delegate eventHandler, params object[] args)
        {
            if (eventHandler != null)
            {
                eventHandler.DynamicInvoke(args);
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
            Send(messageBuffer, MessageSent, this, new EventArgs());
        }

        public void Send(byte[] messageBuffer, SocketCallback nextAction)
        {
            Send(messageBuffer, nextAction as Delegate);
        }

        private void Send(byte[] messageBuffer, Delegate eventHandler, params object[] args)
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
                        FireEvent(eventHandler, args);
                    }
                    else
                    {
                        try
                        {
                            socket.BeginSend(messageBuffer, offset, length, SocketFlags.None, callback, null);
                        }
                        catch (SocketException se)
                        {
                            FireEvent(SocketError, this, se);
                        }
                    }
                };
            try
            {
                socket.BeginSend(messageBuffer, offset, length, SocketFlags.None, callback, null);
            }
            catch (SocketException se)
            {
                FireEvent(SocketError, this, se);
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
                            FireEvent(Disconnected, this, new EventArgs());
                            return;
                        }
                        if (MessageReceived != null)
                        {
                            MessageReceived(this, new ReceiveEventArgs(new ByteString(buffer)));
                        }
                    }
                    catch (SocketException se)
                    {
                        FireEvent(SocketError, this, se);
                    }
                }, null);
        }

        public void Receive(int messageSize, ReceiveCallback nextAction)
        {
            byte[] buffer = new byte[messageSize];
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, delegate(IAsyncResult ar)
                {
                    try
                    {
                        int received = socket.EndReceive(ar);
                        if (received == 0)
                        {
                            FireEvent(Disconnected, this, new EventArgs());
                            return;
                        }
                        FireEvent(nextAction, new ReceiveEventArgs(new ByteString(buffer)));
                    }
                    catch (SocketException se)
                    {
                        FireEvent(SocketError, this, se);
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