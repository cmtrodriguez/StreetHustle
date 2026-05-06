#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEngine;

public static class SaveActiveSceneOnce
{
    public static void Execute()
    {
        var scene = EditorSceneManager.GetActiveScene();
        EditorSceneManager.SaveScene(scene);
        Debug.Log($"Saved scene: {scene.path}");
    }
}
#endif
