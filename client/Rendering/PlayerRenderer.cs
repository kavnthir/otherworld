using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thisworld;

namespace otherworld.Rendering {
    public static class PlayerRenderer {
        public static void Draw(this Player player, SpriteBatch spriteBatch, ContentLoader contentLoader) {
            spriteBatch.Begin();
            spriteBatch.Draw(contentLoader.GhostRight, new Vector2(player.X, player.Y), Color.White);
            spriteBatch.End();
        }
    }
}
