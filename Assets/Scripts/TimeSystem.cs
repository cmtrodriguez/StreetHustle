using UnityEngine;

public enum TimePhase { Morning, Afternoon, Evening }

public class TimeSystem : MonoBehaviour
{
    [Header("Time Config (Seconds)")]
    public float dayDurationRealTime = 120f; // 2 minutes per in-game day
    
    private float currentTime;
    private bool isRunning;
    public TimePhase CurrentPhase { get; private set; }

    private void Update()
    {
        if (!isRunning) return;

        currentTime += Time.deltaTime;
        
        UpdatePhase();
        GameManager.Instance.uiManager.UpdateTimeUI(GetNormalizedTime(), CurrentPhase.ToString());

        if (currentTime >= dayDurationRealTime)
        {
            GameManager.Instance.EndDay();
        }
    }

    private void UpdatePhase()
    {
        float ratio = currentTime / dayDurationRealTime;
        if (ratio < 0.4f) CurrentPhase = TimePhase.Morning;
        else if (ratio < 0.7f) CurrentPhase = TimePhase.Afternoon;
        else CurrentPhase = TimePhase.Evening;
    }

    public void ResetTime()
    {
        currentTime = 0f;
        CurrentPhase = TimePhase.Morning;
    }

    public void PauseTime() => isRunning = false;
    public void ResumeTime() => isRunning = true;

    public float GetNormalizedTime()
    {
        return Mathf.Clamp01(currentTime / dayDurationRealTime);
    }
}
