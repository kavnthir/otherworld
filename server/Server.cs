using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using LiteNetLib;
using LiteNetLib.Utils;
using thisworld;

namespace otherworld_server {
    public class Server {

        private readonly int _port;
        private readonly int _maxConnections;
        private readonly List<Player> _players;

        private readonly EventBasedNetListener _listener;
        private readonly NetManager _server;

        private readonly string _connectionKey;

        public Server(int port, int maxConnections) {
            _port = port;
            _maxConnections = maxConnections;

            _listener = new EventBasedNetListener();
            _server = new NetManager(_listener);

            _players = new List<Player>();

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
            if (_server.ConnectedPeersCount < _maxConnections) {
                request.AcceptIfKey(_connectionKey);
            } else {
                request.Reject();
            }
        }

        private void _listener_PeerConnectedEvent(NetPeer peer) {
            Console.WriteLine("We got connection: {0}", peer.EndPoint);
            _players.Add(new Player(peer.Id));

            //NetDataWriter writer = new NetDataWriter();                
            //writer.Put("Hello client!");                               
            //peer.Send(writer, DeliveryMethod.ReliableOrdered);         
        }

        public void Update() {
            for (var i = 0; i < _players.Count; i++) {
                var player = _players[i];
                if (_server.GetPeerById(player.peerID).ConnectionState == ConnectionState.Disconnected) {
                    _players.RemoveAt(i);
                    i--;
                    Console.WriteLine("Disconnected: {0}", _server.GetPeerById(player.peerID).EndPoint);
                } else {
                    player.UpdateClient(_server);
                }
            }

            _server.PollEvents();
            Thread.Sleep(15);
        }

        private void _listener_NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod) {
            Player.inputType input = Player.inputType.None;
            string InputEvent = reader.GetString();
            Console.WriteLine(InputEvent);
            for (int i = 0; i < _players.Count; i++) {
                if (_players[i].peerID == peer.Id) {
                    switch (InputEvent) {
                        case "Up":
                            input = Player.inputType.Up;
                            break;
                        case "Left":
                            input = Player.inputType.Left;
                            break;
                        case "Right":
                            input = Player.inputType.Right;
                            break;
                        case "Down":
                            input = Player.inputType.Down;
                            break;
                    }
                    _players[i].UpdatePosition(input);
                }
            }
        }


        public void Stop() {
            _server.Stop();
        }

    }
}
