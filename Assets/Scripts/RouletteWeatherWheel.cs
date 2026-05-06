using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RouletteWeatherWheel : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform wheelTransform;
    public Button spinButton;
    public GameObject popupContainer;
    public Image popupIcon;
    public Button prepareStockButton;
    public TMPro.TextMeshProUGUI weatherResultText;

    [Header("Weather Sprites")]
    public Sprite sunnySprite;
    public Sprite cloudySprite;
    public Sprite rainySprite;

    [Header("Settings")]
    public float spinDuration = 5f;
    public float maxSpinSpeed = 1000f;
    public float popupDisplayTime = 3f;
    public AnimationCurve spinCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private bool isSpinning = false;
    private WeatherSystem weatherSystem;

    private void Start()
    {
        weatherSystem = FindObjectOfType<WeatherSystem>();
        if (spinButton != null)
        {
            spinButton.onClick.AddListener(Spin);
        }
        if (popupContainer != null) popupContainer.SetActive(false);
        if (prepareStockButton != null) prepareStockButton.onClick.AddListener(OnPrepareStockClicked);
    }

    public void Spin()
    {
        if (isSpinning) return;
        StartCoroutine(SpinRoutine());
    }

    private IEnumerator SpinRoutine()
    {
        isSpinning = true;
        if (spinButton != null) spinButton.interactable = false;

        float startRotation = wheelTransform.eulerAngles.z;
        float randomExtraSpin = Random.Range(5, 10) * 360f; // Spin at least 5 times
        float randomTargetAngle = Random.Range(0f, 360f);
        float totalRotation = randomExtraSpin + randomTargetAngle;

        float elapsed = 0f;
        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / spinDuration;
            float curveValue = spinCurve.Evaluate(t);
            float currentRotation = startRotation + (totalRotation * curveValue);
            wheelTransform.eulerAngles = new Vector3(0, 0, -currentRotation); // Negative for clockwise spin
            yield return null;
        }

        // Snap to exact end
        float finalAngle = (startRotation + totalRotation) % 360f;
        wheelTransform.eulerAngles = new Vector3(0, 0, -finalAngle);

        DetermineResult(finalAngle);

        isSpinning = false;
        // Removed re-enabling spinButton here to wait for ResetRoulette
    }

    private void DetermineResult(float angle)
    {
        // Angle 0-360 mapped to sections
        // Section 1: 0-120 (Sun)
        // Section 2: 120-240 (Storm)
        // Section 3: 240-360 (Cloud)
        
        WeatherType result;
        if (angle < 120) result = WeatherType.Heat; // Sun
        else if (angle < 240) result = WeatherType.Wind; // Cloud
        else result = WeatherType.Rain; // Storm

        Debug.Log($"Wheel stopped at {angle:F2}. Result: {result}");
        
        if (weatherSystem != null)
        {
            weatherSystem.SetWeather(result);
        }

        ShowPopup(result);
    }

    private void ShowPopup(WeatherType weather)
    {
        if (popupContainer == null || popupIcon == null) return;

        Sprite selectedSprite = null;
        string weatherName = "";

        switch (weather)
        {
            case WeatherType.Heat: 
                selectedSprite = sunnySprite; 
                weatherName = "Sunny";
                break;
            case WeatherType.Wind: 
                selectedSprite = cloudySprite; 
                weatherName = "Cloudy";
                break;
            case WeatherType.Rain: 
                selectedSprite = rainySprite; 
                weatherName = "Rainy";
                break;
        }

        if (selectedSprite != null)
        {
            popupIcon.sprite = selectedSprite;
            if (weatherResultText != null) weatherResultText.text = weatherName;
            popupContainer.SetActive(true);
        }
    }


    public void ResetRoulette()
    {
        if (popupContainer != null) popupContainer.SetActive(false);
        if (spinButton != null) spinButton.interactable = true;
        isSpinning = false;
    }

    private void OnPrepareStockClicked()
    {
        Debug.Log("Prepare Stock Clicked! Button is now disabled.");
        if (prepareStockButton != null)
        {
            prepareStockButton.interactable = false;
        }
        // Button directs to nowhere as requested
    }
}
