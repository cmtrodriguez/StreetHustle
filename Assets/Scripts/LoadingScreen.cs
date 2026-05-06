using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject loadingPanel;
    public Slider progressBar;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI tipText;
    public CanvasGroup canvasGroup;

    [Header("Settings")]
    public float fadeDuration = 0.5f;

    private string[] loadingPhrases = new string[] {
        "HEATING UP THE GRILL...",
        "SETTING UP THE CART...",
        "READY TO HIT THE STREETS...",
        "PREPARING INGREDIENTS...",
        "SHARPENING SKEWERS..."
    };

    private string[] tips = new string[] {
        "TIP: Strategize and fit the food demand to the selected weather for bigger profit!",
        "TIP: Cook more skewers during lunch hours for a sales boost!",
        "TIP: Upgrading your grill speeds up skewering by 20%!",
        "TIP: Keep an eye out for cops, they can shut you down!"
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }
        
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // Setup random text
        if (titleText != null)
            titleText.text = loadingPhrases[Random.Range(0, loadingPhrases.Length)];
        if (tipText != null)
            tipText.text = tips[Random.Range(0, tips.Length)];

        // Show Loading Screen
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        // Fade in
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 0f;
            float time = 0;
            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
                yield return null;
            }
            canvasGroup.alpha = 1f;
        }

        // Start loading asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        
        float minimumLoadTime = 5f;
        float loadTimer = 0f;
        
        // Ensure scene is loaded properly and minimum time has passed
        while (!operation.isDone)
        {
            loadTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            // Artificial progress factor based on time
            float timeProgress = Mathf.Clamp01(loadTimer / minimumLoadTime);
            float displayProgress = Mathf.Min(progress, timeProgress);
            
            if (progressBar != null)
                progressBar.value = displayProgress;
            
            if (progressText != null)
                progressText.text = $"Loading {(displayProgress * 100):0}%... Day 1 - Monday";

            if (operation.progress >= 0.9f && loadTimer >= minimumLoadTime)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        // Wait a small moment to ensure smooth transition
        yield return new WaitForSeconds(0.2f);

        // Optional: Call GameManager to initialize if we want to delay its start
        if (GameManager.Instance != null)
        {
            GameManager.Instance.InitializeGameAfterLoading();
        }

        // Fade out
        if (canvasGroup != null)
        {
            float time = 0;
            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
                yield return null;
            }
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }

        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }
}
