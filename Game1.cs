using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeEscape;

public class Game1 : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    Texture2D playerTexture;
    Texture2D monsterTexture;
    Texture2D wallTexture;
    Player player;
    List<Monster> monsters;
    Maze maze = new();

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        graphics.PreferredBackBufferWidth = maze.Width;
        graphics.PreferredBackBufferHeight = maze.Height;
        //IsMouseVisible = true;
    }
    void StartGame()
    {
        player = new Player(1, 1);
        monsters = new List<Monster>()
        { new Monster(5, 5)};
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
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        Controller.ControlPlayer(player, maze);
        
        foreach (var monster in monsters)
            Controller.ControlMonster(monster, maze, player);
       
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Blue);

        spriteBatch.Begin();
        spriteBatch.Draw(playerTexture, new Rectangle((int)player.Position.X, (int)player.Position.Y,
            Controller.ElementSize, Controller.ElementSize), Color.White);
        foreach(var monster in monsters)
        {
            spriteBatch.Draw(monsterTexture, new Rectangle((int)monster.Position.X, (int)monster.Position.Y,
                Controller.ElementSize, Controller.ElementSize), Color.White);
        }
        foreach (var wall in maze.WallsMap)
            if(wall!=null)
                spriteBatch.Draw(wallTexture, new Rectangle((int)wall.Position.X, (int)wall.Position.Y,
                    Controller.ElementSize, Controller.ElementSize), Color.White);
        spriteBatch.End();

        base.Draw(gameTime);
    }
}
