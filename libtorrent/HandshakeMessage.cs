using System;

namespace torrent.libtorrent
{
    class HandshakeMessage
    {
        private Hash infoHash;
        private PeerId peerId;
        public HandshakeMessage(Hash infoHash, PeerId peerId)
        {
            this.infoHash = infoHash;
            this.peerId = peerId;
        }

        public HandshakeMessage(byte[] bytes)
        {
            if (bytes.Length != 68) throw new ProtocolException("Handshake message was too short");
            
            byte pstrlen = bytes[0];
            ByteString pstr = new ByteString(bytes, 1, pstrlen);
            if (pstr.ToString() != "BitTorrent protocol") throw new ProtocolException("Unsuported version of the protocol");
            
            byte[] hashPart = new byte[20];
            ByteString peerIdPart = new ByteString(bytes, 48, 20);
            Array.Copy(bytes, 28, hashPart, 0, 20);
            infoHash = new Hash(hashPart);
            peerId = new PeerId(peerIdPart.ToString());
        }

        public Hash InfoHash
        {
            get { return infoHash; }
        }

        public PeerId PeerId
        {
            get { return peerId; }
        }

        public byte[] ToBytes()
        {
            return (new ByteString(19) + "BitTorrent protocol" + new byte[8] + infoHash.Value + peerId.ToString()).ToBytes();
        }
    }
}