using System;
using NUnit.Framework;

namespace torrent.libtorrent.TestMocks
{
    internal class FakeSocket : ClientSocket
    {
        public ByteString lastMessage;
        public ByteString response;
        public bool Connected = false;

        public event EventHandler ConnectionEstablished;
        public event EventHandler MessageSent;
        public event EventHandler Disconnected;
        public event ReceiveEventHandler MessageReceived;
        public event SocketErrorHandler SocketError;

        public void Connect()
        {
            Connected = true;
            FireEvent(ConnectionEstablished);
        }

        public void Connect(SocketCallback nextAction)
        {
            Connected = true;
            nextAction.Invoke();
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
            Connected = false;
            FireEvent(Disconnected);
        }

        public void Send(string message)
        {
            Assert.IsTrue(Connected, "Socket was not connected");
            lastMessage = new ByteString(message);
            FireEvent(MessageSent);
        }

        public void Send(byte[] messageBuffer)
        {
            Assert.IsTrue(Connected, "Socket was not connected");
            lastMessage = new ByteString(messageBuffer);
            FireEvent(MessageSent);
        }

        public void Send(byte[] messageBuffer, SocketCallback nextAction)
        {
            Assert.IsTrue(Connected, "Socket was not connected");
            lastMessage = new ByteString(messageBuffer);
            nextAction.Invoke();
        }

        public void Receive(int messageSize)
        {
            Assert.IsTrue(Connected, "Socket was not connected");
            if (MessageReceived != null)
            {
                MessageReceived(this, new ReceiveEventArgs(response));
            }
        }

        public void Receive(int messageSize, ReceiveCallback nextAction)
        {
            Assert.IsTrue(Connected, "Socket was not connected");
            nextAction.Invoke(new ReceiveEventArgs(response));
        }
    }
}