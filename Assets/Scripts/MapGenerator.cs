using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public int regionCount;

    public float regionMinDistance;

    public float neighbourMaxDistance;

    public Sprite mapBase;

    public MapSO mapSO;

    public GameObject map;

    public GameObject regionPoint;

    public GameObject neighbourLine;

    List<Region> _regions;

    List<GameObject> _regionPoints;
    
    Color RandomColor()
    {
        //随机颜色的RGB值。即刻得到一个随机的颜色
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        Color color = new Color(r,g,b);
        return color;
    }

    Vector2 MapToWorldPosition(Vector2Int pos)
    {
        Vector2Int mapBias = new Vector2Int(mapBase.texture.width, mapBase.texture.height) / 2;
        return (Vector2)(pos - mapBias) / mapBase.pixelsPerUnit;
    }

    void ClearMap()
    {
        foreach (GameObject point in _regionPoints)
        {
            Destroy(point);
        }

        _regions = new List<Region>();

    }

    public void GenerateMap()
    {
        ClearMap();
        for (var r = 0; r < regionCount; r++)
        {
            int x = Random.Range(0, mapBase.texture.width);
            int y = Random.Range(0, mapBase.texture.height);
            if (mapBase.texture.GetPixel(x, y).a < 1)
            {
                r--;
                continue;
            }

            var pos = new Vector2Int(x, y);
            var skip = false;
            foreach (Region region in _regions.Where(region => Vector2Int.Distance(pos, region.InitialPos) < regionMinDistance))
            {
                skip = true;
            }

            if (skip)
            {
                r--;
                continue;
            }
            
            
            _regions.Add(new Region(pos, RandomColor()));
        }

        foreach (Region region in _regions)
        {
            var initialPos = region.InitialPos;
            Debug.Log(initialPos);
            var go = Instantiate(regionPoint, map.transform);
            region.Point = go;
            _regionPoints.Add(go);
            go.transform.localPosition = MapToWorldPosition(initialPos);
            go.GetComponent<SpriteRenderer>().color = region.Color;
        }
    }

    public void ConnectNeighbour()
    {
        for (int i = 0; i < _regions.Count-1; i++)
        {
            var nearest = i + 1;
            for (int j = i+1; j < _regions.Count; j++)
            {
                var dist = Vector2Int.Distance(_regions[i].InitialPos, _regions[j].InitialPos);
                if (dist < neighbourMaxDistance)
                {
                    _regions[i].Neighbours.Add(_regions[j]);
                    _regions[j].Neighbours.Add(_regions[i]);
                }

                if (dist < Vector2Int.Distance(_regions[i].InitialPos, _regions[nearest].InitialPos))
                {
                    nearest = j;
                }
            }

            if (_regions[i].Neighbours.Count == 0)
            {
                _regions[i].Neighbours.Add(_regions[nearest]);
                _regions[nearest].Neighbours.Add(_regions[i]);
            }
        }

        foreach (Region region in _regions)
        {
            foreach (Region neighbour in region.Neighbours)
            {
                var line = Instantiate(neighbourLine, map.transform);
                _regionPoints.Add(line);
                line.GetComponent<LineRenderer>().SetPosition(0, MapToWorldPosition(region.InitialPos));
                line.GetComponent<LineRenderer>().SetPosition(1, MapToWorldPosition(neighbour.InitialPos));
            }
        }
    }

    void Start()
    {
        _regions = new List<Region>();
        _regionPoints = new List<GameObject>();
    }
}


