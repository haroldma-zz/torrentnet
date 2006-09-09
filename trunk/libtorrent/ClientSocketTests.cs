using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NUnit.Framework;

namespace torrent.libtorrent
{
    [TestFixture]
    public class ClientSocketTests
    {
        private TestServer server;
        private ClientSocket socket;

        [SetUp]
        public void SetUp()
        {
            server = new TestServer(4000);
            socket = new ClientSocket("localhost", 4000);
        }

        [TearDown]
        public void TearDown()
        {
            server.Close();
            socket.Close();
        }

        [Test]
        public void MakeConnection()
        {
            server.Start();
            bool connected = false;
            socket.ConnectionEstablished += delegate(object sender, EventArgs e)
                {
                    connected = true;
                };
            socket.Connect();
            Wait();
            Assert.IsTrue(connected);
            Assert.AreEqual(1, server.numberOfConnections);
        }

        [Test]
        public void TryConnectToABrokenServer()
        {
            bool errorDetected = false;
            socket.SocketError += delegate(object sender, SocketException se)
                {
                    errorDetected = true;
                };
            socket.Connect();
            Thread.Sleep(1500);
            Assert.IsTrue(errorDetected);
        }

        [Test]
        public void SendMessage()
        {
            Connect();
            bool sent = false;
            socket.MessageSent += delegate(object sender, EventArgs e)
                {
                    sent = true;
                };
            socket.Send("Hello world\n\r");
            Wait();
            Assert.IsTrue(sent);
            Assert.AreEqual("Hello world", server.Receive());
        }
        
        [Test]
        public void SendAMessageAfterDisconnect()
        {
            Connect();
            bool errorDetected = false;
            bool messageSent = false;
            socket.SocketError += delegate(object sender, SocketException se)
                {
                    errorDetected = true;
                };
            
            server.Close();
            Wait();
            socket.Send("Hello World with an error"); // first message gets through
            Wait();
            socket.MessageSent += delegate(object sender, EventArgs se)
                {
                    messageSent = true;
                };
            socket.Send("Hello World with an error");
            Wait();
            Assert.IsTrue(errorDetected);
            Assert.IsFalse(messageSent);
        }
       
        [Test]
        public void ReceiveMessage()
        {
            Connect();
            bool received = false;
            string receivedMessage = null;
            socket.MessageReceived += delegate(object sender, ClientSocket.ReceiveEventArgs e)
                {
                    received = true;
                    receivedMessage = e.Message.ToString();
                };
            socket.Receive(11);
            server.Send("Hello world");
            Wait();
            Assert.IsTrue(received);
            Assert.AreEqual("Hello world", receivedMessage);
        }
        
        [Test]
        public void ReceiveMessageAfterDisconnect()
        {
            Connect();
            bool disconnected = false;
            socket.Disconnected += delegate(object send, EventArgs e)
                {
                    disconnected = true;
                };
            socket.Receive(1024);
            server.Close();
            Wait();
            Assert.IsTrue(disconnected);
        }

        private void Connect()
        {
            server.Start();
            socket.Connect();
            Wait();
        }

        private static void Wait()
        {
            Thread.Sleep(100);
        }

        private class TestServer
        {
            public int numberOfConnections = 0;
            private Socket serverSocket;
            private int port;
            private Socket clientSocket;

            public TestServer(int port)
            {
                this.port = port;
            }

            public void Start()
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                serverSocket.Listen(10);
                serverSocket.BeginAccept(delegate(IAsyncResult result)
                    {
                        clientSocket = serverSocket.EndAccept(result);
                        numberOfConnections++;
                    }, null);
            }

            public void Close()
            {
                if (clientSocket != null)
                {
                    clientSocket.Close();
                }
                if (serverSocket != null)
                {
                    serverSocket.Close();
                }
            }

            public string Receive()
            {
                TextReader reader = new StreamReader(new NetworkStream(clientSocket, false));
                string message = reader.ReadLine();
                reader.Close();
                return message;
            }

            public void Send(string message)
            {
                TextWriter writer = new StreamWriter(new NetworkStream(clientSocket, false));
                writer.Write(message);
                writer.Flush();
                writer.Close();
            }
        }
    }
}