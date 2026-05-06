using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LinearSkillCheckController : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform pointer;
    public Image successZoneImage;
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
        
        // Position success zone
        if (successZoneImage != null)
        {
            var rect = successZoneImage.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(successZoneStart, 0.5f);
            rect.anchorMax = new Vector2(successZoneStart + successZoneWidth, 0.5f);
            rect.offsetMin = new Vector2(0, -rect.sizeDelta.y / 2f);
            rect.offsetMax = new Vector2(0, rect.sizeDelta.y / 2f);
        }

        if (feedbackText != null) feedbackText.text = "READY!";
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

        bool isSuccess = currentPos >= successZoneStart && currentPos <= (successZoneStart + successZoneWidth);

        if (isSuccess)
        {
            feedbackText.text = "GREAT!";
            feedbackText.color = Color.green;
        }
        else
        {
            feedbackText.text = "FAILED!";
            feedbackText.color = Color.red;
        }

        Invoke("StartSkillCheck", 1.5f);
    }
}
