using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Map", menuName = "ScriptableObjects/Map", order = 1)]
public class Map : SerializedScriptableObject
{
        public List<Region> Regions;
        
        [HideInInspector]
        public Region[,] RegionMap;

        public Texture MapTexture;
}