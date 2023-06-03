using Microsoft.Xna.Framework;

namespace MazeEscape;

public class DoubleMove : IMap, ISkill
{
    public Vector2 Position { get; set; }
    public Skill Description { get => Skill.DoubleMove; set  => _ = Skill.DoubleMove; }

    public DoubleMove(int x, int y)
    {
        Position = new Vector2(x, y) * GameController.ElementSize;
    }
}