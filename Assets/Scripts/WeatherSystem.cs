using UnityEngine;

public enum WeatherType { Clear, Rain, Heat, Wind }

public class WeatherSystem : MonoBehaviour
{
    public WeatherType CurrentWeather { get; private set; }

    [Header("Modifiers")]
    public float rainMoveSpeedModifier = 0.8f;
    public float heatStaminaDrainMultiplier = 1.5f;
    public float windCookSpeedModifier = 1.3f; // Increases cook time

    private void Start()
    {
        GenerateRandomWeather();
    }

    public void GenerateRandomWeather()
    {
        CurrentWeather = (WeatherType)Random.Range(0, 4);
        Debug.Log($"Today's weather is: {CurrentWeather}");
        ApplyWeatherEffects();
    }

    private void ApplyWeatherEffects()
    {
        if (GameManager.Instance == null) return;

        StaminaSystem stamina = GameManager.Instance.GetComponentInChildren<StaminaSystem>();
        PlayerController player = GameManager.Instance.GetComponentInChildren<PlayerController>();

        // Reset
        if(stamina != null) stamina.drainRate = 5f;
        if(player != null) player.baseMoveSpeed = 3.0f;

        switch (CurrentWeather)
        {
            case WeatherType.Rain:
                if(player != null) player.baseMoveSpeed *= rainMoveSpeedModifier;
                break;
            case WeatherType.Heat:
                if(stamina != null) stamina.drainRate *= heatStaminaDrainMultiplier;
                break;
            case WeatherType.Wind:
                // Affects cooking system (handled inside CookingSystem by checking WeatherSystem state)
                break;
        }
    }

    public void SetWeather(WeatherType type)
    {
        CurrentWeather = type;
        Debug.Log($"Weather set to: {CurrentWeather}");
        ApplyWeatherEffects();
    }

    public float GetCookModifier()
    {
        if (CurrentWeather == WeatherType.Wind) return windCookSpeedModifier;
        return 1f;
    }
}
