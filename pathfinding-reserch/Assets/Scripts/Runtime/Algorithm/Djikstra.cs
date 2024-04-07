using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Djikstra : IPathFind
{
    public PathfindResult FindPath(Map map, Vector2Int start, Vector2Int end)
    {
        PriorityQueue<Point, float> cloest = new();
        cloest.Enqueue(map[start], 0);

        Point current;
        do
        {
            current = cloest.Dequeue();
            TrySetup(map[current.Coordinate + Vector2Int.up], current, cloest);
            TrySetup(map[current.Coordinate + Vector2Int.down], current, cloest);
            TrySetup(map[current.Coordinate + Vector2Int.left], current, cloest);
            TrySetup(map[current.Coordinate + Vector2Int.right], current, cloest);
        }
        while(cloest.Count != 0);


        return new PathfindResult();
    }

    public void FindPathVisualize(Map map, Vector2Int start, Vector2Int end)
    {

    }

    private void TrySetup(Point point, Point current, PriorityQueue<Point, float> cloest)
    {
        if(point.IsJoined) return;

        point.Cost = current.Cost + 1;
        point.Parent = current;
        cloest.Enqueue(point, point.Cost);
    }
}