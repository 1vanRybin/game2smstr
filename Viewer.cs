using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MazeEscape;
public partial class MazeEscape
{
    public static class Viewer
    {
        static readonly Dictionary<Mode, Action<SpriteBatch, GraphicsDeviceManager>> drawMode = new()
        {
            {Mode.Menu, (sb, gr) => DrawMenuMode(sb, gr) },
            {Mode.Game, (sb, gr) => DrawGameMode(sb, gr) },
            {Mode.Rules, (sb, gr) => DrawRulesMode(sb, gr) },
            {Mode.GameOver, (sb, gr) => DrawGameOverMode(sb, gr) },
            {Mode.WinGame, (sb, gr) =>  DrawWinMode(sb, gr)}
        };

        static Dictionary<Type, Texture2D> textures = new();

        static Texture2D playerTexture, monsterTexture, wallTexture, bulletTexture, exitTexture, winTexture;
        static SpriteFont font;
        static string rulesText;
        public static void Load(Game game)
        {
            playerTexture = game.Content.Load<Texture2D>("Assets/player");
            wallTexture = game.Content.Load<Texture2D>("Assets/maze");
            monsterTexture = game.Content.Load<Texture2D>("Assets/monster");
            bulletTexture = game.Content.Load<Texture2D>("Assets/bullet");
            exitTexture = game.Content.Load<Texture2D>("Assets/exit");
            winTexture = game.Content.Load<Texture2D>("Assets/wingame");
            font = game.Content.Load<SpriteFont>("Assets/Text");
            rulesText = File.ReadAllText("Rules.txt");

            textures.Add(typeof(Player), playerTexture);
            textures.Add(typeof(Wall), wallTexture);
            textures.Add(typeof(Monster), monsterTexture);
            textures.Add(typeof(Bullet), bulletTexture);    
            textures.Add(typeof(Exit), exitTexture);

        }
        public static void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            drawMode[CurrentMode].Invoke(spriteBatch, graphics);
        }

        private static void DrawGameOverMode(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.DrawString(font, "Game Over", new Vector2(graphics.PreferredBackBufferWidth / 2 - 75, graphics.PreferredBackBufferHeight / 2 - 25), Color.Red);
            spriteBatch.DrawString(font, "Retry? (X - yes, esc - no)", new Vector2(graphics.PreferredBackBufferWidth / 2 - 150, graphics.PreferredBackBufferHeight / 2 + 75), Color.White);
        }

        private static void DrawRulesMode(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.DrawString(font, rulesText, new Vector2(graphics.PreferredBackBufferWidth / 100, graphics.PreferredBackBufferHeight / 25), Color.ForestGreen);
            spriteBatch.DrawString(font, "X to back", new Vector2(graphics.PreferredBackBufferWidth - 150, graphics.PreferredBackBufferHeight - 50), Color.IndianRed);
        }

        private static void DrawMenuMode(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
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

        private static void DrawGameMode(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {

            if (Bullet != null)
                spriteBatch.Draw(bulletTexture, new Rectangle((int)Bullet.Position.X + GameController.ElementSize / 2, (int)Bullet.Position.Y + GameController.ElementSize / 2,
                    Bullet.Size, Bullet.Size), Color.White);
            foreach (var element in CurrentMaze.Map)
            {
                if (element is not null)
                    spriteBatch.Draw(textures[element.GetType()], new Rectangle((int)element.Position.X, (int)element.Position.Y,
                        GameController.ElementSize, GameController.ElementSize), Color.White);
            }

            for (int i = 0; i < 4; i++)
            {
                spriteBatch.DrawString(font, "You have " + Player.Bullets.ToString() + " bullets!", new Vector2(graphics.PreferredBackBufferWidth / 1.2f + i, graphics.PreferredBackBufferHeight / 100), Color.GreenYellow);
                if (Player.IsHavePick)
                    spriteBatch.DrawString(font, "You have a pick!", new Vector2(graphics.PreferredBackBufferWidth / 20 + i, graphics.PreferredBackBufferHeight / 100 + i / 2), Color.GreenYellow);
            }

        }

        private static void DrawWinMode(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(winTexture, new Rectangle(graphics.PreferredBackBufferWidth / 2 - 100, graphics.PreferredBackBufferHeight/ 2 - 100,
                    200, 200), Color.White);
            spriteBatch.DrawString(font, "You Win!", new Vector2(graphics.PreferredBackBufferWidth / 2 - 75, graphics.PreferredBackBufferHeight / 2 + 150), Color.Aquamarine);
        }
    }
}
