using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using otherworld.States;

namespace otherworld {
    public class Otherworld : Game {

        Texture2D ghostLeft;
        Texture2D ghostRight;

        Vector2 ghostPos;
        float ghostSpeed;

        bool ghostFacingRight;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private State _currentState;
        private State _nextState;

        public void ChangeState(State state) {
            _nextState = state;
        }

        public Otherworld() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {

            ghostPos = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                                   _graphics.PreferredBackBufferHeight / 2);
            ghostSpeed = 125f;
            ghostFacingRight = true;

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _currentState = new MainMenuState(this, _graphics.GraphicsDevice, Content);

            ghostLeft = Content.Load<Texture2D>("ghost-left");
            ghostRight = Content.Load<Texture2D>("ghost-right");

        }

        protected override void Update(GameTime gameTime) {

            if(_nextState != null) {
                _currentState = _nextState;
                _nextState = null;
            }

            _currentState.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.W))
                ghostPos.Y -= ghostSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.A)) {
                ghostPos.X -= ghostSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                ghostFacingRight = false;
            }

            if (kstate.IsKeyDown(Keys.S))
                ghostPos.Y += ghostSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.D)) {
                ghostPos.X += ghostSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                ghostFacingRight = true;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            _currentState.Draw(gameTime, _spriteBatch);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            // _spriteBatch.Begin();
            if (ghostFacingRight) {
                _spriteBatch.Draw(ghostRight, ghostPos, Color.White);
            } else {
                _spriteBatch.Draw(ghostLeft, ghostPos, Color.White);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
