using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace otherworld {
    public class ContentLoader {

        public Texture2D GhostRight;

        private ContentManager _content;

        public ContentLoader(ContentManager content) {
            _content = content;
            Load();
        }

        public void Load() {
            GhostRight = _content.Load<Texture2D>("ghost-right");
        }

    }
}
