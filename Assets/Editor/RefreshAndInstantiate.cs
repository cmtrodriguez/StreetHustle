using UnityEditor;
using UnityEngine;

public class RefreshAndInstantiate
{
    public static void Execute()
    {
        AssetDatabase.Refresh();
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/filipino-tricycle-ps1-low-poly/source/toPostonSketchfab.fbx");
        if (prefab != null)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            instance.name = "Tricycle";
            Debug.Log("Successfully instantiated Tricycle");
        }
        else
        {
            Debug.LogError("Failed to load toPostonSketchfab.fbx");
        }
    }
}