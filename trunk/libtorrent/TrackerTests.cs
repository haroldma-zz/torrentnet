using System;
using System.Net.Sockets;
using NUnit.Framework;
using torrent.libtorrent.TestMocks;

namespace torrent.libtorrent
{
    namespace TestMocks
    {
    }

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
            FakeErrorSocket fakeSocket = new FakeErrorSocket();
            fakeSocket.ErrorOnConnect = true;
            tracker = new Tracker(file, fakeSocket);
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
        public static PeerId GetIdForTest()
        {
            return new PeerId("12345678901234567890");
        }

        private string id;
        public PeerId(ByteString id):this(id.ToString())
        {
            
        }
        public PeerId(string id)
        { 
            this.id = id;
        }
        
        public override string ToString()
        {
            return id;
        }
        
        public override bool Equals(object o)
        {
            if(o is PeerId)
            {
                return id.Equals((o as PeerId).id);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}