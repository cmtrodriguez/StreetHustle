using UnityEngine;

public class EconomySystem : MonoBehaviour
{
    [Header("Economy Stats")]
    public float startingMoney = 100f;
    
    public float CurrentMoney { get; private set; }
    private float dailySales;
    private float dailyExpenses;

    public void AddMoney(float amount)
    {
        CurrentMoney += amount;
        dailySales += amount;
        UpdateUI();
    }

    public bool SpendMoney(float amount)
    {
        if (CurrentMoney >= amount)
        {
            CurrentMoney -= amount;
            dailyExpenses += amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    private void UpdateUI()
    {
        if (GameManager.Instance != null && GameManager.Instance.uiManager != null)
            GameManager.Instance.uiManager.UpdateMoneyUI(CurrentMoney);
    }

    public void ResetEconomy()
    {
        CurrentMoney = startingMoney;
        dailySales = 0f;
        dailyExpenses = 0f;
        UpdateUI();
    }

    public void EndDayEconomy()
    {
        // Money carries over, but daily stats reset
        dailySales = 0f;
        dailyExpenses = 0f;
        UpdateUI();
    }

    public float GetDailySales() => dailySales;
    public float GetDailyExpenses() => dailyExpenses;
    public float GetDailyProfit() => dailySales - dailyExpenses;
}
