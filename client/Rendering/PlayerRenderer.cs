using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thisworld;

namespace otherworld.Rendering {
    public class PlayerRenderer {

        public Player Player;
        private Texture2D _texture;

        public PlayerRenderer(Player player, ContentLoader contentLoader) {
            Player = player;
            _texture = contentLoader.GhostRight;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Begin();
            spriteBatch.Draw(_texture, new Vector2(Player.X, Player.Y), Color.White);
            spriteBatch.End();
        }
    }
}
