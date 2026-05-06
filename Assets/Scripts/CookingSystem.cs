using UnityEngine;
using System.Collections;

public class CookingSystem : MonoBehaviour
{
    [Header("Cooking Variables")]
    public float baseCookSpeed = 2.0f;
    
    private bool isCooking = false;
    private float currentCookTime = 0f;
    private string currentOrderTarget;

    private CustomerAI currentCustomer;

    private void Update()
    {
        // Simple input for testing the system
        if (Input.GetKeyDown(KeyCode.E) && !isCooking && currentCustomer != null)
        {
            StartCooking(GameManager.Instance.dayManager.GetCurrentMenu());
        }
    }

    public void AssignCustomer(CustomerAI customer)
    {
        currentCustomer = customer;
    }

    public void StartCooking(string foodType)
    {
        isCooking = true;
        currentCookTime = 0f;
        currentOrderTarget = foodType;
        StartCoroutine(CookRoutine());
    }

    private IEnumerator CookRoutine()
    {
        // Stamina affects cook speed
        float modifier = GameManager.Instance.GetComponentInChildren<StaminaSystem>()?.GetSpeedModifier() ?? 1f;
        float finalCookTime = baseCookSpeed / modifier; // Slower if stamina is low

        Debug.Log($"Cooking {currentOrderTarget}... takes {finalCookTime} seconds.");

        yield return new WaitForSeconds(finalCookTime); // Simulated preparation

        CompleteCooking();
    }

    private void CompleteCooking()
    {
        isCooking = false;
        float qualityScore = 100f; // Assume perfect input for this prototype

        if (currentCustomer != null)
        {
            currentCustomer.ReceiveFood(currentOrderTarget, qualityScore);
            currentCustomer = null;
        }
    }
}
