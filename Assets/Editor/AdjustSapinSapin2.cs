using UnityEngine;
using UnityEditor;

public class AdjustSapinSapin2
{
    [MenuItem("Tools/Adjust Sapin Sapin 2")]
    public static void Execute()
    {
        GameObject cake = GameObject.Find("New_Wooden_Cart/SapinSapinCake");
        GameObject plate = GameObject.Find("New_Wooden_Cart/banana leaf plate 3d model");

        if (cake == null || plate == null)
        {
            Debug.LogError("Cake or plate not found.");
            return;
        }

        // Replace MeshColliders with BoxColliders
        foreach (Transform child in cake.transform)
        {
            MeshCollider mc = child.GetComponent<MeshCollider>();
            if (mc != null)
            {
                Object.DestroyImmediate(mc);
            }

            BoxCollider bc = child.GetComponent<BoxCollider>();
            if (bc == null)
            {
                bc = child.gameObject.AddComponent<BoxCollider>();
            }
        }

        // Adjust position
        // Plate max Y is plate.GetComponent<Renderer>().bounds.max.y
        // Cake min Y is cake.GetComponentInChildren<Renderer>().bounds.min.y
        
        float plateMaxY = plate.GetComponent<Renderer>().bounds.max.y;
        
        float cakeMinY = float.MaxValue;
        foreach (Renderer r in cake.GetComponentsInChildren<Renderer>())
        {
            if (r.bounds.min.y < cakeMinY)
            {
                cakeMinY = r.bounds.min.y;
            }
        }

        float diff = plateMaxY - cakeMinY;
        
        // Move cake up by diff
        cake.transform.position += new Vector3(0, diff, 0);

        Debug.Log("Adjusted Sapin Sapin.");
    }
}
