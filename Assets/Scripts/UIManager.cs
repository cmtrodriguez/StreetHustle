using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("HUD Elements")]
    public Slider staminaBar;
    public TextMeshProUGUI timeOfDayText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI dayText;
    public Image detectionVignette;

    [Header("Screens")]
    public GameObject summaryScreen;
    public TextMeshProUGUI salesText;
    public TextMeshProUGUI expensesText;
    public TextMeshProUGUI profitText;
    
    public GameObject gameOverScreen;
    public TextMeshProUGUI gameOverReasonText;

    public void UpdateGameStateUI()
    {
        summaryScreen.SetActive(false);
        gameOverScreen.SetActive(false);
    }

    public void UpdateStamina(float current, float max)
    {
        if(staminaBar != null) staminaBar.value = current / max;
    }

    public void UpdateTimeUI(float normalizedTime, string phase)
    {
        if(timeOfDayText != null) timeOfDayText.text = $"Time: {phase}";
    }

    public void UpdateMoneyUI(float amount)
    {
        if(moneyText != null) moneyText.text = $"$: {amount.ToString("F2")}";
    }

    public void UpdateDayUI(string day)
    {
        if(dayText != null) dayText.text = $"Day: {day}";
    }

    public void UpdateDetectionUI(float normalizedDetection)
    {
        if(detectionVignette != null)
        {
            Color c = detectionVignette.color;
            c.a = normalizedDetection;
            detectionVignette.color = c;
        }
    }

    public void ShowSummaryScreen(float sales, float expenses, float profit)
    {
        summaryScreen.SetActive(true);
        if(salesText != null) salesText.text = $"Sales: +{sales}";
        if(expensesText != null) expensesText.text = $"Expenses: -{expenses}";
        if(profitText != null) profitText.text = $"Profit: {profit}";
    }

    public void OnContinueClicked()
    {
        GameManager.Instance.ContinueToNextDay();
    }

    public void ShowGameOver(string reason)
    {
        gameOverScreen.SetActive(true);
        if(gameOverReasonText != null) gameOverReasonText.text = reason;
    }

    public void ShowWeekComplete()
    {
        gameOverScreen.SetActive(true);
        if(gameOverReasonText != null) gameOverReasonText.text = "You Survived the Week!\nVendor Master!";
    }

    public void HideGameOver()
    {
        if(gameOverScreen != null) gameOverScreen.SetActive(false);
    }
}
