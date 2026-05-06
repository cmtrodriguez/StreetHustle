using UnityEditor;
using UnityEngine;

public class AdjustCart2
{
    public static void Execute()
    {
        GameObject cartRoot = GameObject.Find("New_Wooden_Cart");
        if (cartRoot == null)
        {
            Debug.LogError("New_Wooden_Cart not found");
            return;
        }

        // Handle Bilao
        GameObject bilao = GameObject.Find("New_Wooden_Cart/banana leaf plate 3d model");
        if (bilao != null) {
            // Scale down the bilao so it fits on the table
            bilao.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            // Adjust height slightly
            bilao.transform.localPosition = new Vector3(0, 0.02f, 0);
        }
        
        Debug.Log("Cart adjusted 2.");
    }
}