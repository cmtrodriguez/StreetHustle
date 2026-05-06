using UnityEngine;
using System.Collections;

public enum GameState { Playing, Summary, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }
    
    [Header("Core Systems")]
    public DayManager dayManager;
    public EconomySystem economySystem;
    public TimeSystem timeSystem;
    public UIManager uiManager;

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
        }
    }

    private void Start()
    {
        // If there's no Loading Screen (e.g. testing in Editor), start automatically.
        if (FindObjectOfType<LoadingScreen>() == null)
        {
            StartNewWeek();
        }
    }

    public void InitializeGameAfterLoading()
    {
        StartNewWeek();
    }

    public void StartNewWeek()
    {
        economySystem.ResetEconomy();
        dayManager.ResetToMonday();
        StartDawn();
    }

    public void StartDawn()
    {
        CurrentState = GameState.Playing;
        timeSystem.ResetTime();
        timeSystem.ResumeTime();
        uiManager.UpdateGameStateUI();
    }

    public void EndDay()
    {
        CurrentState = GameState.Summary;
        timeSystem.PauseTime();
        uiManager.ShowSummaryScreen(economySystem.GetDailySales(), economySystem.GetDailyExpenses(), economySystem.GetDailyProfit());
    }

    public void ContinueToNextDay()
    {
        economySystem.EndDayEconomy();
        if (dayManager.AdvanceDay())
        {
            StartDawn();
        }
        else
        {
            // Week completed
            Debug.Log("Week Completed Successfully!");
            CurrentState = GameState.GameOver;
            uiManager.ShowWeekComplete();
        }
    }

    public void TriggerPoliceCaught()
    {
        CurrentState = GameState.GameOver;
        Debug.Log("Caught by Police! Resetting Week.");
        uiManager.ShowGameOver("Caught by Police!");
        StartCoroutine(ResetWithDelay(3f));
    }

    private IEnumerator ResetWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        uiManager.HideGameOver();
        StartNewWeek();
    }
}
