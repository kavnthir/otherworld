using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace otherworld {
    class Player {

        protected ContentManager _content;
        protected GraphicsDevice _graphicsDevice;
        protected Otherworld _game;

        private Client _client;

        public Vector2 Position;

        Texture2D ghostRight;

        public Player(ContentManager content, GraphicsDevice graphicsDevice, Otherworld game, Client client) {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _game = game;

            _client = client;
            Position = new Vector2();
        }

        public void Spawn() {
            _client.SendInput(Client.inputType.Spawn);
            ghostRight = _content.Load<Texture2D>("ghost-right");
        }

        public void Update(GameTime gameTime) {

            var kstate = Keyboard.GetState();

            if(kstate.IsKeyDown(Keys.W))
                _client.SendInput(Client.inputType.Up);

            if (kstate.IsKeyDown(Keys.A))
                _client.SendInput(Client.inputType.Left);

            if(kstate.IsKeyDown(Keys.S)) 
                _client.SendInput(Client.inputType.Down);

            if (kstate.IsKeyDown(Keys.D))
                _client.SendInput(Client.inputType.Right);

            Position = _client.ServerPosition;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

            spriteBatch.Begin();
            spriteBatch.Draw(ghostRight, Position, Color.White);
            spriteBatch.End();

        }
    }
}
