using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace otherworld.States {
    public class MainMenuState : State {

        private List<UIComponent> _components;
        public MainMenuState(Otherworld game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content) {
            var buttonTexture = _content.Load<Texture2D>("Controls/button-1");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            Button connectButton = new Button(buttonTexture, buttonFont) {
                Position = new Vector2(300, 200),
                Text = "connect",
            };

            Button quitButton = new Button(buttonTexture, buttonFont) {
                Position = new Vector2(300, 250),
                Text = "quit",
            };

            connectButton.OnClick += ConnectButton_Click;
            quitButton.OnClick += QuitButton_OnClick;

            _components = new List<UIComponent>() {
                connectButton,
                quitButton
            };

        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Begin();

            foreach(var component in _components)
                component.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            foreach(var component in _components)
                component.Update(gameTime);
        }
        private void ConnectButton_Click(object sender, EventArgs e) {
            // change game state to playing

            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));

            // connect client to server

        }

        private void QuitButton_OnClick(object sender, EventArgs e) {
            _game.Exit();
        }


    }
}
