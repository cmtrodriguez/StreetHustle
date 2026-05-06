using UnityEngine;
using UnityEditor;

public class AdjustSapinSapin4
{
    [MenuItem("Tools/Adjust Sapin Sapin 4")]
    public static void Execute()
    {
        GameObject cake = GameObject.Find("New_Wooden_Cart/SapinSapinCake");
        GameObject plate = GameObject.Find("New_Wooden_Cart/banana leaf plate 3d model");

        if (cake == null || plate == null)
        {
            Debug.LogError("Cake or plate not found.");
            return;
        }

        // Move cake down slightly to sit on the leaf
        cake.transform.position += new Vector3(0, -0.015f, 0);

        Debug.Log("Adjusted Sapin Sapin.");
    }
}
