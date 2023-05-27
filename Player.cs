using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeEscape;

public class Player : IMap
{
    public int Health { get; set; }
    public Vector2 Position { get; set; }
    public bool IsAlive { get; set; }


    public Player(int x, int y)
    {
        Position = new Vector2(x * Controller.ElementSize, y*Controller.ElementSize);
        IsAlive = true;
        Health = 100;
    }
}
