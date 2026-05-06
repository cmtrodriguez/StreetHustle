#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor-only helper: ensures the Game view is focused when entering Play Mode,
/// so keyboard input (e.g., Space) goes to the running game.
/// </summary>
[InitializeOnLoad]
public static class AutoFocusGameViewOnPlay
{
    static AutoFocusGameViewOnPlay()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state != PlayModeStateChange.EnteredPlayMode)
            return;

        EditorApplication.delayCall += FocusGameView;
    }

    static void FocusGameView()
    {
        try
        {
            var gameViewType = Type.GetType("UnityEditor.GameView, UnityEditor");
            if (gameViewType == null)
                return;

            var window = EditorWindow.GetWindow(gameViewType);
            window?.Focus();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"AutoFocusGameViewOnPlay failed: {e.Message}");
        }
    }
}
#endif
