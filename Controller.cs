using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MazeEscape;

public static class Controller
{
    public const int ElementSize = 50;
    static Dictionary<Keys, Vector2> MovePosition = new()
    {
        {Keys.W, -Vector2.UnitY}, 
        {Keys.S, Vector2.UnitY}, 
        {Keys.A, -Vector2.UnitX}, 
        {Keys.D, Vector2.UnitX}
    };

    static bool IsPlayerMove = true;
    static public void ControlPlayer(Player player, Maze maze)
    {
        var keyState = Keyboard.GetState(); 
        var pressedKey = keyState.GetPressedKeys().FirstOrDefault();

        
        if (MovePosition.ContainsKey(pressedKey) && IsPlayerMove)
        {
            var possiblePoint = player.Position + MovePosition[pressedKey] * ElementSize;
            if (CanMove(maze, possiblePoint))
            {
                player.Position = possiblePoint;
                IsPlayerMove = false;
            }
        }
    }

    static public void ControlMonster(Monster monster, Maze maze, Player player)
    {
        if(!IsPlayerMove)
        {
            IsPlayerMove = true;
            monster.Position = monster.GetNextMove(maze, player) * ElementSize;
        }
    }

    static public bool CanMove(Maze maze, Vector2 point)
    {
        return !(maze.WallsMap[(int)point.X / ElementSize, (int)point.Y / ElementSize] is Wall);
    }
}
