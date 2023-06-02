using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeEscape;
public partial class MazeEscape : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
   
    public static Player Player { get; set; }
    public static List<Monster> Monsters { get; set; }
    public static Bullet Bullet { get; set; }
    public static Mode CurrentMode { get; set; }
    public static Maze CurrentMaze { get; set; }
    public static List<string> Mazes { get; set; }

    static float turnTimer;
    public MazeEscape()
    {
        Mazes = new() { "Maze1.txt", "Maze2.txt", "Maze3.txt", "Maze4.txt" };
        Content.RootDirectory = "Content";
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = 1800;
        graphics.PreferredBackBufferHeight = 800;
        IsMouseVisible = true;
    }
    public static void StartGame(int level)
    {
        Monsters = new();
        CurrentMaze = new Maze(File.ReadAllText(Mazes[level]));
        CurrentMode = Mode.Menu;
    }
    protected override void Initialize()
    {
        StartGame(0);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        Viewer.Load(this);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (CurrentMode == Mode.Menu)
            
            MenuController.Control();

        if (CurrentMode == Mode.Game)
        {
            turnTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            GameController.ControlPlayer(Player, CurrentMaze, ref turnTimer);
            GameController.ControlBullet(Bullet, CurrentMaze);
            GameController.ControlMonsters(Monsters, CurrentMaze, Player);

            foreach(var monster in Monsters.ToArray())
            {
                if (monster.Health <= 0)
                {
                    monster.Health = 0;
                    Monsters.Remove(monster);
                    CurrentMaze.Remove(monster.Position);
                }
            }
        }

        if (CurrentMode == Mode.Rules)
            if (Keyboard.GetState().IsKeyDown(Keys.X))
                CurrentMode = Mode.Menu;

        if (CurrentMode == Mode.GameOver || CurrentMode == Mode.WinGame)
            GameController.IsNewGame();

        if (CurrentMode == Mode.Exit)
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        spriteBatch.Begin();
        Viewer.Draw(spriteBatch, graphics);
        spriteBatch.End();

        base.Draw(gameTime);
    }
}
