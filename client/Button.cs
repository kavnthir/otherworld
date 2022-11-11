using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace otherworld {
    class Button : UIComponent {

        private SpriteFont _font;
        private Texture2D _texture;
        private bool _isHovering;
        private MouseState _prevMouse;
        private MouseState _currentMouse;

        public Vector2 Position { get; set; }
        public string Text { get; set; }
        public event EventHandler OnClick;
        public Rectangle ButtonRectangle{
            get {
                return new Rectangle((int) Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        } 

        public Button(Texture2D texture, SpriteFont font) {
            _texture = texture;
            _font = font;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

            var color = Color.White;

            if(_isHovering)
                color = Color.Gray;

            spriteBatch.Draw(_texture, ButtonRectangle, color);

            if(!string.IsNullOrEmpty(Text)) {
                var x = (ButtonRectangle.X + (ButtonRectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (ButtonRectangle.Y + (ButtonRectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), Color.Black);
            }
        }

        public override void Update(GameTime gameTime) {

            _prevMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            _isHovering = false;

            Rectangle MouseRect = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            if(MouseRect.Intersects(ButtonRectangle)) {
                _isHovering = true;
            }

            if(_isHovering && _currentMouse.LeftButton == ButtonState.Released && _prevMouse.LeftButton == ButtonState.Pressed) {
                OnClick.Invoke(this, new EventArgs());
            }
        }
    }
}
