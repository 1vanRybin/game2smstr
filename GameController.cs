using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MazeEscape;

public static class GameController
{
    public const int ElementSize = 50;
    public const float TurnTime = 100;
    static int currentLvl = 0;
    static bool IsPlayerMove = true;
    static Skill ActiveSkill;
    static bool IsActiveSkill;

    static readonly Dictionary<Keys, Vector2> MovePosition = new()
    {
        {Keys.W, -Vector2.UnitY}, 
        {Keys.S, Vector2.UnitY}, 
        {Keys.A, -Vector2.UnitX}, 
        {Keys.D, Vector2.UnitX}
    };
    // расширяемо, можно добавлять скиллы и кнопки их активации
    static readonly Dictionary<Skill, Action> Skills = new()
    {
        { Skill.DoubleMove, ()=>IsPlayerMove = true }
    };

    static readonly Dictionary<Keys, Skill> KeysForSkills = new()
    {
        { Keys.E, Skill.DoubleMove }
    };

    static public void ControlPlayer(Player player, Maze maze, ref float turnTimer)
    {
        if (player.Health <= 0)
        {
            player.Health = 0;
            MazeEscape.CurrentMode = Mode.GameOver;
            return;
        }

        var mouseState = Mouse.GetState();
        var keyState = Keyboard.GetState();
        var pressedKey = keyState.GetPressedKeys().FirstOrDefault();
        var mousePos = Vector2.Floor(mouseState.Position.ToVector2() / ElementSize) * ElementSize;

        if (mouseState.LeftButton == ButtonState.Pressed && player.IsHavePick &&
            maze.GetItem(mousePos) is Wall && maze.InBounds(mousePos))
        {
            maze.Remove(mousePos);
            player.IsHavePick = false;
        }

        if (KeysForSkills.ContainsKey(pressedKey) && player.Skills.Contains(KeysForSkills[pressedKey]))
        {
            ActiveSkill = KeysForSkills[pressedKey];
            IsActiveSkill = true;
        }
    
        if (IsPlayerMove && turnTimer >= TurnTime)
        {
            turnTimer = PlayerMoveCycle(player, maze, mouseState, pressedKey, mousePos);
        }

    }

    static float PlayerMoveCycle(Player player, Maze maze, MouseState mouseState, Keys pressedKey, Vector2 mousePos)
    {
        float turnTimer = 0.0f;
        if (MovePosition.ContainsKey(pressedKey))
        {
            NextPlayerTurn(player, maze, pressedKey);
            if (IsActiveSkill)
            {
                player.Skills.Remove(ActiveSkill);
                Skills[ActiveSkill].Invoke();
                IsActiveSkill = false;
            }
        }

        if (mouseState.RightButton == ButtonState.Pressed && player.Bullets > 0 &&
        (mousePos.X == player.Position.X || mousePos.Y == player.Position.Y))
        {
            MazeEscape.Bullet = new(player.Position, mousePos);
            IsPlayerMove = false;
            player.Bullets--;
        }

        return turnTimer;
    }

    static void NextPlayerTurn(Player player, Maze maze, Keys pressedKey)
    {
        var possiblePoint = player.Position + MovePosition[pressedKey] * ElementSize;
        if (CanMove(maze, possiblePoint))
        {
            if (maze.GetItem(possiblePoint) is Exit)
            {
                currentLvl++;
                if (currentLvl < MazeEscape.Mazes.Count)
                    MazeEscape.CurrentMaze = new Maze(File.ReadAllText(MazeEscape.Mazes[currentLvl]));
                else
                {
                    currentLvl = 0;
                    MazeEscape.CurrentMode = Mode.WinGame;
                }
            }

            if(maze.GetItem(possiblePoint) is ISkill skill)
            {
                maze.Remove(possiblePoint);
                player.Skills.Add(skill.Description);
            }

            maze.Rearrange(player, possiblePoint);
            IsPlayerMove = false;
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
            var rnd = new Random();
            for (int i = 1; i <= rnd.Next(1, 3); i++)
            {
                Parallel.ForEach(monsters, monster =>
                {
                    var nextMove = monster.GetNextMove(maze, player) * ElementSize;
                    if (nextMove == player.Position)
                    {
                        player.Health -= monster.Damage;
                        monster.Health = 0;
                    }
                    else
                        maze.Rearrange(monster, nextMove);
                });
                Task.WaitAll();
            }
            IsPlayerMove = true;
        }
    }

    static public bool CanMove(Maze maze, Vector2 point)=>
                    maze.GetItem(point) is not IObstacle;
    
    static public void IsNewGame()
    {
        KeyboardState keyState = Keyboard.GetState();
        if (keyState.IsKeyDown(Keys.X))
            MazeEscape.StartGame(currentLvl);
    }
}
