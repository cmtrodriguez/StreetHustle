using UnityEngine;

public class EconomySystem : MonoBehaviour
{
    [Header("Economy Stats")]
    public float startingMoney = 100f;
    
    private float currentMoney;
    private float dailySales;
    private float dailyExpenses;

    private void Start()
    {
        currentMoney = startingMoney;
        UpdateUI();
    }

    public void AddMoney(float amount)
    {
        currentMoney += amount;
        dailySales += amount;
        UpdateUI();
    }

    public bool SpendMoney(float amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            dailyExpenses += amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    private void UpdateUI()
    {
        GameManager.Instance.uiManager.UpdateMoneyUI(currentMoney);
    }

    public void ResetEconomy()
    {
        currentMoney = startingMoney;
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
