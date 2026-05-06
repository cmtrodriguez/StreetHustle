using UnityEditor;
using UnityEngine;

public class CheckNewModels
{
    public static void Execute()
    {
        AssetDatabase.Refresh();
        GameObject stall = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Models/wooden market stall 3d model.glb");
        GameObject wheel = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Models/wooden wagon wheel 3d model.glb");

        if (stall != null)
        {
            MeshFilter[] filters = stall.GetComponentsInChildren<MeshFilter>();
            Bounds b = new Bounds();
            bool first = true;
            foreach (var f in filters)
            {
                if (first) { b = f.sharedMesh.bounds; first = false; }
                else { b.Encapsulate(f.sharedMesh.bounds); }
            }
            Debug.Log("Stall Bounds: Center=" + b.center + " Size=" + b.size);
        }
        else
        {
            Debug.Log("Stall not found");
        }

        if (wheel != null)
        {
            MeshFilter[] filters = wheel.GetComponentsInChildren<MeshFilter>();
            Bounds b = new Bounds();
            bool first = true;
            foreach (var f in filters)
            {
                if (first) { b = f.sharedMesh.bounds; first = false; }
                else { b.Encapsulate(f.sharedMesh.bounds); }
            }
            Debug.Log("Wheel Bounds: Center=" + b.center + " Size=" + b.size);
        }
        else
        {
            Debug.Log("Wheel not found");
        }
    }
}