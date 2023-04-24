using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeEscape;

public class Player
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

}
