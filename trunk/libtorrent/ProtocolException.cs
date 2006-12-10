using System;

namespace torrent.libtorrent
{
    internal class ProtocolException:Exception
    {
        public ProtocolException(string s):base(s)
        {
            
        }
    }
}