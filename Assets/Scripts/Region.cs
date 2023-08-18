using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Region", menuName = "ScriptableObjects/Region")]
public class Region : SerializedScriptableObject
{
    [FoldoutGroup("区域生成信息")] public string regionName;
    [FoldoutGroup("区域生成信息")] public Vector2Int initialPos;
    [FoldoutGroup("区域生成信息")] public Color color;
    [FoldoutGroup("区域生成信息")] public float regionInfluence;
    
    public int population;
    public Dictionary<RegionDataType, RegionData> data;

    public People people;
    // [HideInInspector] 
    public Dictionary<Region, float> neighbours;

    public Region()
    {
        neighbours = new Dictionary<Region, float>();

        data = new Dictionary<RegionDataType, RegionData>();
        foreach (RegionDataType type in Enum.GetValues(typeof(RegionDataType)))
        {
            data.Add(type, new RegionData());
        }
    }
    
    // [Button]
    public void Init(string rName, Vector2Int pos, Color color, float minInfluence=0.5f, float maxInfluence=1f)
    {
        regionName = rName;
        initialPos = pos;
        this.color = color;
        regionInfluence = Random.Range(minInfluence, maxInfluence);
        
        people = CreateInstance<People>();
        // people.name = name + " People";
        // AssetDatabase.AddObjectToAsset(people, this);
        AssetDatabase.CreateAsset(people, MapGenerator.LocalSOPath + $"Region/People/{regionName} People.asset");
    }

    public void UpdatePopulation()
    {
        population = people.GetPopulation();
    }

}

public class RegionData
{
    
}

public enum RegionDataType
{
    Influence,
    Production,
    Consumption,
    Military,
    Culture,
    Education,
    Technology,
    Support,
    Order,
}