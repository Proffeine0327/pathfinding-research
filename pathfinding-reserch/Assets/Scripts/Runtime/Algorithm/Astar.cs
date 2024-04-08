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
        map[start].Cost = 0;

        Point current;
        do
        {
            do
            {
                current = pq.Dequeue();
                if (current.IsJoined) current = null;
            }
            while (current == null);
            if (current.Coord != start && current.Coord != end)
                current.Color = Color.blue;
            yield return new WaitForSeconds(pathFinder.WaitTime);
            current.IsJoined = true;

            if (current.Coord == end) break;

            CheckPoint(start, end, current.Coord + Vector2Int.up, current, pq);
            yield return new WaitForSeconds(pathFinder.WaitTime);
            CheckPoint(start, end, current.Coord + Vector2Int.down, current, pq);
            yield return new WaitForSeconds(pathFinder.WaitTime);
            CheckPoint(start, end, current.Coord + Vector2Int.left, current, pq);
            yield return new WaitForSeconds(pathFinder.WaitTime);
            CheckPoint(start, end, current.Coord + Vector2Int.right, current, pq);
            yield return new WaitForSeconds(pathFinder.WaitTime);
        }
        while (pq.Count != 0);

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

    private void CheckPoint(Vector2Int start, Vector2Int end, Vector2Int target, Point current, PriorityQueue<Point, float> pq)
    {
        if (!map.ContainsCoord(target)) return;
        if (map[target].IsJoined) return;
        if (map[target].PointType == PointType.Wall) return;

        // var h = (int)Mathf.Sqrt(Mathf.Pow(target.x - end.x, 2) + Mathf.Pow(target.y - end.y, 2));
        //current.Cost + Vector2.Distance(target, current.Coord) + 
        var h = current.Cost + Vector2.Distance(current.Coord, target) + Mathf.Abs(end.x - target.x) + Mathf.Abs(end.y - target.y);

        if (map[target].Cost > h)
        {
            map[target].Cost = h;
            map[target].Parent = current;
            if (target != end)
                map[target].Color = Color.yellow;
            pq.Enqueue(map[target], h);
        }
    }
}