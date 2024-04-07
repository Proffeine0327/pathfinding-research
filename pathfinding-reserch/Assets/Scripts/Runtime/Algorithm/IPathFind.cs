using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PathfindResult
{
    public int dist;
    public float time;
    public List<Point> path;
}

public interface IPathFind
{
    public PathfindResult FindPath(Map map, Vector2Int start, Vector2Int end);
    public void FindPathVisualize(Map map, Vector2Int start, Vector2Int end);
}
