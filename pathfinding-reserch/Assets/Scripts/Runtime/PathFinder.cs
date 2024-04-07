using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Utils;

public enum AlgorithmType
{
    Djikstra,
    Astar,
    Jps
}

public class PathFinder : MonoBehaviour
{
    public static PathFinder Instance { get; private set; }

    [SerializeField] private Map map;

    public bool IsPathfinding { get; set; }
    public int Distance { get; set; }
    public float FindTime { get; set; }
    public PointType DrawType { get; set; }
    public AlgorithmType AlgorithmType { get; set; }
    public Vector2Int StartCoordinate { get; set; } = new(-1, -1);
    public Vector2Int EndCoordinate { get; set; } = new(-1, -1);

    public void SetNone(Vector2Int coordinate)
    {
        if(StartCoordinate == coordinate) StartCoordinate = Vector2Int.one * -1;
        if(EndCoordinate == coordinate) EndCoordinate = Vector2Int.one * -1;
        map[coordinate.y, coordinate.x].SetType(PointType.None);
    }

    public void SetWall(Vector2Int coordinate)
    {
        if(StartCoordinate == coordinate) StartCoordinate = Vector2Int.one * -1;
        if(EndCoordinate == coordinate) EndCoordinate = Vector2Int.one * -1;
        map[coordinate.y, coordinate.x].SetType(PointType.Wall);
    }

    public void SetStart(Vector2Int coordinate)
    {
        if (StartCoordinate != Vector2.one * -1)
            map[StartCoordinate.y, StartCoordinate.x].SetType(PointType.None);
        map[coordinate.y, coordinate.x].SetType(PointType.Start);
        StartCoordinate = coordinate;
    }

    public void SetEnd(Vector2Int coordinate)
    {
        if (EndCoordinate != Vector2.one * -1)
            map[EndCoordinate.y, EndCoordinate.x].SetType(PointType.None);
        map[coordinate.y, coordinate.x].SetType(PointType.End);
        EndCoordinate = coordinate;
    }

    private void Awake()
    {
        Instance = this;
    }
}
