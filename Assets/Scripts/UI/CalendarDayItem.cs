using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CalendarDayItem : MonoBehaviour
{
    [Header("UI References")]
    public Image dayNameBackground;
    public TextMeshProUGUI dayNameText;
    public Image dayNumberBackground;
    public TextMeshProUGUI dayNumberText;
    public Image glowEffect;
    public Image borderOutline;

    [Header("Colors - Active")]
    public Color activeNameColor = new Color(1f, 0.5f, 0f, 1f);
    public Color activeNumColor = new Color(1f, 0.4f, 0f, 1f);
    
    [Header("Colors - Inactive")]
    public Color inactiveNameColor = new Color(0.25f, 0.25f, 0.25f, 0.9f);
    public Color inactiveNumColor = new Color(0.35f, 0.35f, 0.35f, 0.8f);

    [Header("Text Colors")]
    public Color activeTextColor = Color.white;
    public Color inactiveTextColor = new Color(0.9f, 0.9f, 0.9f, 1f);

    [Header("Sprites")]
    public Sprite activeSprite;
    public Sprite inactiveSprite;

    public void SetDay(string name, string number)
    {
        if (dayNameText != null) dayNameText.text = name;
        if (dayNumberText != null) dayNumberText.text = number;
    }

    public void SetActive(bool isActive)
    {
        if (dayNumberBackground != null) 
        {
            dayNumberBackground.sprite = isActive ? activeSprite : inactiveSprite;
            dayNumberBackground.color = isActive ? activeNumColor : inactiveNumColor;
        }

        if (dayNameBackground != null)
        {
            dayNameBackground.enabled = true;
            dayNameBackground.color = isActive ? activeNameColor : inactiveNameColor;
        }
        
        if (dayNameText != null) dayNameText.color = isActive ? activeTextColor : inactiveTextColor;
        if (dayNumberText != null) dayNumberText.color = isActive ? activeTextColor : inactiveTextColor;

        if (glowEffect != null) glowEffect.gameObject.SetActive(isActive);
        if (borderOutline != null) borderOutline.gameObject.SetActive(isActive);
    }
}