using System;
using System.Net.Sockets;

namespace torrent.libtorrent
{
    public delegate void ReceiveEventHandler(object sender, ReceiveEventArgs e);
    public delegate void SocketErrorHandler(object sender, SocketException se);
    public delegate void ReceiveCallback(ReceiveEventArgs e);
    public delegate void SocketCallback();
    
    public interface ClientSocket
    {
        event EventHandler ConnectionEstablished;
        event EventHandler MessageSent;
        event EventHandler Disconnected;
        event ReceiveEventHandler MessageReceived;
        event SocketErrorHandler SocketError;
        void Connect();
        void Connect(SocketCallback nextAction);
        void Close();
        void Send(string message);
        void Send(byte[] messageBuffer);
        void Send(byte[] messageBuffer, SocketCallback nextAction);
        void Receive(int messageSize);
        void Receive(int messageSize, ReceiveCallback nextAction);
    }
}