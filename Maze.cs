using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SharpDX.MediaFoundation;

namespace MazeEscape;

public class Maze
{
    public IMap[,] WallsMap { get; set; }
    public readonly int Height;
    public readonly int Width;

    public Maze()
    {
        WallsMap = CreateMaze();
        Width = WallsMap.GetLength(0) * Controller.ElementSize;
        Height = WallsMap.GetLength(1) * Controller.ElementSize;
    }
    public Maze(Maze maze)
    {
        WallsMap = maze.WallsMap;
        Width = maze.Width;
        Height = maze.Height;
    }
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

    string maze1 =
      @"111111111111111111
10P111111000000001
100000001111110001
111101111000000001
100100000000000001
100100000000000001
100100000000000001
100100001111100001
100100000001110001
1001000000M0000001
100100000000000001
100000011111111111
100011111000M00001
100000000000000001
100111111100000001
111111111111111111";
}