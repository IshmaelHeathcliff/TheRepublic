using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Region", menuName = "ScriptableObjects/Region")]
public class Region : SerializedScriptableObject
{
    [FoldoutGroup("区域生成信息")] public string regionName;
    [FoldoutGroup("区域生成信息")] public Vector2Int InitialPos;
    [FoldoutGroup("区域生成信息")] public Color Color;
    [FoldoutGroup("区域生成信息")] public float RegionInfluence;

    [FoldoutGroup("区域数据")] public int population;
    [FoldoutGroup("区域数据")] public float production;
    [FoldoutGroup("区域数据")] public float consumption;
    [FoldoutGroup("区域数据")] public float military;
    [FoldoutGroup("区域数据")] public float culture;
    [FoldoutGroup("区域数据")] public float education;
    [FoldoutGroup("区域数据")] public float technology;
    [FoldoutGroup("区域数据")] public float support;
    [FoldoutGroup("区域数据")] public float order;

    public People People;
    [HideInInspector] public Dictionary<Region, float> Neighbours;

    // [Button]
    public void Init(string rName, Vector2Int pos, Color color, float minInfluence=0.5f, float maxInfluence=1f)
    {
        regionName = rName;
        InitialPos = pos;
        Color = color;
        Neighbours = new Dictionary<Region, float>();
        RegionInfluence = Random.Range(minInfluence, maxInfluence);

        People = ScriptableObject.CreateInstance<People>();
        AssetDatabase.AddObjectToAsset(People, this);
        People.name = name + " People";

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}