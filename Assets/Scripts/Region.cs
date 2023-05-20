using System.Collections.Generic;
using UnityEngine;

public class Region
{
    public Vector2Int InitialPos;
    public List<Region> Neighbours;
    public Color Color;
    public float RegionInfluence;

    public Region(Vector2Int pos, Color color)
    {
        InitialPos = pos;
        Color = color;
        Neighbours = new List<Region>();
        RegionInfluence = Random.Range(0.5f, 1f);
    }
}