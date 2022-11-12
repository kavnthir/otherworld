using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using otherworld.States;

namespace otherworld {
    public class Otherworld : Game {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private State _currentState;
        private State _nextState;

        private Client _client;

        public void ChangeState(State state) {
            _nextState = state;
        }

        public Otherworld() {
            _graphics = new GraphicsDeviceManager(this);
            _client = new Client("localhost", 9050, "pass", Content, _graphics.GraphicsDevice, this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            _client.Start();
            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _currentState = new MainMenuState(this, _graphics.GraphicsDevice, Content);
            _client.Load();
        }

        protected override void Update(GameTime gameTime) {

            if (_nextState != null) {
                _currentState = _nextState;
                _nextState = null;
            }
            _currentState.Update(gameTime);
            _client.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _currentState.Draw(gameTime, _spriteBatch);
            _client.Draw(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}
