using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeEscape;

public class Player : ICreature
{
    int health;
    public Vector2 Position { get; set; }
    bool isAlive;


    public Player(int x, int y, int health)
    {
        Position = new Vector2(x, y);
        this.health = health;
        isAlive = true;
    }

    public bool CanMove(Maze maze, Vector2 point)
    {
        return !(maze.WallsMap[(int)point.X/Controller.ElementSize,(int)point.Y/ Controller.ElementSize] is Wall);
    }
}
