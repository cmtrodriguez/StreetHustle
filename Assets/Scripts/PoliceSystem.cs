using UnityEngine;

public class PoliceSystem : MonoBehaviour
{
    [Header("Detection Settings")]
    public float maxDetection = 100f;
    public float detectionRate = 10f; // per second in risk zone
    public float decayRate = 5f; // per second when safe
    
    private float currentDetection = 0f;
    private bool inRiskZone = false;
    private bool isCaught = false;

    private void Update()
    {
        if (isCaught || GameManager.Instance.CurrentState != GameState.Playing) return;

        if (inRiskZone)
        {
            currentDetection += detectionRate * Time.deltaTime;
            currentDetection = Mathf.Clamp(currentDetection, 0, maxDetection);
            
            if (currentDetection >= maxDetection)
            {
                CatchPlayer();
            }
        }
        else
        {
            currentDetection -= decayRate * Time.deltaTime;
            currentDetection = Mathf.Clamp(currentDetection, 0, maxDetection);
        }

        UpdateUI();
    }

    private void CatchPlayer()
    {
        isCaught = true;
        GameManager.Instance.TriggerPoliceCaught();
    }

    private void UpdateUI()
    {
        GameManager.Instance.uiManager.UpdateDetectionUI(currentDetection / maxDetection);
    }

    // Call these via triggers on Risk Zones
    public void SetInRiskZone(bool state)
    {
        inRiskZone = state;
    }
}
