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
public partial class MazeEscape : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
   
    public static Player Player { get; set; }
    public static List<Monster> Monsters { get; set; }
    public static Bullet Bullet { get; set; }
    static Maze maze;
    public static Mode Mode { get; set; }
    static float turnTimer;
    public MazeEscape()
    {
        Content.RootDirectory = "Content";
        Monsters = new();
        maze = new();
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = maze.Width;
        graphics.PreferredBackBufferHeight = maze.Height;
        IsMouseVisible = true;
    }
    public static void StartGame()
    {
        Monsters = new();
        maze = new();
        Mode = Mode.Menu;
    }
    protected override void Initialize()
    {
        StartGame();
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

        if (Mode == Mode.Menu)
            
            MenuController.Control();

        if (Mode == Mode.Game)
        {
            turnTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            GameController.ControlPlayer(Player, maze, ref turnTimer);
            GameController.ControlBullet(Bullet, maze);
            GameController.ControlMonsters(Monsters, maze, Player);

            foreach (var monster in Monsters.ToArray())
            {
                if (monster.Position == Player.Position)
                {
                    Player.Health -= monster.Damage;
                    monster.Health = 0;
                }

                if (monster.Health <= 0)
                {
                    monster.Health = 0;
                    Monsters.Remove(monster);
                    maze.Remove(monster.Position);
                }
            }
        }

        if (Mode == Mode.Rules)
            if (Keyboard.GetState().IsKeyDown(Keys.X))
                Mode = Mode.Menu;

        if (Mode == Mode.GameOver)
            GameController.GameOver();

        if (Mode == Mode.Exit)
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
