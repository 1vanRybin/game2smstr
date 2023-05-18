using System.Data;
using Microsoft.Xna.Framework;

namespace MazeEscape;

public class Wall
{
    public Vector2 Position { get; set; }

    public Wall(int x, int y)
    {
        Position = new Vector2(x,y);
    }
}