using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class JPS : BasePathFind
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
            if (current.Coord != start && current.Coord != end)
                current.Color = Color.blue;
            yield return new WaitForSeconds(pathFinder.WaitTime);
            current.IsJoined = true;

            if (current.Coord != start && current.Coord != end)
                map[current.Coord].Color = Color.blue;

            if (current.JPSDirection == Vector2Int.zero)
            {
                var isEnd = false;
                JumpDiagonal(current.Coord, new(1, 1), end, pq, ref isEnd);
                if (isEnd) break;
                yield return new WaitForSeconds(pathFinder.WaitTime);
                JumpDiagonal(current.Coord, new(-1, 1), end, pq, ref isEnd);
                if (isEnd) break;
                yield return new WaitForSeconds(pathFinder.WaitTime);
                JumpDiagonal(current.Coord, new(1, -1), end, pq, ref isEnd);
                if (isEnd) break;
                yield return new WaitForSeconds(pathFinder.WaitTime);
                JumpDiagonal(current.Coord, new(-1, -1), end, pq, ref isEnd);
                if (isEnd) break;
                yield return new WaitForSeconds(pathFinder.WaitTime);
            }
            else
            {
                var isEnd = false;
                JumpDiagonal(current.Coord, current.JPSDirection, end, pq, ref isEnd);
                if (isEnd) break;
                yield return new WaitForSeconds(pathFinder.WaitTime);
            }
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

    private void JumpStraight(Vector2Int start, Vector2Int dir, Vector2Int end, PriorityQueue<Point, float> pq, ref bool isFindEnd)
    {
        var current = start;
        do
        {
            if (!map.ContainsCoord(current)) break;
            if (map[current].PointType == PointType.Wall) break;

            if (current == end)
            {
                map[end].Parent = map[start];
                map[end].IsJoined = true;
                isFindEnd = true;
                return;
            }

            if (current != start && current != end)
                map[current].Color = Color.gray;

            if (dir.x != 0) //horizontal
            {
                var upWall =
                    map.ContainsCoord(new(current.x, current.y + 1)) &&
                    map[new(current.x, current.y + 1)].PointType == PointType.Wall &&
                    map.ContainsCoord(new(current.x + dir.x, current.y + 1)) &&
                    map[new(current.x + dir.x, current.y + 1)].PointType != PointType.Wall;

                var downWall =
                    map.ContainsCoord(new(current.x, current.y - 1)) &&
                    map[new(current.x, current.y - 1)].PointType == PointType.Wall &&
                    map.ContainsCoord(new(current.x + dir.x, current.y - 1)) &&
                    map[new(current.x + dir.x, current.y - 1)].PointType != PointType.Wall;

                if (upWall) CheckPoint(new(current.x + dir.x, current.y + 1), start, end, new(dir.x, 1), pq);
                if (downWall) CheckPoint(new(current.x + dir.x, current.y - 1), start, end, new(dir.x, -1), pq);
            }
            else //vertical
            {
                var leftWall =
                    map.ContainsCoord(new(current.x - 1, current.y)) &&
                    map[new(current.x - 1, current.y)].PointType == PointType.Wall &&
                    map.ContainsCoord(new(current.x - 1, current.y + dir.y)) &&
                    map[new(current.x - 1, current.y + dir.y)].PointType != PointType.Wall;

                var rightWall =
                    map.ContainsCoord(new(current.x + 1, current.y)) &&
                    map[new(current.x + 1, current.y)].PointType == PointType.Wall &&
                    map.ContainsCoord(new(current.x + 1, current.y + dir.y)) &&
                    map[new(current.x + 1, current.y + dir.y)].PointType != PointType.Wall;

                if (leftWall) CheckPoint(new(current.x - 1, current.y + dir.y), start, end, new(-1, dir.y), pq);
                if (rightWall) CheckPoint(new(current.x + 1, current.y + dir.y), start, end, new(1, dir.y), pq);
            }
            current += dir;
        }
        while (true);
    }

    private void JumpDiagonal(Vector2Int start, Vector2Int dir, Vector2Int end, PriorityQueue<Point, float> pq, ref bool isFindEnd)
    {
        var current = start;
        do
        {
            JumpStraight(current, new(dir.x, 0), end, pq, ref isFindEnd);
            if (isFindEnd) return;
            JumpStraight(current, new(0, dir.y), end, pq, ref isFindEnd);
            if (isFindEnd) return;

            current += dir;
            if (!map.ContainsCoord(current)) break;
            if (map[current].PointType == PointType.Wall) break;

            if (current == end)
            {
                map[end].Parent = map[start];
                map[end].IsJoined = true;
                isFindEnd = true;
                return;
            }

            if (current != start && current != end)
                map[current].Color = Color.gray;

            map[current].G = map[start].G + Vector2.Distance(start, current);
            map[current].Parent = map[start];

            var wall1 =
                map.ContainsCoord(new(current.x, current.y - dir.y)) &&
                map[new(current.x, current.y - dir.y)].PointType == PointType.Wall &&
                map.ContainsCoord(new(current.x + dir.x, current.y - dir.y)) &&
                map[new(current.x + dir.x, current.y - dir.y)].PointType != PointType.Wall;
            var wall2 =
                map.ContainsCoord(new(current.x - dir.x, current.y)) &&
                map[new(current.x - dir.x, current.y)].PointType == PointType.Wall &&
                map.ContainsCoord(new(current.x - dir.x, current.y + dir.y)) &&
                map[new(current.x - dir.x, current.y + dir.y)].PointType != PointType.Wall;

            if (wall1) CheckPoint(new(current.x + dir.x, current.y - dir.y), start, end, new(dir.x, -dir.y), pq);
            if (wall2) CheckPoint(new(current.x - dir.x, current.y + dir.y), start, end, new(-dir.x, dir.y), pq);
            
        }
        while (true);
    }

    private void CheckPoint(Vector2Int current, Vector2Int parent, Vector2Int end, Vector2Int dir, PriorityQueue<Point, float> pq)
    {
        if (!map.ContainsCoord(current)) return;
        if (map[current].IsJoined) return;
        if (map[current].PointType == PointType.Wall) return;

        var g = map[parent].G + Vector2.Distance(parent, current);
        var h = Vector2.Distance(end, current);

        if (map[current].G + h > g + h)
        {
            map[current].G = g;
            map[current].Parent = map[parent];
            map[current].JPSDirection = dir;
            if (current != end)
                map[current].Color = Color.yellow;
            pq.Enqueue(map[current], g + h);
        }
    }
}