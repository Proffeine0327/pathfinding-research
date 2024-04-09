using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map Instance { get; private set; }

    [SerializeField] private Point pointPrefab;
    [SerializeField] private Vector2Int size;

    private Point[,] map;

    public Vector2Int StartCoord { get; set; } = new(-1, -1);
    public Vector2Int EndCoord { get; set; } = new(-1, -1);

    public void Initialize()
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                map[y, x].G = float.MaxValue;
                map[y, x].IsJoined = false;
                map[y, x].Parent = null;
                map[y, x].JPSDirection = Vector2Int.zero;
                SetPoint(map[y, x].PointType, new(x, y));
            }
        }
    }

    public void SetPoint(PointType pointType, Vector2Int coordinate)
    {
        switch (pointType)
        {
            case PointType.None:
                if (coordinate == StartCoord) StartCoord = new(-1, -1);
                if (coordinate == EndCoord) EndCoord = new(-1, -1);

                if (ContainsCoord(coordinate))
                    this[coordinate].Color = Color.white;
                break;
            case PointType.Wall:
                if (coordinate == StartCoord) StartCoord = new(-1, -1);
                if (coordinate == EndCoord) EndCoord = new(-1, -1);

                if (ContainsCoord(coordinate))
                    this[coordinate].Color = Color.black;
                break;
            case PointType.Start:
                if (ContainsCoord(StartCoord))
                {
                    this[StartCoord].Color = Color.white;
                    this[StartCoord].PointType = PointType.None;
                }

                if (ContainsCoord(coordinate))
                {
                    this[coordinate].Color = Color.red;
                    StartCoord = coordinate;
                }
                break;
            case PointType.End:
                if (ContainsCoord(EndCoord))
                {
                    this[EndCoord].Color = Color.white;
                    this[EndCoord].PointType = PointType.None;
                }

                if (ContainsCoord(coordinate))
                {
                    this[coordinate].Color = Color.cyan;
                    EndCoord = coordinate;
                }
                break;
        }
        if (ContainsCoord(coordinate))
            this[coordinate].PointType = pointType;
    }

    private void Awake()
    {
        Instance = this;

        map = new Point[size.y, size.x];
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                var newPoint = Instantiate(pointPrefab, transform);
                newPoint.transform.position = new(x, y);
                newPoint.Coord = new(x, y);
                map[y, x] = newPoint;
            }
        }
    }

    public bool ContainsCoord(int x, int y)
        => !(y < 0 || y > size.y - 1 || x < 0 || x > size.x - 1);

    public bool ContainsCoord(Vector2Int coord)
        => ContainsCoord(coord.x, coord.y);

    public Point this[int y, int x]
    {
        get
        {
            if (!ContainsCoord(x, y)) return null;
            return map[y, x];
        }
        set
        {
            if (!ContainsCoord(x, y)) return;
            map[y, x] = value;
        }
    }

    public Point this[Vector2Int coord]
    {
        get
        {
            if (!ContainsCoord(coord)) return null;
            return map[coord.y, coord.x];
        }
        set
        {
            if (!ContainsCoord(coord)) return;
            map[coord.y, coord.x] = value;
        }
    }
}
