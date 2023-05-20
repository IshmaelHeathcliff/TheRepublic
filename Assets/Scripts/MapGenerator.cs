using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public bool autoUpdate;
    public bool displayNeighbour;
    
    [Header("Map Base")]
    public int width;

    public int height;
    
    public int edgeCount = 8;
    
    public float edgeDistance = 50f;

    [Range(0f, 1f)]
    public float baseInfluence = 0.5f;



    [Header("Region")]
    public int regionCount;
    
    public float regionMinDistance;
    
    public float neighbourMaxDistance;
    
    
    
    [Header("Noise Map")] 
    public int seed;

    public float scale;

    [Range(0, 10)]
    public int octaves;

    [Range(0, 1)]
    public float persistence;

    public float lacunarity;

    public Vector2 offset;

    public float noiseInfluence;


    bool[,] _mapBase;
    float[,] _noise;
    List<Region> _regions;
    Region[,] _regionMap;
    
    Color RandomColor()
    {
        //随机颜色的RGB值。即刻得到一个随机的颜色
        var r = Random.Range(0f, 1f);
        var g = Random.Range(0f, 1f);
        var b = Random.Range(0f, 1f);
        var color = new Color(r,g,b);
        return color;
    }

    public Vector3 MapToWorldPosition(Vector2Int pos, bool local=false)
    {
        Vector2Int mapBias = new Vector2Int(width, height) / 2;
        GameObject map = GetComponent<MapDisplay>().map;
        var pixelPerUnit = map.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        Vector3 mapScale = map.transform.localScale;
        Vector3 worldPos = (Vector2) (pos - mapBias) / pixelPerUnit;
        if (local) return worldPos;
        worldPos.x *= mapScale.x;
        worldPos.y *= mapScale.y;
        worldPos += map.transform.position;
        return worldPos;
    }

    public void ClearMap()
    {
        GetComponent<MapDisplay>().ClearPoints();
    }

    void GenerateNoise()
    {
        _noise = Noise.GenerateNoiseMap(width, height, seed, scale, 
            octaves, persistence, lacunarity, offset);
        
        // Texture2D noiseMap = TextureGenerator.TextureFromHeightMap(_noise);
        // var display = GetComponent<MapDisplay>();
        // display.DrawNoiseTexture(noiseMap);
    }

    void GenerateMapBase()
    {
        var edgePoints = new List<Vector2Int>();
        
        edgePoints.Add(new Vector2Int(width/2, height/2));
        for (var i = 0; i < edgeCount; i++)
        {
            var x = Random.Range(0, width);
            var y = Random.Range(0, height);

            var newPoint = new Vector2Int(x, y);
            var skip = edgePoints.Any(point => Vector2Int.Distance(point, newPoint) < edgeDistance);

            if (skip)
            {
                i--;
                continue;
            }
            
            edgePoints.Add(newPoint); 
        }

        _mapBase = new bool[width, height];
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                Vector2Int nearestEdge = edgePoints[0];
                var nearestInfluence = 1f;
                for (var i = 0; i < edgePoints.Count; i++)
                {
                    var edgeInfluence = 1f;
                    if (i == 0)
                    {
                        edgeInfluence = baseInfluence;
                    }
                    
                    var distance1 = Vector2Int.Distance(nearestEdge, new Vector2Int(x, y)) *
                        nearestInfluence + (_noise[x, y] * 2 - 1)* noiseInfluence;
                    var distance2 = Vector2Int.Distance(edgePoints[i], new Vector2Int(x, y)) *
                                    edgeInfluence;
                    if (distance1 > distance2)
                    {
                        nearestEdge = edgePoints[i];
                        nearestInfluence = edgeInfluence;
                    }
                }

                if(nearestEdge == edgePoints[0])
                    _mapBase[x, y] = true;
            }
        }

        // var colorMap = new Color[width * height];
        // for (var x = 0; x < width; x++)
        // {
        //     for (var y = 0; y < height; y++)
        //     {
        //         if (_mapBase[x, y])
        //         {
        //             colorMap[x + y * width] = Color.white;
        //         }
        //     }
        // }

        // Texture2D texture = TextureGenerator.TextureFromColourMap(colorMap, width, height);
        // var display = GetComponent<MapDisplay>();
        // display.DrawTexture(texture);
    }

    void GenerateInitialPoint()
    {
        _regions = new List<Region>();
        for (var r = 0; r < regionCount; r++)
        {
            var x = Random.Range(0, width);
            var y = Random.Range(0, height);
            if (!_mapBase[x, y])
            {
                r--;
                continue;
            }

            var pos = new Vector2Int(x, y);
            var skip = _regions.Any(region => Vector2Int.Distance(pos, region.InitialPos) < regionMinDistance);

            if (skip)
            {
                r--;
                continue;
            }
            
            
            _regions.Add(new Region(pos, RandomColor()));
        }
    }

    void GenerateRegion()
    {
        var colorMap = new Color[width * height];
        _regionMap = new Region[width, height];
        
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if (!_mapBase[x, y])
                {
                    colorMap[x + y*width] = new Color(0, 0, 0, 0);
                    continue;
                }

                Region nearestRegion = _regions[0];
                foreach (Region region in _regions)
                {
                    var distance1 = Vector2Int.Distance(nearestRegion.InitialPos, new Vector2Int(x, y)) *
                                    nearestRegion.RegionInfluence + (_noise[x, y] * 2 - 1)* noiseInfluence;
                    var distance2 = Vector2Int.Distance(region.InitialPos, new Vector2Int(x, y)) *
                                    region.RegionInfluence;
                    if (distance1 > distance2)
                        nearestRegion = region;
                }

                colorMap[x + y*width] = nearestRegion.Color;
                _regionMap[x, y] = nearestRegion;
            }
        }

        Texture2D texture = TextureGenerator.TextureFromColourMap(colorMap, width, height);
        var display = GetComponent<MapDisplay>();
        display.DrawTexture(texture);
    }

    void ConnectNeighbour()
    {
        for (int x = 1; x < width-1; x++)
        {
            for (int y = 1; y < height-1; y++)
            {
                Region region = _regionMap[x, y];
                if (region == null) continue;
                
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (i == 0 && j == 0) continue;
                        Region nearbyRegion = _regionMap[x + i, y + j];
                        if (nearbyRegion == null) continue;
                        
                        if (region != nearbyRegion)
                        {
                            if (!region.Neighbours.ContainsKey(nearbyRegion))
                            {
                                region.Neighbours.Add(nearbyRegion, 1f);
                            }
                            else
                            {
                                region.Neighbours[nearbyRegion] += 1;
                            }
                        }
                    }
                }
            }
        }
    }

    void DisplayRegionInfo()
    {
        var display = GetComponent<MapDisplay>();
        display.DisplayPoints(_regions);
        display.DisplayNeighbour(_regions);
    }

    public void GenerateMap()
    {
        GenerateNoise();
        GenerateMapBase();
        GenerateInitialPoint();
        GenerateRegion();
        ConnectNeighbour();
        ClearMap();
        if(displayNeighbour) DisplayRegionInfo();
    }

    void Start()
    {
        _regions = new List<Region>(); 
    }
    
    void OnValidate() {
        if (lacunarity < 1) {
            lacunarity = 1;
        }

        if (width < 100)
        {
            width = 100;
        }

        if (height < 100)
        {
            height = 100;
        }
    }
}


