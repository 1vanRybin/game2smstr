using Microsoft.Xna.Framework;

namespace MazeEscape;

public class DoubleMove : IMap
{
    public Vector2 Position { get; set; }

    public DoubleMove(int x, int y)
    {
        Position = new Vector2(x, y);
    }
}