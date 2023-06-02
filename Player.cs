using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MazeEscape;

public class Player : IMap
{
    public int Health { get; set; }
    public Vector2 Position { get; set; }
    public List<Skill> Skills { get; set; }
    public bool IsHavePick { get; set; }
    public int Bullets { get; set; }

    public Player(int x, int y)
    {
        Position = new Vector2(x, y) * GameController.ElementSize;
        IsHavePick = true;
        Health = 100;
        Skills = new();
        Bullets = 5;
    }
}


