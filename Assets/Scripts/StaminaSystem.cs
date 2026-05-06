using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float drainRate = 5f;
    public float regenRate = 10f;
    
    [Header("Debuff Settings")]
    public float speedPenaltyThreshold = 20f;
    public float speedPenaltyMultiplier = 0.5f;
    
    private float currentStamina;
    private UIManager uiManager;

    private void Start()
    {
        currentStamina = maxStamina;
        uiManager = GameManager.Instance.uiManager;
        UpdateUI();
    }

    public void DrainStamina(float amount)
    {
        currentStamina -= amount * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        UpdateUI();
    }

    public void RegenStamina()
    {
        currentStamina += regenRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        UpdateUI();
    }

    public float GetSpeedModifier()
    {
        if (currentStamina <= speedPenaltyThreshold)
            return speedPenaltyMultiplier;
        return 1f;
    }

    private void UpdateUI()
    {
        if(uiManager != null)
        {
            uiManager.UpdateStamina(currentStamina, maxStamina);
        }
    }
}
