using System.Collections.Generic;
using UnityEngine;

public class Region
{
    public Vector2Int InitialPos;
    public Dictionary<Region, float> Neighbours;
    public Color Color;
    public float RegionInfluence;

    public Region(Vector2Int pos, Color color, float minInfluence=0.5f, float maxInfluence=1f)
    {
        InitialPos = pos;
        Color = color;
        Neighbours = new Dictionary<Region, float>();
        RegionInfluence = Random.Range(minInfluence, maxInfluence);
    }
}