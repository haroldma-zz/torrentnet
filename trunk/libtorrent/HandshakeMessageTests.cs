using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace torrent.libtorrent
{
    [TestFixture]
    public class HandshakeMessageTests
    {
        [Test]
        public void CreateHandshakeMessage()
        {
            HandshakeMessage message = new HandshakeMessage(TorrentTestUtils.CreateMultiFileTorrent().InfoHash, PeerId.GetIdForTest());
            Assert.AreEqual(GetHandshake(19, "BitTorrent protocol"), new ByteString(message.ToBytes()));
            Assert.AreEqual(68, message.ToBytes().Length);
        }

        [Test]
        public void ParseWholeHandshackeMessage()
        {
            HandshakeMessage message = new HandshakeMessage(GetHandshake(19, "BitTorrent protocol").ToBytes());
            Assert.AreEqual(TorrentTestUtils.CreateMultiFileTorrent().InfoHash, message.InfoHash);
            Assert.AreEqual(PeerId.GetIdForTest(), message.PeerId);
        }
        
        [Test]
        [ExpectedException(typeof(ProtocolException))]
        public void WrongMessageLength()
        {
            HandshakeMessage message = new HandshakeMessage(new byte[10]);
        }
        
        [Test]
        [ExpectedException(typeof(ProtocolException))]
        public void WrongHeader()
        {
            HandshakeMessage message = new HandshakeMessage(GetHandshake(19, "BitTorrent gtotohol").ToBytes());
        }

        public static ByteString GetHandshake(byte pstrlen, string indentifier)
        {
            ByteString pstr = new ByteString(indentifier);
            byte[] reserved = new byte[8];
            return new ByteString(pstrlen) + pstr + reserved + TorrentTestUtils.CreateMultiFileTorrent().InfoHash.Value + PeerId.GetIdForTest().ToString();
        }
        
        public static ByteString GetTestHandshake()
        {
            return GetHandshake(19, "BitTorrent protocol");
        }
    }
}
