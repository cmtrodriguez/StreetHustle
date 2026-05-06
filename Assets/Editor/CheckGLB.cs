using UnityEditor;
using UnityEngine;

public class CheckGLB
{
    public static void Execute()
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Models/vintage_cart.glb");
        if (prefab != null)
        {
            PrintHierarchy(prefab.transform, "");
        }
        else
        {
            Debug.Log("Prefab not found");
        }
    }

    private static void PrintHierarchy(Transform t, string indent)
    {
        Debug.Log(indent + t.name);
        foreach (Transform child in t)
        {
            PrintHierarchy(child, indent + "  ");
        }
    }
}