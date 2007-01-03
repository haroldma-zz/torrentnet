using System;
using System.Threading;
using NUnit.Framework;
using torrent.libtorrent.TestMocks;

namespace torrent.libtorrent
{
    [TestFixture]
    public class PeerThreadedTests
    {
        [Test]
        public void PeerWaitsToSendHandshakeBeforeReceiving()
        {
            TimingSocket socket = new TimingSocket();
            socket.response = HandshakeMessageTests.GetTestHandshake();
            Peer peer = new Peer(socket, TorrentTestUtils.CreateMultiFileTorrent(), PeerId.GetIdForTest());
            peer.Connect();
        }

        private class TimingSocket: FakeSocket
        {
            public bool waitCompleted = false;

            new public void Send(byte[] mesageBuffer, SocketCallback nextAction)
            {
                Thread t = new Thread(delegate()
                    {
                        Thread.Sleep(100);
                        waitCompleted = true;
                        nextAction.Invoke();
                    });
                t.Start();
            }

            new public void Receive(int messageSize, ReceiveCallback nextAction)
            {
                Assert.IsTrue(waitCompleted);
                nextAction.Invoke(new ReceiveEventArgs(HandshakeMessageTests.GetTestHandshake()));
            }
        }
    }
}