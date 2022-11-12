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

        private readonly WorldState _world;

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
            // replace private player information with remote player information

            string InputEvent = reader.GetString(100);

            int startInd = InputEvent.IndexOf("X:") + 2;
            float aXPosition = float.Parse(InputEvent.Substring(startInd, InputEvent.IndexOf(" Y") - startInd));
            startInd = InputEvent.IndexOf("Y:") + 2;
            float aYPosition = float.Parse(InputEvent.Substring(startInd, InputEvent.IndexOf("}") - startInd));

            _player.X = aXPosition;
            _player.Y = aYPosition;

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
            MemoryStream ms = input.Export();

            Debug.WriteLine(ms.GetBuffer().ToString());

            writer.Put(ms.ToArray());
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
            _player.Draw(spriteBatch, _contentLoader);
        }
    }
}
