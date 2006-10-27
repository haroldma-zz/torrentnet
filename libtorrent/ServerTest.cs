using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace torrent.libtorrent
{
    [TestFixture]
    public class ServerTest
    {
        [Test]
        public void MakeConnection()
        {
            Server server = new Server();
            server.StartListeningAt(1234);
            AsyncClientSocket socket = new AsyncClientSocket("localhost", 1234);
            bool connected = false;
            socket.ConnectionEstablished += delegate(object sender, EventArgs e)
                {
                    connected = true;
                };
            socket.Connect();
            Thread.Sleep(100);
            Assert.IsTrue(connected);
        }
    }

    internal class Server
    {
        public void StartListeningAt(int port)
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            serverSocket.Listen(10);
            serverSocket.BeginAccept(delegate(IAsyncResult result)
                {
                    Socket clientSocket = serverSocket.EndAccept(result);
                }, null);
        }
    }
}
