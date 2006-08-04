using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace torrent.libtorrent
{
    interface IConnection
    {
        void SendRequest(WebRequest request);
    }
}
