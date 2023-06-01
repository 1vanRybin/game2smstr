using Microsoft.Xna.Framework;

namespace MazeEscape;

public class Bullet : IMap
{
    public Vector2 Position { get; set; }
    public Vector2 Target { get; set; }
    public Vector2 SpeedDirection { get; set; }

    public const int Damage = 100;
    public const int Size = 20;
    public const float Speed = 5f;
    public Bullet(Vector2 pos, Vector2 target)
    {
        Position = pos;
        target.Round();
        Target = target;
        SpeedDirection = Vector2.Normalize(target - pos) * Speed; 
    }
}
