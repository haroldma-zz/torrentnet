using System;
using System.Net.Sockets;

namespace torrent.libtorrent.TestMocks
{
    internal class FakeErrorSocket : ClientSocket
    {
        public event EventHandler ConnectionEstablished;
        public event EventHandler MessageSent;
        public event EventHandler Disconnected;
        public event ReceiveEventHandler MessageReceived;
        public event SocketErrorHandler SocketError;

        public void Connect()
        {
            if(SocketError != null)
            {
                SocketError(this, new SocketException(4));
            }
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Send(string message)
        {
            throw new NotImplementedException();
        }

        public void Send(byte[] messageBuffer)
        {
            throw new NotImplementedException();
        }

        public void Receive(int messageSize)
        {
            throw new NotImplementedException();
        }
        
    }
}