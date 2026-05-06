using UnityEditor;
using UnityEngine;

public class CheckFBX
{
    public static void Execute()
    {
        string path = "Assets/Import/Sorbetes_Cart/Sorbetes_Cart.fbx";
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
        foreach (Object asset in assets)
        {
            Debug.Log(asset.name + " (" + asset.GetType().Name + ")");
        }
    }
}