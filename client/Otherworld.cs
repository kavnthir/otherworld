using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace otherworld {
    public class Otherworld : Game {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Client _client;
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
            _client.Load();
        }

        protected override void Update(GameTime gameTime) {
            _client.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _client.Draw(_spriteBatch);
            base.Draw(gameTime);
        }
    }
}
