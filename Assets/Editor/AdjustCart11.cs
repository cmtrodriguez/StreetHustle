using UnityEditor;
using UnityEngine;

public class AdjustCart11
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
            // Adjust height slightly higher
            bilao.transform.localPosition = new Vector3(0, -0.175f, 0);
        }
        
        Debug.Log("Cart adjusted 11.");
    }
}