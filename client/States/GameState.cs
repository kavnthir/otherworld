﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace otherworld.States {
    class GameState : State {
        public GameState(Otherworld game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content) {


        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime) {
            throw new NotImplementedException();
        }
    }
}
