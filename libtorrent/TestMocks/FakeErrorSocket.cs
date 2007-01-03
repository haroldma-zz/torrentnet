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


        public bool ErrorOnConnect, ErrorOnSend;
        
        public void Connect()
        {
            GenerateError(ErrorOnConnect, ConnectionEstablished, new EventArgs());
        }

        public void Connect(SocketCallback nextAction)
        {
            GenerateError(ErrorOnConnect, ConnectionEstablished, new EventArgs());
        }

        private void GenerateError(bool createError, Delegate handler, object args)
        {
            if(createError && SocketError != null)
            {
                SocketError(this, new SocketException(4));
            }
            else if(!createError && handler != null)
            {
                handler.DynamicInvoke(this, args);
            }
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Send(string message)
        {
            GenerateError(ErrorOnSend, MessageSent, new EventArgs());
        }

        public void Send(byte[] messageBuffer)
        {
            GenerateError(ErrorOnSend, MessageSent, new EventArgs());
        }

        public void Send(byte[] mesageBuffer, SocketCallback nextAction)
        {
            throw new NotImplementedException();
        }

        public void Receive(int messageSize)
        {
            throw new NotImplementedException();
        }

        public void Receive(int messageSize, ReceiveCallback nextAction)
        {
            throw new NotImplementedException();
        }
    }
}