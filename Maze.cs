using System;
using System.Drawing.Drawing2D;
using Microsoft.Xna.Framework;

namespace MazeEscape;

public class Maze
{
    public IMap[,] Map { get; set; }
    public readonly int Height;
    public readonly int Width;

    public Maze(string maze)
    {
        Map = CreateMaze(maze);
        Width = Map.GetLength(0) * GameController.ElementSize;
        Height = Map.GetLength(1) * GameController.ElementSize;
    }

    public Maze(Maze maze)
    {
        Map = maze.Map;
        Width = maze.Width;
        Height = maze.Height;
    }

    public IMap GetItem(Vector2 point) =>
            Map[(int)point.X / GameController.ElementSize, (int)point.Y / GameController.ElementSize];
    
    void SetPosition(Vector2 position, IMap item) =>
            Map[(int)position.X / GameController.ElementSize, (int)position.Y / GameController.ElementSize] = item;
    
    public void Add(IMap item, Vector2 position)=>
            SetPosition(position, item);
    
    public void Remove(Vector2 position)=>
            SetPosition(position, null);
    
    public void Rearrange(IMap item, Vector2 nextPoint)
    {
        Remove(item.Position);
        item.Position = nextPoint;
        Add(item, item.Position);
    }
    public bool InBounds(Vector2 point)
    {
        point /= GameController.ElementSize;
        return point.X>0 && point.Y>0 && point.X<Width && point.Y<Height;
    }

    IMap[,] CreateMaze(string maze)
    {
        var rows = maze.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
        var result = new IMap[rows[0].Length, rows.Length];

        for (var x = 0; x < rows[0].Length; x++)
            for (var y = 0; y < rows.Length; y++)
            {
                switch (rows[y][x])
                {
                    case '1':
                        result[x, y] = new Wall(x, y);
                        break;
                    case 'P':
                        var player = new Player(x, y);
                        result[x, y] = player;
                        MazeEscape.Player = player;
                        break;
                    case 'M':
                        var monster = new Monster(x, y);
                        result[x, y] = monster;
                        MazeEscape.Monsters.Add(monster);
                        break;
                    case 'D':
                        result[x, y] = new DoubleMove(x, y);
                        break;
                    case 'E':
                        result[x, y] = new Exit(x, y);
                        break;
                }
            }
        return result;
    }
}