using System;
using Microsoft.Xna.Framework;
using System.IO;

namespace MazeEscape;

public class Maze
{
    public IMap[,] Map { get; set; }
    public readonly int Height;
    public readonly int Width;

    public Maze()
    {
        Map = CreateMaze();
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
    

    IMap[,] CreateMaze()
    {
        var rows = maze1.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
        var result = new IMap[rows[0].Length, rows.Length];

        for (var x = 0; x < rows[0].Length; x++)
            for (var y = 0; y < rows.Length; y++)
            {
                if (rows[y][x] == '1')
                    result[x, y] = new Wall(x, y);
                if (rows[y][x] == 'P')
                {
                    var player = new Player(x, y);
                    result[x, y] = player;
                    MazeEscape.Player = player;
                }

                if (rows[y][x] == 'M')
                {
                    var monster = new Monster(x, y);
                    result[x, y] = monster;
                    MazeEscape.Monsters.Add(monster);
                }
            }
        return result;
    }

    string maze1 = File.ReadAllText("Maze1.txt");
      
}