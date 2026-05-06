using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCheckController : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform pointer;
    public Image successZoneImage;
    public RectTransform successZoneContainer;
    public TextMeshProUGUI feedbackText;
    public GameObject centerPrompt;
    public Image ringImage;

    [Header("Settings")]
    public float rotationSpeed = 200f;
    public float successZoneWidth = 60f; // degrees
    public KeyCode interactionKey = KeyCode.Space;

    private float successZoneStartAngle; // Visual angle (0-360 CW)
    private bool isRunning = false;

    void Start()
    {
        StartSkillCheck();
    }

    public void StartSkillCheck()
    {
        isRunning = true;
        successZoneStartAngle = Random.Range(45f, 315f);
        
        if (successZoneContainer != null) 
            successZoneContainer.localRotation = Quaternion.Euler(0, 0, -successZoneStartAngle);
        
        if (successZoneImage != null) {
            successZoneImage.fillAmount = successZoneWidth / 360f;
            successZoneImage.color = Color.white;
        }
        
        if (pointer != null)
            pointer.localRotation = Quaternion.identity;
        
        if (feedbackText != null) feedbackText.text = "READY!";
        if (centerPrompt != null) centerPrompt.SetActive(true);
        if (ringImage != null) ringImage.color = Color.white;
    }

    void Update()
    {
        if (!isRunning) return;
        pointer.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        if (Input.GetKeyDown(interactionKey)) CheckResult();
    }

    void CheckResult()
    {
        isRunning = false;
        float currentEulerZ = pointer.localEulerAngles.z;
        float visualCenterCW = successZoneStartAngle + (successZoneWidth / 2f);
        float targetCenterEuler = (360f - visualCenterCW) % 360f;
        float angleDiff = Mathf.Abs(Mathf.DeltaAngle(currentEulerZ, targetCenterEuler));
        
        if (angleDiff <= (successZoneWidth / 2f))
        {
            Success();
        }
        else
        {
            Fail();
        }
    }

    void Success()
    {
        if (ringImage != null) ringImage.color = Color.green;
        if (successZoneImage != null) successZoneImage.color = Color.green;
        Invoke("ResetCheck", 0.5f);
    }

    void Fail()
    {
        if (ringImage != null) ringImage.color = Color.red;
        if (successZoneImage != null) successZoneImage.color = Color.red;
        Invoke("ResetCheck", 0.5f);
    }

    void ResetCheck()
    {
        StartSkillCheck();
    }
}
