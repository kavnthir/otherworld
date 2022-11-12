using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using otherworld.Rendering;
using System.Diagnostics;
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

        private readonly PlayerRenderer _playerRenderer;
        private readonly Player _player;

        public enum inputType {
            Up,
            Left,
            Right,
            Down,
        }

        public Client(string ip, int port, string connectionKey, ContentManager content, GraphicsDevice graphicsDevice, Otherworld game) {
            _listener = new EventBasedNetListener();
            _client = new NetManager(_listener);

            _ip = ip;
            _port = port;
            _connectionKey = connectionKey;

            _contentLoader = new ContentLoader(content);

            _playerRenderer = new PlayerRenderer(new Player(0), _contentLoader);

            _world = new WorldState();
        }

        public void Start() {
            _client.Start();
            _client.Connect(_ip, _port, _connectionKey);

            _listener.NetworkReceiveEvent += _listener_NetworkReceiveEvent;
        }

        private void _listener_NetworkReceiveEvent(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod) {
            string InputEvent = reader.GetString(100);

            int startInd = InputEvent.IndexOf("X:") + 2;
            float aXPosition = float.Parse(InputEvent.Substring(startInd, InputEvent.IndexOf(" Y") - startInd));
            startInd = InputEvent.IndexOf("Y:") + 2;
            float aYPosition = float.Parse(InputEvent.Substring(startInd, InputEvent.IndexOf("}") - startInd));

            _playerRenderer.Player.X = aXPosition;
            _playerRenderer.Player.Y = aYPosition;

            reader.Recycle();
        }

        public void SendInput() {
            var kstate = Keyboard.GetState();
            string inputString = "";
            if (kstate.IsKeyDown(Keys.W)) {
                inputString = "Up";
            }
            if (kstate.IsKeyDown(Keys.A)) {
                inputString = "Left";
            }
            if (kstate.IsKeyDown(Keys.S)) {
                inputString = "Right";
            }
            if (kstate.IsKeyDown(Keys.D)) {
                inputString = "Down";
            }
            if (inputString.Equals("")) {
                return;
            }
            NetDataWriter writer = new NetDataWriter();
            writer.Put(inputString);
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
            _playerRenderer.Draw(spriteBatch);
        }
    }
}
