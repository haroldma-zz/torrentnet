using NUnit.Framework;
using torrent.libtorrent.TestMocks;

namespace torrent.libtorrent
{
    [TestFixture]
    public class ErrorsOnHandshakeTest
    {
        [Test]
        public void SocketCantConnect()
        {
            FakeErrorSocket socket = new FakeErrorSocket();
            socket.ErrorOnConnect = true;
            Peer peer = new Peer(socket, TorrentTestUtils.CreateMultiFileTorrent(), PeerId.GetIdForTest());
            bool connected = false;
            bool error = false;
            peer.Connected += delegate
                {
                    connected = true;
                };
            peer.Error += delegate
                {
                    error = true;
                };
            peer.Connect();
            Assert.IsFalse(connected);
            Assert.IsTrue(error);
        }

        //[Test]
        //public void SocketCantSend()
        //{
        //    FakeErrorSocket socket = new FakeErrorSocket();
        //    socket.ErrorOnSend = true;
        //    Peer peer = new Peer(socket, TorrentTestUtils.CreateMultiFileTorrent(), PeerId.GetIdForTest());
        //    bool connected = false;
        //    bool error = false;
        //    peer.Connected += delegate
        //        {
        //            connected = true;
        //        };
        //    peer.Error += delegate
        //        {
        //            error = true;
        //        };
        //    peer.Connect();
        //    Assert.IsTrue(connected);
        //    Assert.IsTrue(error);
        //}
    }
}