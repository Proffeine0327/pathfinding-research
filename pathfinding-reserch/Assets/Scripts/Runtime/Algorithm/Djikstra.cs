using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Djikstra : BasePathFind
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
                if(current.IsJoined) current = null;
            }
            while(current == null);
            if(current.Coord != start && current.Coord != end)
                current.Color = Color.blue;
            yield return new WaitForSeconds(pathFinder.WaitTime);
            current.IsJoined = true;
            
            if(current.Coord == end) break;
            
            CheckPoint(end, current.Coord + Vector2Int.up, current, pq);
            yield return new WaitForSeconds(pathFinder.WaitTime);
            CheckPoint(end, current.Coord + Vector2Int.down, current, pq);
            yield return new WaitForSeconds(pathFinder.WaitTime);
            CheckPoint(end, current.Coord + Vector2Int.left, current, pq);
            yield return new WaitForSeconds(pathFinder.WaitTime);
            CheckPoint(end, current.Coord + Vector2Int.right, current, pq);
            yield return new WaitForSeconds(pathFinder.WaitTime);
        }
        while(pq.Count != 0);

        if(map[end].IsJoined)
        {
            var back = map[end];
            while(back.Parent != null)
            {
                back = back.Parent;
                if(back.Coord != start)
                    back.Color = Color.magenta;
            }
        }
    }

    private void CheckPoint(Vector2Int end, Vector2Int target, Point current, PriorityQueue<Point, float> pq)
    {
        if(!map.ContainsCoord(target)) return;
        if(map[target].IsJoined) return;
        if(map[target].PointType == PointType.Wall) return;

        if(map[target].G > current.G + 1)
        {
            map[target].G = current.G + 1;
            map[target].Parent = current;
            if(target != end)
                map[target].Color = Color.yellow;
            pq.Enqueue(map[target], current.G + 1);
        }
    }
}