using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using torrent.libtorrent;

namespace PeerConnect
{
    class Program
    {
        static void Main(string[] args)
        {
            Torrent torrent = new Torrent(BenDecoder.Decode(args[0]));
            Tracker trackerClient = new Tracker(torrent);
            Console.WriteLine("Connecting to tracker at {0}", torrent.AnnounceUri);
            object cv = new object();
            trackerClient.Connected += delegate
                {
                    Console.WriteLine("Connected to {0}", torrent.AnnounceUri);
                };
            trackerClient.Updated += delegate
                {
                    if(trackerClient.LastResponse.IsSuccessful)
                    {
                        Console.WriteLine("{0} Seeders, {1} leechers", trackerClient.LastResponse.NumberOfSeeds, trackerClient.LastResponse.NumberOfLeechers);
                        ConnectToPeers(trackerClient.LastResponse.Peers, torrent);
                    }
                    else
                    {
                        QuitWithError(cv, trackerClient.LastResponse.FailureReason);
                    }
                   
                };
            trackerClient.Error += delegate(object sender, Exception e)
                {
                    QuitWithError(cv, e.Message);
                };
            trackerClient.Start();
            lock(cv)
            {
                Monitor.Wait(cv);
            }
        }

        private static void QuitWithError(object cv, string message)
        {
            Console.WriteLine("Error: {0}", message);
            Monitor.PulseAll(cv);
        }

        private static void ConnectToPeers(List<PeerInfo> peers, Torrent torrent)
        {
            foreach(PeerInfo peerInfo in peers)
            {
                Peer remotePeer = new Peer(peerInfo, torrent, PeerId.GetDefaultPeerId());
                Console.WriteLine("Connecting to {0}:{1}", peerInfo.IpAddress, peerInfo.Port);
                remotePeer.Connected += delegate(object sender, EventArgs args)
                    {
                        Peer peer = sender as Peer;
                        Console.WriteLine("Connected to peer at {0} on port {1}", peer.Info.IpAddress, (ushort)peer.Info.Port);
                    };
                remotePeer.HandshakeReceived += delegate(object sender, PeerEventArgs args)
                    {
                        Peer peer = sender as Peer;
                        Console.WriteLine("Received handshake from {0}", peer.Info.IpAddress);
                    };
                remotePeer.Error += delegate(object sender, PeerEventArgs args)
                    {
                        Peer peer = sender as Peer;
                        System.Diagnostics.Debug.WriteLine(string.Format("Error from {0}: {1}", peer.Info.IpAddress, args.Exception.Message));
                    };
                remotePeer.Connect();
            }
        }
    }
}
