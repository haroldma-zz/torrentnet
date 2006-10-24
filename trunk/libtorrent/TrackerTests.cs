using System;
using System.Net.Sockets;
using NUnit.Framework;

namespace torrent.libtorrent
{
    [TestFixture]
    public class TrackerTests
    {
        private Torrent file;
        private FakeSocket socket;
        private Tracker tracker;

        [SetUp]
        public void SetUp()
        {
            socket = new FakeSocket();
            file = TorrentTestUtils.CreateMultiFileTorrent();
            socket.response = TrackerResponseTest.CreateTestResponseString();

        }
        
        [Test]
        public void SendStartedMessage()
        {
            tracker = new Tracker(file, socket);
            bool connected = false;
            tracker.Connected += delegate(object sender, EventArgs e)
                {
                    connected = true;
                };
            ByteString expectedMessage = new ByteString("GET /announce?info_hash=%cdt%feF%cc%a3%a0%e6%dc_%f7%1c%bd%82%82ljZ%5d%3e&" +
                                                        "peer_id=12345678901234567890&port=6881&compact=1&uploaded=0&downloaded=0&left=3706908089&event=started HTTP/1.1\r\n\r\n");
            tracker.Start();
            Assert.IsTrue(socket.Connected);
            Assert.IsTrue(connected);
            Assert.AreEqual(expectedMessage, socket.lastMessage);
        }

        [Test]
        public void ReceiveResponse()
        {
            tracker = new Tracker(file, socket);
            bool responseReceived = false;
            tracker.Updated += delegate(object sender, EventArgs e)
                {
                    responseReceived = true;
                    Assert.IsTrue(tracker.LastResponse.IsSuccessful);
                };
            tracker.Start();
            Assert.IsTrue(responseReceived);
        }
        
        [Test]
        public void HandleError()
        {
            tracker = new Tracker(file, new FakeErrorSocket());
            bool errorReceived = false;
            tracker.Error += delegate(object sender, Exception se)
                {
                    errorReceived = true;
                    Assert.IsNotNull(se);
                };
            tracker.Start();
            Assert.IsTrue(errorReceived);
        }
        
        [Test]
        public void ClosingTrackerClosesSocket()
        {
            tracker = new Tracker(file, socket);
            tracker.Start();
            Assert.IsTrue(socket.Connected);
            tracker.Close();
            Assert.IsFalse(socket.Connected);
        }
    }

    internal class PeerId
    {
        public static string GetId()
        {
            return "12345678901234567890";
        }
    }

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
            lastMessage = new ByteString(message);
            FireEvent(MessageSent);
        }

        public void Send(byte[] messageBuffer)
        {
            lastMessage = new ByteString(messageBuffer);
            FireEvent(MessageSent);
        }

        public void Receive(int messageSize)
        {
            if (MessageReceived != null)
            {
                MessageReceived(this, new ReceiveEventArgs(response));
            }
        }
    }
}