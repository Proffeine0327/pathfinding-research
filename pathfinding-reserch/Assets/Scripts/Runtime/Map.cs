using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UniRx;
using System.Security.Cryptography;

public class Map : MonoBehaviour
{
    [SerializeField] private Point pointPrefab;
    [SerializeField] private Vector2Int size;

    private Point[,] map;

    public void Initialize()
    {
        for(int y = 0; y < size.y; y++)
        {
            for(int x = 0; x < size.x; x++)
            {
                map[y, x].Cost = float.MaxValue;
                map[y, x].IsJoined = false;
                map[y, x].Parent = null;
            }
        }
    }

    private void Awake()
    {
        map = new Point[size.y, size.x];
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                var newPoint = Instantiate(pointPrefab, transform);
                newPoint.transform.position = new(x, y);
                newPoint.Coordinate = new(x, y);
                map[y, x] = newPoint;
            }
        }
    }

    public Point this[int y, int x]
    {
        get => map[y, x];
        set => map[y, x] = value;
    }

    public Point this[Vector2Int index]
    {
        get => map[index.y, index.x];
        set => map[index.y, index.x] = value;
    }
}
