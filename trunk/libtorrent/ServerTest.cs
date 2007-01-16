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
        private Server server;
        private TestIncommingHandler handler;

        [SetUp]
        public void SetUp()
        {
            handler = new TestIncommingHandler();
            server = new Server(handler);
        }

        [TearDown]
        public void TearDown()
        {
            server.Close();
        }

        [Test]
        public void MakeConnection()
        {
            server.StartListeningAt(1234);
            AsyncClientSocket socket = new AsyncClientSocket("localhost", 1234);
            bool connected = false;
            socket.ConnectionEstablished += delegate
                {
                    connected = true;
                };
            socket.Connect();
            Thread.Sleep(100);
            Assert.IsTrue(connected);
        }

        [Test]
        public void HandleIncommingConnections()
        {
            server.StartListeningAt(1234);
            for (int i = 0; i < 10; i++)
            {
                AsyncClientSocket socket = new AsyncClientSocket("localhost", 1234);
                socket.Connect();
            }
            Thread.Sleep(100);
            Assert.AreEqual(10, handler.NumberOfConnections);
        }
    }

    internal class TestIncommingHandler : IncommingConnectionHandler
    {
        public int NumberOfConnections;
        public void HandleConnection(ClientSocket socket)
        {
            NumberOfConnections += 1;
            socket.Close();
        }
    }

    internal interface IncommingConnectionHandler
    {
        void HandleConnection(ClientSocket socket);
    }

    internal class Server
    {
        private Socket socket;
        private IncommingConnectionHandler handler;

        public Server(IncommingConnectionHandler handler)
        {
            this.handler = handler;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void StartListeningAt(int port)
        {
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            socket.Listen(10);
            socket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
        }
        
        private void AcceptCallBack(IAsyncResult result)
        {
            try
            {
                Socket clientSocket = socket.EndAccept(result);
                handler.HandleConnection(new AsyncClientSocket(clientSocket));
                socket.BeginAccept(new AsyncCallback(AcceptCallBack), null);
            }
            catch (ObjectDisposedException e)
            {
                // Ignore, this seems to normal
                // this exception is raised when a socket is accepting connections and the server is shutting down
            }
        }

        public void Close()
        {
            socket.Close();
        }
    }
}
