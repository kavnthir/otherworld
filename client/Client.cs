using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using LiteNetLib;
using LiteNetLib.Utils;

namespace otherworld {
    class Client {

        private int _playerID;

        private EventBasedNetListener _listener;
        private NetManager _client;

        private string _ip;
        private int _port;
        private string _connectionKey;

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

            _listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) => {
                Console.WriteLine("We got: {0}", dataReader.GetString(100 /* max length of string */));
                dataReader.Recycle();
            };

            while (!Console.KeyAvailable) {
                _client.PollEvents();
                Thread.Sleep(15);
            }
        }

        public void Stop() { 
            _client.Stop();
        }
    }
}
