using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LinearSkillCheckController : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform pointer;
    public Image successZoneImage;
    public Image amazingZoneImage;
    public Image goodZoneImage;
    public TextMeshProUGUI feedbackText;
    public RectTransform barBackground;

    [Header("Settings")]
    public float moveSpeed = 2f;
    public float successZoneWidth = 0.15f; // 0 to 1 range
    public KeyCode interactionKey = KeyCode.Space;

    private float successZoneStart; // 0 to 1
    private bool isRunning = false;
    private float barWidth;

    void Start()
    {
        barWidth = barBackground.rect.width;
        StartSkillCheck();
    }

    public void StartSkillCheck()
    {
        isRunning = true;
        successZoneStart = Random.Range(0.1f, 0.9f - successZoneWidth);
        
        // Position success zones
        UpdateZonePosition(successZoneImage, successZoneStart, successZoneWidth);
        
        // Amazing zone is the center 30% of the success zone
        float amazingWidth = successZoneWidth * 0.3f;
        float amazingStart = successZoneStart + (successZoneWidth / 2f) - (amazingWidth / 2f);
        UpdateZonePosition(amazingZoneImage, amazingStart, amazingWidth);

        // Good zone is 1.5x the success zone width
        float goodWidth = successZoneWidth * 1.5f;
        float goodStart = successZoneStart + (successZoneWidth / 2f) - (goodWidth / 2f);
        UpdateZonePosition(goodZoneImage, goodStart, goodWidth);

        if (feedbackText != null) 
        {
            feedbackText.text = "READY!";
            feedbackText.color = Color.white;
        }
    }

    private void UpdateZonePosition(Image img, float start, float width)
    {
        if (img != null)
        {
            var rect = img.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(Mathf.Clamp01(start), 0.5f);
            rect.anchorMax = new Vector2(Mathf.Clamp01(start + width), 0.5f);
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(0, 40f); // Fixed height
            rect.offsetMin = new Vector2(0, -20f);
            rect.offsetMax = new Vector2(0, 20f);
        }
    }

    void Update()
    {
        if (!isRunning) return;

        // Ping-pong movement between 0 and 1
        float t = Mathf.PingPong(Time.time * moveSpeed, 1f);
        
        // Update pointer position
        if (pointer != null)
        {
            pointer.anchorMin = new Vector2(t, 0.5f);
            pointer.anchorMax = new Vector2(t, 0.5f);
            pointer.anchoredPosition = Vector2.zero;
        }

        if (Input.GetKeyDown(interactionKey))
        {
            CheckResult(t);
        }
    }

    void CheckResult(float currentPos)
    {
        isRunning = false;

        float center = successZoneStart + (successZoneWidth / 2f);
        float diff = Mathf.Abs(currentPos - center);
        
        // Thresholds are half-widths (distance from center)
        float amazingThreshold = (successZoneWidth * 0.3f) / 2f; 
        float greatThreshold = successZoneWidth / 2f;
        float goodThreshold = (successZoneWidth * 1.5f) / 2f; 

        if (diff <= amazingThreshold)
        {
            feedbackText.text = "AMAZING!";
            feedbackText.color = new Color(1f, 0.84f, 0f); // Gold
        }
        else if (diff <= greatThreshold)
        {
            feedbackText.text = "GREAT!";
            feedbackText.color = Color.green;
        }
        else if (diff <= goodThreshold)
        {
            feedbackText.text = "GOOD!";
            feedbackText.color = Color.yellow;
        }
        else
        {
            feedbackText.text = "TRY AGAIN!";
            feedbackText.color = Color.red;
        }

        Invoke("StartSkillCheck", 1.5f);
    }
}
