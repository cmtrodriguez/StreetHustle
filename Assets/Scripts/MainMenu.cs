using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Settings")]
    public string gameSceneName = "GameScene";

    public void OnStartWeekClicked()
    {
        if (string.IsNullOrEmpty(gameSceneName))
        {
            Debug.LogError("MainMenu: No game scene name specified!");
            return;
        }

        if (LoadingScreen.Instance != null)
        {
            LoadingScreen.Instance.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogWarning("MainMenu: No LoadingScreen instance found. Loading scene directly: " + gameSceneName);
            SceneManager.LoadScene(gameSceneName);
        }
    }

    public void OnQuitClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
