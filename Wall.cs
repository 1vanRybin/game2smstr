using System.Data;
using Microsoft.Xna.Framework;

namespace MazeEscape;

public class Wall : IMap
{
    public Vector2 Position { get; set; }

    public Wall(int x, int y)
    {
        Position = new Vector2(x * Controller.ElementSize, y * Controller.ElementSize);
    }
}