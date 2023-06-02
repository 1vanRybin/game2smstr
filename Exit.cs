using System.Reflection.Metadata.Ecma335;
using Microsoft.Xna.Framework;

namespace MazeEscape;

public class Exit : IMap
{
    public Vector2 Position { get; set; }

    public Exit(int x, int y)
    {
        Position = new Vector2(x, y) * GameController.ElementSize;
    }
}