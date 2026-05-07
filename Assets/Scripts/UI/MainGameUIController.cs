using UnityEngine;
using UnityEngine.UIElements;

public class MainGameUIController : MonoBehaviour
{
    [Header("System References")]
    public EconomySystem economySystem;
    public TimeSystem timeSystem;
    public WeatherSystem weatherSystem;
    public StaminaSystem staminaSystem;
    public DayManager dayManager;

    private VisualElement root;
    private Label dayLabel;
    private Label weatherLabel;
    private VisualElement weatherIcon;
    private Label moneyLabel;
    private VisualElement staminaFill;

    [Header("Weather Sprites")]
    public Sprite sunnySprite;
    public Sprite cloudySprite;
    public Sprite rainySprite;
    private Button bellButton;
    private Button serveButton;
    private Button restButton;
    private Button endDayButton;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        // Find systems if not assigned
        if (economySystem == null) economySystem = FindObjectOfType<EconomySystem>();
        if (timeSystem == null) timeSystem = FindObjectOfType<TimeSystem>();
        if (weatherSystem == null) weatherSystem = FindObjectOfType<WeatherSystem>();
        if (staminaSystem == null) staminaSystem = FindObjectOfType<StaminaSystem>();
        if (dayManager == null) dayManager = FindObjectOfType<DayManager>();

        // Query elements
        dayLabel = root.Q<Label>("DayLabel");
        weatherLabel = root.Q<Label>("WeatherLabel");
        weatherIcon = root.Q<VisualElement>("WeatherIcon");
        moneyLabel = root.Q<Label>("MoneyLabel");
        staminaFill = root.Q<VisualElement>("StaminaFill");
        
        // Set default icon to verify visibility
        if (weatherIcon != null && sunnySprite != null)
        {
            weatherIcon.style.backgroundImage = new StyleBackground(sunnySprite.texture);
        }
        
        
        bellButton = root.Q<Button>("BellButton");
        serveButton = root.Q<Button>("ServeButton");
        restButton = root.Q<Button>("RestButton");
        endDayButton = root.Q<Button>("EndDayButton");

        // Register callbacks
        if (bellButton != null) bellButton.clicked += OnBellClicked;
        if (serveButton != null) serveButton.clicked += OnServeClicked;
        if (restButton != null) restButton.clicked += OnRestClicked;
        if (endDayButton != null) endDayButton.clicked += OnEndDayClicked;

        // Start update loop
        StartCoroutine(UpdateUILoop());
    }

    private System.Collections.IEnumerator UpdateUILoop()
    {
        while (true)
        {
            UpdateStatusUI();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void UpdateStatusUI()
    {
        if (economySystem != null) UpdateMoney(economySystem.CurrentMoney);
        if (staminaSystem != null) UpdateStamina(staminaSystem.CurrentStamina / staminaSystem.maxStamina * 100f);
        if (weatherSystem != null) UpdateWeather(weatherSystem.CurrentWeather);
        
        if (dayManager != null && dayLabel != null)
        {
            dayLabel.text = $"[ ] {dayManager.CurrentDay} - {dayManager.GetCurrentMenu()}";
        }
    }

    private void UpdateWeather(WeatherType weather)
    {
        if (weatherLabel != null) weatherLabel.text = weather.ToString();
        if (weatherIcon != null)
        {
            Sprite targetSprite = weather switch
            {
                WeatherType.Clear => sunnySprite,
                WeatherType.Rain => rainySprite,
                WeatherType.Heat => sunnySprite,
                WeatherType.Wind => cloudySprite,
                _ => sunnySprite
            };

            if (targetSprite != null)
            {
                weatherIcon.style.backgroundImage = new StyleBackground(targetSprite.texture);
            }
        }
    }


    private void OnBellClicked() => Debug.Log("Ring Bell Clicked");
    private void OnServeClicked() => Debug.Log("Serve Clicked");
    private void OnRestClicked() => Debug.Log("Rest Clicked");
    private void OnEndDayClicked() => Debug.Log("End Day Clicked");

    // Public methods to update UI from systems
    public void UpdateMoney(float amount)
    {
        if (moneyLabel != null) moneyLabel.text = $"$ P{amount:F2}";
    }

    public void UpdateStamina(float percentage)
    {
        if (staminaFill != null) staminaFill.style.width = Length.Percent(Mathf.Clamp(percentage, 0, 100));
    }
}
