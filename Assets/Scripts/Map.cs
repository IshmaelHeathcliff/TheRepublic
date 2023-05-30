using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Map", menuName = "ScriptableObjects/Map", order = 1)]
public class Map : SerializedScriptableObject
{
        public List<Region> regions;
        
        [HideInInspector]
        public Region[,] RegionMap;

        [PreviewField(100f)]
        public Texture mapTexture;
}