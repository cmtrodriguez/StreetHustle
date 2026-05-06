using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class ReplaceMainMenuUI
{
    public static void Execute()
    {
        // Delete old MainMenuCanvas
        GameObject oldCanvas = GameObject.Find("MainMenuCanvas");
        if (oldCanvas != null) GameObject.DestroyImmediate(oldCanvas);

        // Build the new one
        BuildMainMenuUIV23.Execute();
    }
}