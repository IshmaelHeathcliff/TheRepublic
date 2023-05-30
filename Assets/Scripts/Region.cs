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

    [FoldoutGroup("区域数据")] public int population;
    [FoldoutGroup("区域数据")] public float production;
    [FoldoutGroup("区域数据")] public float consumption;
    [FoldoutGroup("区域数据")] public float military;
    [FoldoutGroup("区域数据")] public float culture;
    [FoldoutGroup("区域数据")] public float education;
    [FoldoutGroup("区域数据")] public float technology;
    [FoldoutGroup("区域数据")] public float support;
    [FoldoutGroup("区域数据")] public float order;

    public People people;
    // [HideInInspector] 
    public Dictionary<Region, float> neighbours;

    // [Button]
    public void Init(string rName, Vector2Int pos, Color color, float minInfluence=0.5f, float maxInfluence=1f)
    {
        regionName = rName;
        initialPos = pos;
        this.color = color;
        neighbours = new Dictionary<Region, float>();
        regionInfluence = Random.Range(minInfluence, maxInfluence);
        
        // Debug.Log(pos);

        people = CreateInstance<People>();
        // people.name = name + " People";
        // AssetDatabase.AddObjectToAsset(people, this);
        AssetDatabase.CreateAsset(people, MapGenerator.LocalSOPath + $"Region/People/{regionName} People.asset");
    }
}