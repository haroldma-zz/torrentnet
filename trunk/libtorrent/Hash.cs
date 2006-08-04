using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace torrent.libtorrent
{
    public class Hash
    {
        private byte[] hash;
        public Hash(byte[] data)
        {
            hash = SHA1.Create().ComputeHash(data);
        }
        public byte[] Value
        {
            get { return hash; }
        }

        public string ToHexString()
        {
            StringBuilder builder = new StringBuilder(40);
            foreach (byte value in hash)
            {
                builder.AppendFormat("{0:X2}", value);
            }
            return builder.ToString();
        }
    }
}
