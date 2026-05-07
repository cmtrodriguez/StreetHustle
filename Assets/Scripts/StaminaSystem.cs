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
    
    public float CurrentStamina { get; private set; }
    private UIManager uiManager;

    private void Start()
    {
        CurrentStamina = maxStamina;
        uiManager = GameManager.Instance.uiManager;
        UpdateUI();
    }

    public void DrainStamina(float amount)
    {
        CurrentStamina -= amount * Time.deltaTime;
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, maxStamina);
        UpdateUI();
    }

    public void RegenStamina()
    {
        CurrentStamina += regenRate * Time.deltaTime;
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, maxStamina);
        UpdateUI();
    }

    public float GetSpeedModifier()
    {
        if (CurrentStamina <= speedPenaltyThreshold)
            return speedPenaltyMultiplier;
        return 1f;
    }

    private void UpdateUI()
    {
        if(uiManager != null)
        {
            uiManager.UpdateStamina(CurrentStamina, maxStamina);
        }
    }
}
