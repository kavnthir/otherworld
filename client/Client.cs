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

        public Vector2 ServerPosition;

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

            _listener.NetworkReceiveEvent += _listener_NetworkReceiveEvent;
        }

        private void _listener_NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod) {
            string InputEvent = reader.GetString(100);
            Debug.WriteLine(InputEvent);


            int startInd = InputEvent.IndexOf("X:") + 2;
            float aXPosition = float.Parse(InputEvent.Substring(startInd, InputEvent.IndexOf(" Y") - startInd));
            startInd = InputEvent.IndexOf("Y:") + 2;
            float aYPosition = float.Parse(InputEvent.Substring(startInd, InputEvent.IndexOf("}") - startInd));

            ServerPosition = new Vector2(aXPosition, aYPosition);

            reader.Recycle();
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

        public void Update() {
            _client.PollEvents();
        }

        public void Stop() { 
            _client.Stop();
        }
    }
}
