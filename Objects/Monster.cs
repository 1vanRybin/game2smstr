using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MazeEscape;

public class Monster : IMap, IObstacle
{
    public Vector2 Position { get; set; }
    public int Damage { get; set; }
    public int Health { get; set; }
    
    public Monster(int x, int y)
    {
        Position = new Vector2(x * GameController.ElementSize, y * GameController.ElementSize);
        Health = 100;
        Damage = 50;
    }

    enum State
    {
        Empty,
        Wall,
        Visited
    };

    List<Vector2> _deltas = new List<Vector2>()
    {
        -Vector2.UnitX,
        Vector2.UnitX,
        -Vector2.UnitY,
        Vector2.UnitY
    };

    public Vector2 GetNextMove(Maze maze, Player player)
    {
        var pointConnections = new Dictionary<Vector2, Vector2>();

        var map = new State[maze.Map.GetLength(0), maze.Map.GetLength(1)];

        FillStateMap(map, maze);

        var queue = new Queue<Vector2>();
        queue.Enqueue(player.Position / GameController.ElementSize);
        return DoBFS(pointConnections, map, queue);
    }

    Vector2 DoBFS(Dictionary<Vector2, Vector2> pointConnections, State[,] map, Queue<Vector2> queue)
    {
        while (queue.Count != 0)
        {
            var point = queue.Dequeue();

            if (point == Position / GameController.ElementSize)
            {
                return pointConnections.ContainsKey(point) ? pointConnections[point] : Vector2.Zero;
            }

            if (point.X < 0 || point.X >= map.GetLength(0) || point.Y < 0 || point.Y >= map.GetLength(1)) continue;
            map[(int)point.X, (int)point.Y] = State.Visited;


            foreach (var delta in _deltas)
            {
                if (map[(int)(point.X + delta.X), (int)(point.Y + delta.Y)] != State.Empty) continue;

                else
                {
                    var nextPoint = point + delta;
                    queue.Enqueue(nextPoint);
                    if (!pointConnections.ContainsKey(nextPoint))
                        pointConnections.Add(nextPoint, point);
                    else pointConnections[nextPoint] = point;
                }
            }
        }


        return Vector2.Zero;
    }

    void FillStateMap(State[,] map, Maze maze)
    {
        for (int x = 0; x < map.GetLength(0); x++)
            for (int y = 0; y < map.GetLength(1); y++)
                map[x, y] = maze.Map[x, y] is Wall ? State.Wall : State.Empty;
    }
}