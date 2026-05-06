using UnityEngine;
using UnityEditor;

public class CheckSapinSapin
{
    public static void Execute()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Models/sapin sapin.glb");
        GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        instance.name = "TempSapinSapin";
        
        MeshFilter mf = instance.GetComponentInChildren<MeshFilter>();
        if (mf != null)
        {
            Debug.Log("Bounds: " + mf.sharedMesh.bounds);
        }
    }
}