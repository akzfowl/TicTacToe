using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TicTacToe
{
    public enum Entry { Empty, Circle, Cross }

    public class Cell
    {
        public Vector2 position;
        public Texture2D buttonTex;
        public Entry entry;
        public bool set;

        public Cell(Vector2 startPos, ContentManager content)
        {
            position = startPos;
            buttonTex = content.Load<Texture2D>(@"empty");
            entry = Entry.Empty;
            set = false;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(buttonTex, new Rectangle((int)position.X, (int)position.Y, 128, 128), Color.White);
        }
    }
}
