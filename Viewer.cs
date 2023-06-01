using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeEscape;
public partial class MazeEscape
{
    public static class Viewer
    {
        static Texture2D playerTexture, monsterTexture, wallTexture, bulletTexture;
        static SpriteFont font;
        static string rulesText;
        public static void Load(Game game)
        {
            playerTexture = game.Content.Load<Texture2D>("Assets/player");
            wallTexture = game.Content.Load<Texture2D>("Assets/maze");
            monsterTexture = game.Content.Load<Texture2D>("Assets/monster");
            bulletTexture = game.Content.Load<Texture2D>("Assets/bullet");
            font = game.Content.Load<SpriteFont>("Assets/Text");
            rulesText = File.ReadAllText("Rules.txt");
        }
        public static void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            if (Mode == Mode.Menu)
            {
                spriteBatch.DrawString(font, "Start Game", new Vector2(graphics.PreferredBackBufferWidth / 2 - 75, graphics.PreferredBackBufferHeight / 2 - 25), Color.White);
                spriteBatch.DrawString(font, "Rules", new Vector2(graphics.PreferredBackBufferWidth / 2 - 75, graphics.PreferredBackBufferHeight / 2 + 25), Color.White);
                spriteBatch.DrawString(font, "Exit", new Vector2(graphics.PreferredBackBufferWidth / 2 - 75, graphics.PreferredBackBufferHeight / 2 + 75), Color.White);
                if (MenuController.State == MenuState.StartGame)
                    spriteBatch.DrawString(font, ">", new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, graphics.PreferredBackBufferHeight / 2 - 25), Color.Green);
                if (MenuController.State == MenuState.Rules)
                    spriteBatch.DrawString(font, ">", new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, graphics.PreferredBackBufferHeight / 2 + 25), Color.Green);
                if (MenuController.State == MenuState.Exit)
                    spriteBatch.DrawString(font, ">", new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, graphics.PreferredBackBufferHeight / 2 + 75), Color.Green);
            }
            if (Mode == Mode.Game)
            {
                spriteBatch.Draw(playerTexture, new Rectangle((int)Player.Position.X, (int)Player.Position.Y,
                    GameController.ElementSize, GameController.ElementSize), Color.White);
                if(Bullet!=null)
                    spriteBatch.Draw(bulletTexture, new Rectangle((int)Bullet.Position.X + GameController.ElementSize/2, (int)Bullet.Position.Y + GameController.ElementSize / 2,
                        Bullet.Size, Bullet.Size), Color.White);
                //foreach (var monster in Monsters)
                //{
                //    spriteBatch.Draw(monsterTexture, new Rectangle((int)monster.Position.X, (int)monster.Position.Y,
                //        GameController.ElementSize, GameController.ElementSize), Color.White);
                //}
                foreach (var element in maze.Map)
                {
                    if (element is Wall)
                        spriteBatch.Draw(wallTexture, new Rectangle((int)element.Position.X, (int)element.Position.Y,
                            GameController.ElementSize, GameController.ElementSize), Color.White);
                    if(element is Monster)
                        spriteBatch.Draw(monsterTexture, new Rectangle((int)element.Position.X, (int)element.Position.Y,
                       GameController.ElementSize, GameController.ElementSize), Color.White);
                }

                if(Player.IsHavePick)
                    for(int i = 0; i < 4; i++) 
                        spriteBatch.DrawString(font, "You have a pick!", new Vector2(graphics.PreferredBackBufferWidth /20 + i, graphics.PreferredBackBufferHeight /100 + i/2), Color.PaleVioletRed);
            }
            if (Mode == Mode.Rules)
            {
                spriteBatch.DrawString(font, rulesText, new Vector2(graphics.PreferredBackBufferWidth / 100, graphics.PreferredBackBufferHeight / 25), Color.ForestGreen);
                spriteBatch.DrawString(font, "X to back", new Vector2(graphics.PreferredBackBufferWidth - 150, graphics.PreferredBackBufferHeight  - 50), Color.IndianRed);
            }

            if (Mode == Mode.GameOver)
            {
                spriteBatch.DrawString(font, "Game Over", new Vector2(graphics.PreferredBackBufferWidth / 2 - 75, graphics.PreferredBackBufferHeight / 2 - 25), Color.Red);
                spriteBatch.DrawString(font, "Retry? (X - yes, esc - no)", new Vector2(graphics.PreferredBackBufferWidth / 2 - 150, graphics.PreferredBackBufferHeight / 2 + 75), Color.White);
            }
        }
    }
}

