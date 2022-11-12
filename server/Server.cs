using System;
using System.IO;
using System.Threading;
using LiteNetLib;
using LiteNetLib.Utils;
using ProtoBuf;
using thisworld;

namespace otherworld_server {
    public class Server {

        private readonly int _port;
        private readonly int _maxConnections;
        private readonly WorldState _world;

        private readonly EventBasedNetListener _listener;
        private readonly NetManager _server;

        private readonly string _connectionKey;

        public Server(int port, int maxConnections) {
            _port = port;
            _maxConnections = maxConnections;

            _listener = new EventBasedNetListener();
            _server = new NetManager(_listener);

            _world = new WorldState();

            _connectionKey = "pass";
        }

        public void Start() {
            _server.Start(_port);
            Console.WriteLine("Listening on port: {0}", _port);
            _listener.ConnectionRequestEvent += _listener_ConnectionRequestEvent;
            _listener.PeerConnectedEvent += _listener_PeerConnectedEvent;
            _listener.NetworkReceiveEvent += _listener_NetworkReceiveEvent;
        }

        private void _listener_ConnectionRequestEvent(ConnectionRequest request) {
            if(_server.ConnectedPeersCount < _maxConnections) {
                request.AcceptIfKey(_connectionKey);
                return;
            }
            request.Reject();
        }

        private void _listener_PeerConnectedEvent(NetPeer peer) {
            Console.WriteLine("We got connection: {0}", peer.EndPoint);
            _world.Entities.Add(new Player(peer.Id));
            // NetDataWriter writer = new NetDataWriter();
            // writer.Put(peer.Id);
            // peer.Send(writer, DeliveryMethod.ReliableOrdered);         
        }

        public void Update() {
            for (var i = 0; i < _world.Entities.Count; i++) {
                if(!(_world.Entities[i] is Player))
                    continue;
                Player player = (Player)_world.Entities[i];
                if (_server.GetPeerById(player.peerID).ConnectionState == ConnectionState.Disconnected) {
                    _world.Entities.RemoveAt(i);
                    i--;
                    Console.WriteLine("Disconnected: {0}", _server.GetPeerById(player.peerID).EndPoint);
                } else {
                    NetDataWriter writer = new NetDataWriter();
                    writer.Put(_world.Export());
                    NetPeer peer = _server.GetPeerById(player.peerID);
                    peer.Send(writer, DeliveryMethod.Unreliable);         
                }
            }

            _server.PollEvents();
            Thread.Sleep(15);
        }

        private void _listener_NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod) {

            ClientInputState input = new ClientInputState(reader);

            for(int i = 0; i < _world.Entities.Count; i++) {
                if(!(_world.Entities[i] is Player))
                    continue;

                Player player = (Player)_world.Entities[i];
                if (player.peerID == peer.Id) {
                    player.UpdatePosition(input);
                }
            }
        }

        public void Stop() {
            _server.Stop();
        }

    }
}
