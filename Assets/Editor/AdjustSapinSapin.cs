using UnityEngine;
using UnityEditor;

public class AdjustSapinSapin
{
    public static void Execute()
    {
        GameObject cakeGroup = GameObject.Find("New_Wooden_Cart/SapinSapinCake");
        if (cakeGroup == null)
        {
            Debug.LogError("Cake group not found");
            return;
        }

        // Adjust the position of the pieces to form a circle
        float radius = 0.15f; // Distance from center
        
        for (int i = 0; i < 6; i++)
        {
            Transform piece = cakeGroup.transform.GetChild(i);
            
            // Calculate position in a circle
            float angle = i * 60f * Mathf.Deg2Rad;
            float x = Mathf.Sin(angle) * radius;
            float z = Mathf.Cos(angle) * radius;
            
            piece.localPosition = new Vector3(x, 0, z);
            
            // The pieces need to face outward or inward to form a cake
            // Let's rotate them so they point towards the center
            piece.localRotation = Quaternion.Euler(0, i * 60f, 0);
        }
        
        Debug.Log("Sapin Sapin pieces adjusted.");
    }
}