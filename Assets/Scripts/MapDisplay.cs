using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class MapDisplay : MonoBehaviour {

    public SpriteRenderer textureRender;

    public SpriteRenderer noiseRender;
    
    public GameObject map;

    public GameObject regionPoint;
    
    public GameObject neighbourLine;

    List<GameObject> _regionParts;

    public void DrawTexture(Texture2D texture) {
        textureRender.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    public void DrawNoiseTexture(Texture2D texture)
    {
        noiseRender.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    public void DisplayPoints(List<Region> regions)
    {
        var mapGen = GetComponent<MapGenerator>();
        foreach (Region region in regions)
        {
            var initialPos = region.InitialPos;
            Debug.Log(initialPos);
            var go = Instantiate(regionPoint, map.transform);
            _regionParts.Add(go);
            go.transform.localPosition = mapGen.MapToWorldPosition(initialPos);
            go.GetComponent<SpriteRenderer>().color = region.Color;
        }
    }
    
    public void DisplayNeighbour(List<Region> regions)
    {
        var mapGen = GetComponent<MapGenerator>();
        foreach (Region region in regions)
        {
            foreach (Region neighbour in region.Neighbours)
            {
                GameObject line = Instantiate(neighbourLine, map.transform);
                _regionParts.Add(line);
                Vector3 point1 = mapGen.MapToWorldPosition(region.InitialPos);
                Vector3 point2 = mapGen.MapToWorldPosition(neighbour.InitialPos);
                line.GetComponent<LineRenderer>().SetPosition(0, point1);
                line.GetComponent<LineRenderer>().SetPosition(1, point2);
            }
        }
    }

    public void ClearPoints()
    {
        foreach (GameObject point in _regionParts)
        {
            DestroyImmediate(point);
        }
    }
}