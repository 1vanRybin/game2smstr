using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeEscape;

public class Game1 : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    Texture2D playerTexture;
    Player player;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        graphics.PreferredBackBufferWidth = 1200;
        graphics.PreferredBackBufferHeight = 900;
        //IsMouseVisible = true;
    }
    void StartGame()
    {
        player = new Player(15, 15, 100);
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
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        Controller.ControlPlayer(player);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Blue);

        spriteBatch.Begin();
        spriteBatch.Draw(playerTexture, new Rectangle((int)player.Position.X, (int)player.Position.Y,
            50, 50), Color.White);
        spriteBatch.End();

        base.Draw(gameTime);
    }
}
