using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using otherworld.Rendering;
using System.Diagnostics;
using System.IO;
using thisworld;

namespace otherworld {
    class Client {

        private readonly EventBasedNetListener _listener;
        private readonly NetManager _client;

        private readonly string _ip;
        private readonly int _port;
        private readonly string _connectionKey;

        private readonly ContentLoader _contentLoader;

        private WorldState _world;

        private readonly Player _player;

        public Client(string ip, int port, string connectionKey, ContentManager content, GraphicsDevice graphicsDevice, Otherworld game) {
            _listener = new EventBasedNetListener();
            _client = new NetManager(_listener);

            _ip = ip;
            _port = port;
            _connectionKey = connectionKey;

            _contentLoader = new ContentLoader(content);

            _world = new WorldState();
            _player = new Player(0);
        }

        public void Start() {
            _client.Start();
            _client.Connect(_ip, _port, _connectionKey);

            _listener.NetworkReceiveEvent += _listener_NetworkReceiveEvent;
        }

        public void Load() {
            _contentLoader.Load();
        }

        private void _listener_NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod) {

            _world = new WorldState(reader);

            // _player.peerID = peer.RemoteId; determine why this doesnt work
            // this information is needed for client side prediction

            reader.Recycle();
        }

        public void SendInput() {
            var kstate = Keyboard.GetState();
            ClientInputState input = new ClientInputState();
            if (kstate.IsKeyDown(Keys.W)) {
                input.MoveUp = true;
            }
            if (kstate.IsKeyDown(Keys.A)) {
                input.MoveLeft = true;
            }
            if (kstate.IsKeyDown(Keys.S)) {
                input.MoveDown = true;
            }
            if (kstate.IsKeyDown(Keys.D)) {
                input.MoveRight = true;
            }
            if (input == new ClientInputState()) {
                return;
            }

            NetDataWriter writer = new NetDataWriter();
            writer.Put(input.Export());
            NetPeer peer = _client.GetPeerById(0);
            peer.Send(writer, DeliveryMethod.Unreliable);
        }

        public void Update(GameTime gameTime) {
            _client.PollEvents();
            SendInput();
        }

        public void Stop() {
            NetPeer peer = _client.GetPeerById(0);
            peer.Disconnect();
            _client.Stop();
        }

        public void Draw(SpriteBatch spriteBatch) {
            for(int i = 0; i < _world.Entities.Count; i++) {
                if(!(_world.Entities[i] is Player))
                    continue;
                Player player = (Player)_world.Entities[i];
                player.Draw(spriteBatch, _contentLoader);
            }

            // _player.Draw(spriteBatch, _contentLoader);
            // goal would be to not draw server location of player, instead predict
        }
    }
}
