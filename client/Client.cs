using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace otherworld {
    class Client {

        private readonly EventBasedNetListener _listener;
        private readonly NetManager _client;

        private readonly string _ip;
        private readonly int _port;
        private readonly string _connectionKey;

        public enum inputType { 
            Spawn,
            Up,
            Left,
            Right,
            Down,
        }

        private inputType _currentInput;

        public Client(string ip, int port, string connectionKey) {
            _listener = new EventBasedNetListener();
            _client = new NetManager(_listener);

            _ip = ip;
            _port = port;
            _connectionKey = connectionKey;
        }

        public void Start() {
            _client.Start();
            _client.Connect(_ip, _port , _connectionKey);
        }

        public void SendInput(inputType input) {
            _currentInput = input;

            Debug.WriteLine("sending input ", input);

            NetDataWriter writer = new NetDataWriter();
            string inputString = "";
            if(_currentInput == Client.inputType.Spawn) {
                inputString = "Spawn Player";
            } else if(_currentInput == Client.inputType.Up) {
                inputString = "Up";
            } else if(_currentInput == Client.inputType.Left) {
                inputString = "Left";
            } else if(_currentInput == Client.inputType.Right) {
                inputString = "Right";
            } else if(_currentInput == Client.inputType.Down) {
                inputString = "Down";
            }
            writer.Put(inputString);
            NetPeer peer = _client.GetPeerById(0);
            Debug.WriteLine(peer.ConnectionState.ToString());
            peer.Send(writer, DeliveryMethod.Unreliable);         

        }
        public Vector2 RecievePosition() {
            _listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) => {
                // Console.WriteLine("We got: {0}", dataReader.GetString(100 /* max length of string */));
                // dataReader.Recycle();
            };
            return new Vector2();
        }

        public void Update() {
            _client.PollEvents();
        }

        public void Stop() { 
            _client.Stop();
        }
    }
}
