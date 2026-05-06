using UnityEditor;
using UnityEngine;

public class CheckMesh
{
    public static void Execute()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Models/vintage_cart.glb");
        if (prefab != null)
        {
            MeshFilter[] filters = prefab.GetComponentsInChildren<MeshFilter>();
            foreach (MeshFilter filter in filters)
            {
                Debug.Log("Mesh: " + filter.sharedMesh.name + " submesh count: " + filter.sharedMesh.subMeshCount);
            }
        }
        else
        {
            Debug.Log("Prefab not found");
        }
    }
}