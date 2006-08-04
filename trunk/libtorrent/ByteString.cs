using System;
using System.Collections.Generic;
using System.Text;

namespace torrent.libtorrent
{
    class ByteString
    {
        private byte[] bytes;
        private string stringValue;

        public ByteString(byte[] bytes)
        {
            this.bytes = bytes;
            stringValue = Encoding.Default.GetString(bytes);
        }

        public ByteString(string value)
        {
            stringValue = value;
            bytes = Encoding.Default.GetBytes(value);
        }

        public override string ToString()
        {
            return stringValue;
        }

        public byte[] ToBytes()
        {
            return bytes;
        }
    }
}
