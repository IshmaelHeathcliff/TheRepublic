using System.Collections.Generic;
using UnityEngine;

public class Region
{
    public Vector2Int InitialPos;
    public List<Region> Neighbours;
    public Color Color;
    public GameObject Point;

    public Region(Vector2Int pos, Color color)
    {
        InitialPos = pos;
        Color = color;
        Neighbours = new List<Region>();
    }
}