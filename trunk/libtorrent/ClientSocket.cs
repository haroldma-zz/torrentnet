using System;
using System.Net.Sockets;

namespace torrent.libtorrent
{
    public delegate void ReceiveEventHandler(object sender, ReceiveEventArgs e);
    public delegate void SocketErrorHandler(object sender, SocketException se);
    
    public interface ClientSocket
    {
        event EventHandler ConnectionEstablished;
        event EventHandler MessageSent;
        event EventHandler Disconnected;
        event ReceiveEventHandler MessageReceived;
        event SocketErrorHandler SocketError;
        void Connect();
        void Close();
        void Send(string message);
        void Send(byte[] messageBuffer);
        void Receive(int messageSize);
    }
}