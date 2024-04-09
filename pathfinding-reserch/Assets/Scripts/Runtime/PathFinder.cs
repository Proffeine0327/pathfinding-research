using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathFindType
{
    Djikstra,
    Astar,
    Jps
}

public class PathFinder : MonoBehaviour
{
    public static PathFinder Instance { get; private set; }

    private Map map => Map.Instance;

    private Dictionary<PathFindType, BasePathFind> pathFindAlgorithmTable = new();

    public bool IsFindPath { get; set; }
    public int Distance { get; set; }
    public float WaitTime { get; set; }
    public PointType DrawType { get; set; }
    public PathFindType PathFindType { get; set; }

    public void FindPath()
    {
        foreach(var a in pathFindAlgorithmTable.Values)
            a.StopPathFind();
        map.Initialize();
        pathFindAlgorithmTable[PathFindType].FindPathVisualize(map.StartCoord, map.EndCoord);
    }
    
    private void Awake()
    {
        Instance = this;
        pathFindAlgorithmTable.Add(PathFindType.Djikstra, gameObject.AddComponent<Djikstra>());
        pathFindAlgorithmTable.Add(PathFindType.Astar, gameObject.AddComponent<Astar>());
        pathFindAlgorithmTable.Add(PathFindType.Jps, gameObject.AddComponent<JPS>());
    }
}
