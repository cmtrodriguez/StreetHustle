#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public static class SetupTransitionOverlayInScene
{
    public static void Execute()
    {
        var scene = EditorSceneManager.GetActiveScene();

        // Root
        var root = GameObject.Find("TransitionOverlay");
        if (root == null)
        {
            root = new GameObject("TransitionOverlay");
            Undo.RegisterCreatedObjectUndo(root, "Create TransitionOverlay");
        }

        // VideoPlayer + controller
        var vp = root.GetComponent<VideoPlayer>();
        if (vp == null) vp = Undo.AddComponent<VideoPlayer>(root);

        var overlay = root.GetComponent<TransitionVideoOverlay>();
        if (overlay == null) overlay = Undo.AddComponent<TransitionVideoOverlay>(root);

        // Canvas
        var canvasGO = root.transform.Find("Canvas")?.gameObject;
        if (canvasGO == null)
        {
            canvasGO = new GameObject("Canvas");
            Undo.RegisterCreatedObjectUndo(canvasGO, "Create TransitionOverlay Canvas");
            canvasGO.transform.SetParent(root.transform, false);
        }

        var canvas = canvasGO.GetComponent<Canvas>();
        if (canvas == null) canvas = Undo.AddComponent<Canvas>(canvasGO);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        var scaler = canvasGO.GetComponent<CanvasScaler>();
        if (scaler == null) scaler = Undo.AddComponent<CanvasScaler>(canvasGO);
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        var raycaster = canvasGO.GetComponent<GraphicRaycaster>();
        if (raycaster == null) raycaster = Undo.AddComponent<GraphicRaycaster>(canvasGO);

        // CanvasGroup
        var cg = canvasGO.GetComponent<CanvasGroup>();
        if (cg == null) cg = Undo.AddComponent<CanvasGroup>(canvasGO);

        // RawImage
        var rawGO = canvasGO.transform.Find("TransitionVideo")?.gameObject;
        if (rawGO == null)
        {
            rawGO = new GameObject("TransitionVideo", typeof(RectTransform));
            Undo.RegisterCreatedObjectUndo(rawGO, "Create Transition RawImage");
            rawGO.transform.SetParent(canvasGO.transform, false);
        }

        var rt = rawGO.GetComponent<RectTransform>();
        if (rt == null) rt = Undo.AddComponent<RectTransform>(rawGO);
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.pivot = new Vector2(0.5f, 0.5f);

        var raw = rawGO.GetComponent<RawImage>();
        if (raw == null) raw = Undo.AddComponent<RawImage>(rawGO);
        raw.raycastTarget = true;

        // Material
        var shader = Shader.Find("UI/ChromaKeyBlack");
        if (shader != null)
        {
            const string matFolder = "Assets/Materials";
            const string matPath = "Assets/Materials/ChromaKeyBlackUI_Mat.mat";

            if (!AssetDatabase.IsValidFolder(matFolder))
                AssetDatabase.CreateFolder("Assets", "Materials");

            var mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
            if (mat == null)
            {
                mat = new Material(shader) { name = "ChromaKeyBlackUI_Mat" };
                AssetDatabase.CreateAsset(mat, matPath);
            }

            raw.material = mat;
            overlay.chromaKeyMaterial = mat;
        }
        else
        {
            Debug.LogWarning("Shader UI/ChromaKeyBlack not found (material not created).");
        }

        // Assign references
        overlay.overlayCanvas = canvas;
        overlay.rawImage = raw;
        overlay.canvasGroup = cg;

        // Assign clip if present
        var clip = AssetDatabase.LoadAssetAtPath<VideoClip>("Assets/Models/Brush Stroke Animation Black Screen_Brush Stroke After Effects Free Template_Brush Stroke Transition.mp4");
        if (clip != null)
            overlay.clip = clip;
        else
            Debug.LogWarning("Transition video clip not found at expected path.");

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log("Transition overlay setup complete.");
    }
}
#endif
