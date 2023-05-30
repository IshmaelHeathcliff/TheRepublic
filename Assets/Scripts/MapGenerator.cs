using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MapDisplay))]
public class MapGenerator : MonoBehaviour
{
    public const string Path = "Assets/ScriptableObjects/";
    public bool autoUpdate;
    
    
    [FoldoutGroup("Map Base")] public int width;
    [FoldoutGroup("Map Base")] public int height;
    [FoldoutGroup("Map Base")] public int edgeCount = 8;
    [FoldoutGroup("Map Base")] public float edgeDistance = 50f;
    [FoldoutGroup("Map Base")] [Range(0f, 1f)] public float centerInfluence = 0.5f;

    
    [FoldoutGroup("Region")] public int regionCount;
    [FoldoutGroup("Region")] public float regionMinDistance;
    [FoldoutGroup("Region")]public bool displayNeighbour;


    [FoldoutGroup("Noise Map")] public int seed;
    [FoldoutGroup("Noise Map")] public float scale;
    [FoldoutGroup("Noise Map")] [Range(0, 10)] public int octaves;
    [FoldoutGroup("Noise Map")] [Range(0, 1)] public float persistence;
    [FoldoutGroup("Noise Map")] public float lacunarity;
    [FoldoutGroup("Noise Map")] public Vector2 offset;
    [FoldoutGroup("Noise Map")] public float noiseInfluence;


    MapDisplay _display;
    MapDisplay Display
    {
        get
        {
            if (_display == null)
            {
                _display = GetComponent<MapDisplay>();
            }

            return _display;
        }
    }

    Map _map;
    bool[,] _mapBase;
    float[,] _noise;

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
        GameObject displayMap = Display.map;
        var pixelPerUnit = Display.textureRender.sprite.pixelsPerUnit;
        Vector3 mapScale = displayMap.transform.localScale;
        Vector3 worldPos = (Vector2) (pos - mapBias) / pixelPerUnit;
        if (local) return worldPos;
        worldPos.x *= mapScale.x;
        worldPos.y *= mapScale.y;
        worldPos += displayMap.transform.position;
        return worldPos;
    }

    Map CreateMapSO()
    {
        var map = ScriptableObject.CreateInstance<Map>();
        AssetDatabase.CreateAsset(map, Path + "Map.asset");
        // AssetDatabase.SaveAssets();
        // AssetDatabase.Refresh();
        return map;
    }

    Region CreateRegionSO(string regionName, Vector2Int pos)
    {
        var region = ScriptableObject.CreateInstance<Region>();
        // Debug.Log($"Start initiating Region{r}");
        region.Init(regionName, pos, RandomColor());
        // Debug.Log($"finish initiating Region{r}");
        AssetDatabase.CreateAsset(region, Path + $"Region/{regionName}.asset");
        // AssetDatabase.SaveAssets();
        // AssetDatabase.Refresh();
        return region;
    }

    void GenerateNoise()
    {
        _noise = Noise.GenerateNoiseMap(width, height, seed, scale, 
            octaves, persistence, lacunarity, offset);
        
        // Texture2D noiseMap = TextureGenerator.TextureFromHeightMap(_noise);
        // Display.DrawNoiseTexture(noiseMap);
    }

    void GenerateMapBase()
    {
        var edgePoints = new List<Vector2Int> {new(width/2, height/2)};

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
                        edgeInfluence = centerInfluence;
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
        // Display.DrawTexture(texture);
    }

    void GenerateInitialPoint()
    {
        _map.regions = new List<Region>();
        var regions = _map.regions;
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
            var skip = false;
            for (var i = 0; i < r; i++)
            {
                if (!(Vector2Int.Distance(pos, regions[i].initialPos) < regionMinDistance)) continue;
                skip = true;
                break;
            }

            if (skip)
            {
                r--;
                continue;
            }

            Region region = CreateRegionSO($"Region {r}", pos);
            regions.Add(region);
        }
        
        // EditorUtility.SetDirty(_map);
        AssetDatabase.SaveAssets();
        // AssetDatabase.Refresh();
    }

    void GenerateRegion()
    {
        var colorMap = new Color[width * height];
        _map.RegionMap = new Region[width, height];
        var regionMap = _map.RegionMap;
        var regions = _map.regions;
        
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                if (!_mapBase[x, y])
                {
                    colorMap[x + y*width] = new Color(0, 0, 0, 0);
                    continue;
                }

                Region nearestRegion = regions[0];
                foreach (Region region in regions)
                {
                    var distance1 = Vector2Int.Distance(nearestRegion.initialPos, new Vector2Int(x, y)) *
                                    nearestRegion.regionInfluence + (_noise[x, y] * 2 - 1)* noiseInfluence;
                    var distance2 = Vector2Int.Distance(region.initialPos, new Vector2Int(x, y)) *
                                    region.regionInfluence;
                    if (distance1 > distance2)
                        nearestRegion = region;
                }

                colorMap[x + y*width] = nearestRegion.color;
                regionMap[x, y] = nearestRegion;
            }
        }

        Texture2D texture = TextureGenerator.TextureFromColourMap(colorMap, width, height);
        // Encode texture into PNG
        var bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Artworks/Map.png", bytes);
        AssetDatabase.Refresh();
        _map.mapTexture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Artworks/Map.png");
        EditorUtility.SetDirty(_map);
        Display.DrawTexture(texture);
    }

    void ConnectNeighbour()
    {
        var regionMap = _map.RegionMap;
        
        for (int x = 1; x < width-1; x++)
        {
            for (int y = 1; y < height-1; y++)
            {
                Region region = regionMap[x, y];
                if (region == null) continue;
                
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (i == 0 && j == 0) continue;
                        Region nearbyRegion = regionMap[x + i, y + j];
                        if (nearbyRegion == null) continue;
                        
                        if (region != nearbyRegion)
                        {
                            if (!region.neighbours.ContainsKey(nearbyRegion))
                            {
                                region.neighbours.Add(nearbyRegion, 1f);
                            }
                            else
                            {
                                region.neighbours[nearbyRegion] += 1;
                            }
                        }
                    }
                }
            }
        }
    }

    void DisplayRegionInfo()
    {
        Display.ClearPoints();
        Display.DisplayPoints(_map.regions);
        Display.DisplayNeighbour(_map.regions);
    }

    [Button]
    public void GenerateMap()
    {
        _map = CreateMapSO();
        GenerateNoise();
        GenerateMapBase();
        GenerateInitialPoint();
        GenerateRegion();
        ConnectNeighbour();
        if(displayNeighbour) DisplayRegionInfo();
    }

    [Button]
    public void LoadMap()
    {
        _map = AssetDatabase.LoadAssetAtPath<Map>(Path + "Map.asset");
        Display.DrawTexture((Texture2D)_map.mapTexture);
        Debug.Log(_map.RegionMap.Length);
    }

    void Start()
    {
        _display = GetComponent<MapDisplay>();
        if (_map != null)
        {
            LoadMap();
        }
        else
        {
            GenerateMap();
        }
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


