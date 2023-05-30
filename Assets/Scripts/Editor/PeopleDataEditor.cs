using System.IO;
using System.Net;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace Editor
{
    public class PeopleDataEditor : OdinMenuEditorWindow
    {
        readonly string _resourcePath = "Assets/ScriptableObjects/";
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(supportsMultiSelect: true);
            tree.AddAssetAtPath($"Map", _resourcePath + "Map.asset");
            GetRegion(tree);
            GetPeople(tree);
            
            return tree;
        }
        
        [MenuItem("Tools/PeopleData Editor")]
        static void OpenWindow()           
        {               
            var window = GetWindow<PeopleDataEditor>();               
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1200, 600);           
        }

        void GetRegion(OdinMenuTree tree)
        {
            var regionPath = _resourcePath + "Region/";

            var dir = new DirectoryInfo(regionPath);
            var files = dir.GetFiles();

            foreach (var file in files)
            {
                if (file.Name.EndsWith(".asset"))
                {
                    // Debug.Log(file.FullName);
                    tree.AddAssetAtPath($"Region/{file.Name.TrimEnd(".asset")}", regionPath + file.Name);
                }
            }
        }
        
        void GetPeople(OdinMenuTree tree)
        {
            var regionPath = _resourcePath + "Region/People/";

            var dir = new DirectoryInfo(regionPath);
            var files = dir.GetFiles();

            foreach (var file in files)
            {
                if (file.Name.EndsWith(".asset"))
                {
                    // Debug.Log(file.FullName);
                    tree.AddAssetAtPath($"People/{file.Name.TrimEnd(".asset")}", regionPath + file.Name);
                }
            }
        }
    }
}