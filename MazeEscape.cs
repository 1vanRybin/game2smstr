using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
using SharpDX.XInput;
using static System.Net.Mime.MediaTypeNames;

namespace MazeEscape;

public class MazeEscape : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    Texture2D playerTexture, monsterTexture, wallTexture;
    SpriteFont text;
    public static Player Player { get; set; }
    public static List<Monster> Monsters { get; set; }
    static Maze maze;
    public static string mode { get; set; }

    public MazeEscape()
    {
        Content.RootDirectory = "Content";
        Monsters = new();
        maze = new();
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = maze.Width;
        graphics.PreferredBackBufferHeight = maze.Height;
        //IsMouseVisible = true;
    }
    public static void StartGame()
    {
        mode = "Menu";
       
    }
    protected override void Initialize()
    {
        StartGame();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        playerTexture = Content.Load<Texture2D>("Assets/player");
        wallTexture = Content.Load <Texture2D>("Assets/wall");
        monsterTexture = Content.Load<Texture2D>("Assets/monster");
        text = Content.Load<SpriteFont>("Assets/Text");
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (mode == "Menu")
            if(MenuController.Control())
                Exit();
        if (mode == "Game")
        {
            Controller.ControlPlayer(Player, maze);
            Controller.ControlMonsters(Monsters, maze, Player);
            foreach(var monster in Monsters)
            {
                if (monster.Position == Player.Position)
                    mode = "Game Over";
            }
        }

        if (mode == "Game Over")
            Controller.GameOver();

        
       
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        spriteBatch.Begin();
        if (mode == "Menu")
        {
            spriteBatch.DrawString(text, "Start Game", new Vector2(graphics.PreferredBackBufferWidth / 2 - 75, graphics.PreferredBackBufferHeight / 2 - 25), Color.White);
            spriteBatch.DrawString(text, "Shop", new Vector2(graphics.PreferredBackBufferWidth / 2 - 75, graphics.PreferredBackBufferHeight / 2 + 25), Color.White);
            spriteBatch.DrawString(text, "Exit", new Vector2(graphics.PreferredBackBufferWidth / 2 - 75, graphics.PreferredBackBufferHeight / 2 + 75), Color.White);
            if (MenuController.State == MenuState.StartGame)
                spriteBatch.DrawString(text, ">", new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, graphics.PreferredBackBufferHeight / 2 - 25), Color.White);
            if (MenuController.State == MenuState.Shop)
                spriteBatch.DrawString(text, ">", new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, graphics.PreferredBackBufferHeight / 2 + 25), Color.White);
            if (MenuController.State == MenuState.Exit)
                spriteBatch.DrawString(text, ">", new Vector2(graphics.PreferredBackBufferWidth / 2 - 100, graphics.PreferredBackBufferHeight / 2 + 75), Color.White);
        }
        if (mode == "Game")
        {
            spriteBatch.Draw(playerTexture, new Rectangle((int)Player.Position.X, (int)Player.Position.Y,
            Controller.ElementSize, Controller.ElementSize), Color.White);
            foreach (var monster in Monsters)
            {
                if (monster.IsAlive)
                    spriteBatch.Draw(monsterTexture, new Rectangle((int)monster.Position.X, (int)monster.Position.Y,
                        Controller.ElementSize, Controller.ElementSize), Color.White);
            }
            foreach (var wall in maze.WallsMap)
                if (wall is Wall)
                    spriteBatch.Draw(wallTexture, new Rectangle((int)wall.Position.X, (int)wall.Position.Y,
                        Controller.ElementSize, Controller.ElementSize), Color.White);
        }
        if (mode == "Game Over")
        {
            spriteBatch.DrawString(text, "Game Over", new Vector2(graphics.PreferredBackBufferWidth / 2 - 75, graphics.PreferredBackBufferHeight / 2 - 25), Color.Red);
                
            spriteBatch.DrawString(text, "Retry?", new Vector2(graphics.PreferredBackBufferWidth / 2 - 75, graphics.PreferredBackBufferHeight / 2 + 75), Color.White);

        }
        
        spriteBatch.End();

        base.Draw(gameTime);
    }
}
