using System;
using System.Net;
using NUnit.Framework;
using torrent.libtorrent.TestMocks;

namespace torrent.libtorrent
{
    [TestFixture]
    public class PeerTests
    {
        private FakeSocket socket;
        private Torrent torrent;
        private Peer peer;

        [SetUp]
        public void Setup()
        {
            socket = new FakeSocket();
            torrent = TorrentTestUtils.CreateMultiFileTorrent();
            peer = new Peer(socket, torrent, PeerId.GetIdForTest());
            socket.response = HandshakeMessageTests.GetTestHandshake();
        }

        [Test]
        public void InitiateConnectionToPeer()
        {
            bool connected = false;
            peer.Connected += delegate
                {
                    connected = true;
                };
            peer.Connect();
            Assert.IsTrue(connected);
        }

        [Test]
        public void PeerSendsHandshakeOnConnection()
        {
            peer.Connect();
            Assert.AreEqual(HandshakeMessageTests.GetTestHandshake(), socket.lastMessage);
        }

        [Test]
        public void PeerCompletesHandshakeWhileMakingConnection()
        {
            bool handshakeReceived = false;
            peer.HandshakeReceived += delegate
                {
                    handshakeReceived = true;
                };
            peer.Connect();
            Assert.IsTrue(handshakeReceived);
        }

        [Test]
        public void PeerCompletesHandshakeCycleOnAcceptingAConnection()
        {
            socket.Connected = true;
            peer = new Peer(socket, PeerId.GetIdForTest());
            peer.HandshakeReceived += delegate(object sender, PeerEventArgs args)
                {
                    peer.Torrent = torrent;
                    Assert.AreSame(peer, sender);
                    Assert.AreEqual(torrent.InfoHash, args.Handshake.InfoHash);
                    Assert.AreEqual(PeerId.GetIdForTest(), args.Handshake.PeerId);
                };
            peer.AcceptHandshake();
            Assert.AreEqual(HandshakeMessageTests.GetTestHandshake(), socket.lastMessage);
        }
    }
}