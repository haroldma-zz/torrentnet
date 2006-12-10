using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Windows.Forms;
using torrent.libtorrent;

namespace TrackerProbe
{
    public partial class TrackerProbe : Form
    {
        Torrent torrent;
        Tracker tracker;
        
        public TrackerProbe()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenTorrent();
            ShowTorrentInfo();
            ConnectToTracker();
        }

        private void ConnectToTracker()
        {
            if(tracker != null)
            {
                tracker.Close();
            }
            tracker = new Tracker(torrent);
            tracker.Error += new Tracker.TrackerErrorHandler(tracker_Error);
            trackerStatus.Text = "Connecting...";
            tracker.Connected += delegate(object sender, EventArgs arg)
                {
                    MethodInvoker invoker = new MethodInvoker(delegate ()
                        {
                            trackerStatus.Text = "Updating...";
                        });
                    Invoke(invoker);
                };
            tracker.Updated += delegate(object sender, EventArgs arg)
               {
                   MethodInvoker invoker = new MethodInvoker(delegate()
                       {
                           if (tracker.LastResponse.IsSuccessful)
                           {
                               trackerStatus.Text = "OK";
                               swarmSize.Text = string.Format("Peers: {0}     Seeds: {1}", tracker.LastResponse.NumberOfLeechers, tracker.LastResponse.NumberOfSeeds);
                               UpdatePeerList();
                           }
                           else
                           {
                               trackerStatus.Text = string.Format("Error: {0}", tracker.LastResponse.FailureReason);
                           }
                           
                       });
                   Invoke(invoker);
               };
            tracker.Start();
        }

        private void tracker_Error(object sender, Exception e)
        {
            MethodInvoker invoker = new MethodInvoker(delegate ()
                {
                    trackerStatus.Text = string.Format("Error: {0}", e.Message);
                });
            Invoke(invoker);
        }

        private void UpdatePeerList()
        {
            peerList.BeginUpdate();
            peerList.Items.Clear();
            foreach(PeerInfo peer in tracker.LastResponse.Peers)
            {
                ListViewItem peerEntry = new ListViewItem(peer.IpAddress.ToString());
                peerEntry.SubItems.Add(((ushort)peer.Port).ToString());
                peerList.Items.Add(peerEntry);
            }
            peerList.EndUpdate();
        }

        private void ShowTorrentInfo()
        {
            trackerUrl.Text = torrent.AnnounceUri.ToString();
            numberOfPieces.Text = torrent.Pieces.Count.ToString();
            pieceLength.Text = torrent.PieceLength.ToString();
            hash.Text = torrent.InfoHash.ToHexString();
            peerList.Items.Clear();
        }

        private void OpenTorrent()
        {
            OpenFileDialog openTorrent = new OpenFileDialog();
            if(openTorrent.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                torrent = new Torrent(BenDecoder.Decode(openTorrent.FileName));
            }
        }
    }
}