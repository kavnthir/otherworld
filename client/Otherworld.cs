using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using otherworld.States;

namespace otherworld {
    public class Otherworld : Game {

        Texture2D ghostRight;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private State _currentState;
        private State _nextState;

        private Client _client;
        private Player _player;

        public void ChangeState(State state) {
            _nextState = state;
        }

        public Otherworld() {
            _graphics = new GraphicsDeviceManager(this);
            _client = new Client("localhost", 9050, "pass");
            _player = new Player(Content, _graphics.GraphicsDevice, this, _client);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            _client.Start();
            _player.Spawn();
            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _currentState = new MainMenuState(this, _graphics.GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime) {

            if(_nextState != null) {
                _currentState = _nextState;
                _nextState = null;
            }
            _currentState.Update(gameTime);

            // if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    _client.Stop();
            //    Exit();

            _client.Update();
            _player.Update(gameTime);

            //var kstate = Keyboard.GetState();

            //if (kstate.IsKeyDown(Keys.W))
            //    ghostPos.Y -= ghostSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //if (kstate.IsKeyDown(Keys.A)) {
            //    ghostPos.X -= ghostSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    ghostFacingRight = false;
            //}

            //if (kstate.IsKeyDown(Keys.S))
            //    ghostPos.Y += ghostSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //if (kstate.IsKeyDown(Keys.D)) {
            //    ghostPos.X += ghostSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    ghostFacingRight = true;
            //}

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            _currentState.Draw(gameTime, _spriteBatch);
            _player.Draw(gameTime, _spriteBatch);

            // world drawing 
            GraphicsDevice.Clear(Color.CornflowerBlue);


            base.Draw(gameTime);
        }
    }
}
