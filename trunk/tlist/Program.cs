using System;
using System.Collections.Generic;
using System.Text;
using torrent.libtorrent;
using System.IO;

namespace tlist
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = args[0];
            BenDecoder decoder = new BenDecoder(File.OpenRead(filename));
            Torrent torrent = new Torrent(decoder.ReadDictionary());
            Console.WriteLine("Tracker: {0}", torrent.AnnounceUri);
            Console.WriteLine("Number of pieces: {0}", torrent.Pieces.Count);
            Console.WriteLine("Piece length: {0}", torrent.PieceLength);
            Console.WriteLine("Hash: {0}", torrent.Hash.ToHexString());
            Console.WriteLine("Files:");
            foreach (ITorrentFile file in torrent.Files)
            {
                Console.WriteLine("  Name: {0}, Size: {1}", file.Name, file.Length);
            }
        }
    }
}
