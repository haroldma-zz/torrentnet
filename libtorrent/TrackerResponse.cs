using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace torrent.libtorrent
{
    public class TrackerResponse
    {
        private IDictionary responseContent = new Hashtable();
        private List<Peer> peerList = new List<Peer>();
        public TrackerResponse(ByteString responseText)
        {
            HttpResponse response = new HttpResponse(responseText);
            BenDecoder decoder = new BenDecoder(new MemoryStream(response.Content.ToBytes()));
            responseContent = decoder.ReadDictionary();
            
            if(IsSuccessful)
            {
                ByteString peers = responseContent["peers"] as ByteString;
                BinaryReader reader = new BinaryReader(new MemoryStream(peers.ToBytes()));
                for(int i = 0; i<peers.ToBytes().Length; i+=6)
                {
                    peerList.Add(new Peer(reader.ReadBytes(4), reader.ReadInt16()));
                }
            }
        }

        public bool IsSuccessful
        {
            get
            {
                return !responseContent.Contains("failure reason");
            }
        }

        public string FailureReason
        {
            get
            {
                return (responseContent["failure reason"] as ByteString).ToString();
            }
        }

        public int NumberOfSeeds
        {
            get { return Convert.ToInt32(responseContent["complete"]); }
        }

        public int NumberOfLeechers
        {
            get { return Convert.ToInt32(responseContent["incomplete"]); }
        }

        public List<Peer> Peers
        {
            get { return peerList; }
        }
    }
}