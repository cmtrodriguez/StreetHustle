using UnityEngine;
using UnityEditor;

public class SetupSapinSapin
{
    public static void Execute()
    {
        GameObject plate = GameObject.Find("New_Wooden_Cart/banana leaf plate 3d model");
        if (plate == null)
        {
            Debug.LogError("Plate not found");
            return;
        }

        GameObject cakeGroup = new GameObject("SapinSapinCake");
        cakeGroup.transform.SetParent(plate.transform, false);
        cakeGroup.transform.localPosition = new Vector3(0, 0.2f, 0); // slightly above the plate
        // The plate is rotated at (0, 0, 270), so its local Y is world X, local X is world -Y.
        // Wait, let's just parent it to the cart and position it above the plate to avoid weird local rotations.
        cakeGroup.transform.SetParent(plate.transform.parent, false);
        
        // Plate world position
        Vector3 platePos = plate.transform.position;
        // Plate bounds max Y
        float plateTopY = plate.GetComponent<Renderer>().bounds.max.y;
        
        cakeGroup.transform.position = new Vector3(platePos.x, plateTopY, platePos.z);

        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Models/sapin sapin.glb");
        
        float scale = 0.2f; // scale down to fit the plate
        
        for (int i = 0; i < 6; i++)
        {
            GameObject piece = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            piece.name = "SapinSapinPiece_" + i;
            piece.transform.SetParent(cakeGroup.transform, false);
            
            // Rotate each piece by 60 degrees around Y axis
            piece.transform.localRotation = Quaternion.Euler(0, i * 60f, 0);
            piece.transform.localScale = new Vector3(scale, scale, scale);
            
            // Add physics
            MeshCollider mc = piece.AddComponent<MeshCollider>();
            mc.convex = true;
            
            Rigidbody rb = piece.AddComponent<Rigidbody>();
            rb.mass = 0.1f;
            
            // Add FixedJoint to attach to the plate so it doesn't fall off when moving
            FixedJoint joint = piece.AddComponent<FixedJoint>();
            joint.connectedBody = plate.GetComponent<Rigidbody>();
        }
        
        // Also destroy the TempSapinSapin if it exists
        GameObject temp = GameObject.Find("TempSapinSapin");
        if (temp != null) GameObject.DestroyImmediate(temp);
        
        Debug.Log("Sapin Sapin cake created and placed on the plate.");
    }
}