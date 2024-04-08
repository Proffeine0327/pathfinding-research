using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;

public enum PointType { None, Wall, Start, End }

public class Point : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    private PathFinder pathFinder => PathFinder.Instance;
    private Map map => Map.Instance;

    private SpriteRenderer spriteRenderer;

    public bool IsJoined { get; set; }
    public float Cost { get; set; }
    public Point Parent { get; set; }
    public PointType PointType { get; set; }
    public Vector2Int Coord { get; set; }
    public Color Color
    {
        get => spriteRenderer.color;
        set => spriteRenderer.color = value;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (pathFinder.DrawType == PointType.Wall)
        {
            if (Input.GetMouseButton(0)) map.SetPoint(PointType.Wall, Coord);
            if (Input.GetMouseButton(1)) map.SetPoint(PointType.None, Coord);
        }
        else
        {
            if (Input.GetMouseButton(0))
                map.SetPoint(pathFinder.DrawType, Coord);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (pathFinder.DrawType != PointType.Wall) return;

        if (Input.GetMouseButton(0)) map.SetPoint(PointType.Wall, Coord);
        if (Input.GetMouseButton(1)) map.SetPoint(PointType.None, Coord);
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
