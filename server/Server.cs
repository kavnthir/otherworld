using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using LiteNetLib;
using LiteNetLib.Utils;

namespace otherworld_server {
    public class Server {

        private readonly int _port;
        private readonly int _maxConnections;

        private readonly EventBasedNetListener _listener;
        private readonly NetManager _server;

        private readonly string _connectionKey;

        public Server(int port, int maxConnections) {
            _port = port;
            _maxConnections = maxConnections;

            _listener = new EventBasedNetListener();
            _server = new NetManager(_listener);

            _connectionKey = "pass";
        }

        public void Start() {
            _server.Start(_port);

            Console.WriteLine("Listening on port: {0}", _port); 

            _listener.ConnectionRequestEvent += request => {
                if (_server.ConnectedPeersCount < _maxConnections) {
                    request.AcceptIfKey(_connectionKey);
                } else {
                    request.Reject();
                }
            };

            _listener.PeerConnectedEvent += peer => {
                Console.WriteLine("We got connection: {0}", peer.EndPoint); 
                NetDataWriter writer = new NetDataWriter();                
                writer.Put("Hello client!");                               
                peer.Send(writer, DeliveryMethod.ReliableOrdered);         
            };

            while (!Console.KeyAvailable) {
                _server.PollEvents();
                Thread.Sleep(15);
            }

        }

        public void Stop() {
            _server.Stop();
        }

    }
}
