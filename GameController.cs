using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace MazeEscape;

public static class GameController
{
    public const int ElementSize = 50;
    public const float TurnTime = 100;

    static Dictionary<Keys, Vector2> MovePosition = new()
    {
        {Keys.W, -Vector2.UnitY}, 
        {Keys.S, Vector2.UnitY}, 
        {Keys.A, -Vector2.UnitX}, 
        {Keys.D, Vector2.UnitX}
    };

    static bool IsPlayerMove = true;
    static public void ControlPlayer(Player player, Maze maze, ref float turnTimer)
    {
        if (player.Health <= 0)
        {
            player.Health = 0;
            MazeEscape.Mode = Mode.GameOver;
            return;
        }
        var mouseState = Mouse.GetState();
        var keyState = Keyboard.GetState();
        var pressedKey = keyState.GetPressedKeys().FirstOrDefault();
        var mousePos = Vector2.Floor(mouseState.Position.ToVector2() / ElementSize) * ElementSize;

        if (mouseState.LeftButton == ButtonState.Pressed && player.IsHavePick && !CanMove(maze, mousePos))
        {
            maze.Remove(mousePos);
            player.IsHavePick = false;
        }

        if (IsPlayerMove && turnTimer >= TurnTime)
        {
            turnTimer = 0.0f;
            if (MovePosition.ContainsKey(pressedKey))
            {
                var possiblePoint = player.Position + MovePosition[pressedKey] * ElementSize;
                if (CanMove(maze, possiblePoint))
                {
                    player.Position = possiblePoint;
                    IsPlayerMove = false;
                }
            }

            if (mouseState.RightButton == ButtonState.Pressed && player.Bullets > 0 &&
            (mousePos.X == player.Position.X || mousePos.Y == player.Position.Y))
            {
                MazeEscape.Bullet = new(player.Position, mousePos);
                IsPlayerMove = false;
                player.Bullets--;
            }
        }
    }

    static public void ControlBullet(Bullet bullet, Maze maze)
    {
        if (bullet != null)
        {
            bullet.Position += bullet.SpeedDirection;
            if (!CanMove(maze, bullet.Position))
                MazeEscape.Bullet = null;

            if (maze.GetItem(bullet.Position) is Monster monster)
                monster.Health -= Bullet.Damage;
            
        }
    }

    static public void ControlMonsters(List<Monster> monsters, Maze maze, Player player)
    {

        if (!IsPlayerMove)
        {
            Parallel.ForEach(monsters, monster =>
            {
                maze.Remove(monster.Position);
                monster.Position = monster.GetNextMove(maze, player) * ElementSize;
                maze.Add(monster, monster.Position);
            }
            );
            Task.WaitAll();
            IsPlayerMove = true;
        }
    }

    static public bool CanMove(Maze maze, Vector2 point)=>
                    maze.GetItem(point) is not IObstacle;
    
    static public void GameOver()
    {
        KeyboardState keyState = Keyboard.GetState();
        if (keyState.IsKeyDown(Keys.X))
        {
            MazeEscape.StartGame();
        }
    }
}
