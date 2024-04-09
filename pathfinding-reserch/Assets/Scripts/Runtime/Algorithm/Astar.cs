using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar : BasePathFind
{
    private PathFinder pathFinder => PathFinder.Instance;
    private Map map => Map.Instance;

    protected override IEnumerator FindPathRoutine(Vector2Int start, Vector2Int end)
    {
        PriorityQueue<Point, float> pq = new();
        pq.Enqueue(map[start], 0);
        map[start].G = 0;

        Point current;
        do
        {
            do
            {
                current = pq.Dequeue();
                if (current.IsJoined) current = null;
            }
            while (current == null);
            current.IsJoined = true;
            if (current.Coord != start && current.Coord != end)
                current.Color = Color.blue;
            yield return new WaitForSeconds(pathFinder.WaitTime);
            if (current.Coord == end) break;

            CheckPoint(current.Coord + Vector2Int.up, current.Coord, end, pq);
            yield return new WaitForSeconds(pathFinder.WaitTime);
            CheckPoint(current.Coord + Vector2Int.down, current.Coord, end, pq);
            yield return new WaitForSeconds(pathFinder.WaitTime);
            CheckPoint(current.Coord + Vector2Int.left, current.Coord, end, pq);
            yield return new WaitForSeconds(pathFinder.WaitTime);
            CheckPoint(current.Coord + Vector2Int.right, current.Coord, end, pq);
            yield return new WaitForSeconds(pathFinder.WaitTime);
        }
        while (pq.Count > 0);

        if (map[end].IsJoined)
        {
            var back = map[end];
            while (back.Parent != null)
            {
                back = back.Parent;
                if (back.Coord != start)
                    back.Color = Color.magenta;
            }
        }
    }

    private void CheckPoint(Vector2Int target, Vector2Int parent, Vector2Int end, PriorityQueue<Point, float> pq)
    {
        if (!map.ContainsCoord(target)) return;
        if (map[target].IsJoined) return;
        if (map[target].PointType == PointType.Wall) return;

        var g = map[parent].G + 1;
        var h = Vector2.Distance(end, target) * 1.25f;
        // var h = Mathf.Abs(end.x - target.x) + Mathf.Abs(end.y - target.y);

        if (map[target].G + h > g + h)
        {
            map[target].G = g;
            map[target].Parent = map[parent];
            if (target != end)
                map[target].Color = Color.yellow;
            pq.Enqueue(map[target], g + h);
        }
    }
}