using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;

public enum PointType { None, Wall, Start, End }

public class Point : MonoBehaviour, IComparable<Point>, IPointerDownHandler, IPointerEnterHandler
{
    private PathFinder pathFinder => PathFinder.Instance;

    private SpriteRenderer spriteRenderer;

    public bool IsJoined { get; set; }
    public float Cost { get; set; }
    public Point Parent { get; set; }
    public PointType PointType { get; set; }
    public Vector2Int Coordinate { get; set; }
    public Color Color
    {
        get => spriteRenderer.color;
        set => spriteRenderer.color = value;
    }

    public void SetType(PointType type)
    {
        PointType = type;
        switch (type)
        {
            case PointType.None: Color = Color.white; break;
            case PointType.Wall: Color = Color.black; break;
            case PointType.Start: Color = Color.red; break;
            case PointType.End: Color = Color.cyan; break;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1)) return;

        switch (pathFinder.DrawType)
        {
            case PointType.None: break;
            case PointType.Wall:
                if (eventData.button == PointerEventData.InputButton.Left) pathFinder.SetWall(Coordinate);
                if (eventData.button == PointerEventData.InputButton.Right) pathFinder.SetNone(Coordinate);
                break;
            case PointType.Start: pathFinder.SetStart(Coordinate); break;
            case PointType.End: pathFinder.SetEnd(Coordinate); break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (pathFinder.DrawType != PointType.Wall) return;

        if (Input.GetMouseButton(0)) pathFinder.SetWall(Coordinate);
        if (Input.GetMouseButton(1)) pathFinder.SetNone(Coordinate);
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public int CompareTo(Point other)
    {
        if (Cost == other.Cost) return 0;
        return Cost - other.Cost > 0 ? 1 : -1;
    }
}
