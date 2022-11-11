using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace otherworld {
    abstract class UIComponent {
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    }
}
