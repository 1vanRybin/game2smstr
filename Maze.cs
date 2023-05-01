using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SharpDX.MediaFoundation;

namespace MazeEscape;

public class Maze
{
    public Wall[,] WallsMap { get; set; }
    public readonly int Height;
    public readonly int Width;

    public Maze()
    {
        WallsMap = CreateMaze();
        Width = WallsMap.GetLength(0) * Controller.ElementSize;
        Height = WallsMap.GetLength(1) * Controller.ElementSize;
    }
    public Wall[,] CreateMaze()
    {
        var rows = maze1.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
        var result = new Wall[rows[0].Length, rows.Length];

        for (var x = 0; x < rows[0].Length; x++)
            for (var y = 0; y < rows.Length; y++)
                if (rows[y][x] == '1')
                    result[x, y] = new Wall(x * Controller.ElementSize, y * Controller.ElementSize);
        return result;
    }

    string maze1 =
      @"111111111111111111
100111111000000001
100000001111110001
111101111000000001
100100000000000001
100100000000000001
100100000000000001
100100001111100001
100100000001110001
100100000000000001
100100000000000001
100000011111111111
100011111000000001
100000000000000001
100111111100000001
111111111111111111";
}