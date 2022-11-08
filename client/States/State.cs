using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace otherworld.States {
    public abstract class State {

        protected ContentManager _content;
        protected GraphicsDevice _graphicsDevice;
        protected Otherworld _game;
        public State(Otherworld game, GraphicsDevice graphicsDevice, ContentManager content) {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _content = content;
        }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);

    }
}
