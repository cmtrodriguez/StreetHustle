using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UniversalMenuController : MonoBehaviour
{
    private VisualElement _pauseOverlay;
    private Button _resumeButton;
    private Button _quitButton;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var menuButton = root.Q<Button>("MenuButton");
        _pauseOverlay = root.Q<VisualElement>("PauseOverlay");
        _resumeButton = root.Q<Button>("ResumeButton");
        _quitButton = root.Q<Button>("QuitButton");
        
        if (menuButton != null)
        {
            menuButton.clicked += OnMenuClicked;
        }

        if (_resumeButton != null)
        {
            _resumeButton.clicked += ResumeGame;
        }

        if (_quitButton != null)
        {
            _quitButton.clicked += () => 
            {
                Time.timeScale = 1f; // Ensure time is unpaused before loading
                SceneManager.LoadScene("Main Menu and Loading Screen");
            };
        }
    }

    private void OnMenuClicked()
    {
        if (_pauseOverlay != null)
        {
            _pauseOverlay.style.display = DisplayStyle.Flex;
            Time.timeScale = 0f; // Pause the game
        }
    }

    private void ResumeGame()
    {
        if (_pauseOverlay != null)
        {
            _pauseOverlay.style.display = DisplayStyle.None;
            Time.timeScale = 1f; // Resume the game
        }
    }
}
