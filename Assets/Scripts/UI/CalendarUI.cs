using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CalendarUI : MonoBehaviour
{
    [Header("Global Headers")]
    public TextMeshProUGUI topWeekText;
    public TextMeshProUGUI topMonthText;

    [Header("Panel Headers")]
    public TextMeshProUGUI panelWeekText;
    public TextMeshProUGUI panelMonthText;

    [Header("Calendar Grid")]
    public List<CalendarDayItem> dayItems;

    [Header("Buttons")]
    public Button resetWeekButton;
    public Button continueHustleButton;

    private void Start()
    {
        // Button logic disabled for now as per user request
        /*
        if (resetWeekButton != null) resetWeekButton.onClick.AddListener(OnResetClicked);
        if (continueHustleButton != null) continueHustleButton.onClick.AddListener(OnContinueClicked);
        */

        UpdateCalendarUI();
    }

    [ContextMenu("Update UI")]
    public void UpdateCalendarUI()
    {
        string weekStr = "WEEK 1";
        string monthStr = "APRIL 1989";

        if (topWeekText != null) topWeekText.text = weekStr;
        if (topMonthText != null) topMonthText.text = monthStr;
        if (panelWeekText != null) panelWeekText.text = weekStr;
        if (panelMonthText != null) panelMonthText.text = monthStr;

        string[] dayNames = { "SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT" };
        string[] dayNumbers = { "2", "3", "4", "5", "6", "7", "8" };

        int currentDayIndex = 0; // Default for editor
        if (GameManager.Instance != null && GameManager.Instance.dayManager != null)
        {
            currentDayIndex = (int)GameManager.Instance.dayManager.CurrentDay;
        }

        if (dayItems == null) return;

        for (int i = 0; i < dayItems.Count; i++)
        {
            if (dayItems[i] == null) continue;
            if (i < dayNames.Length)
            {
                dayItems[i].SetDay(dayNames[i], dayNumbers[i]);
                
                if (i == 0)
                {
                    dayItems[i].SetActive(false);
                }
                else
                {
                    // If we have a real DayManager, use it. Otherwise, highlight MON (index 1)
                    bool isActive = (GameManager.Instance != null) ? (i == currentDayIndex + 1) : (i == 1);
                    dayItems[i].SetActive(isActive);
                }
            }
        }
    }

    private void OnResetClicked()
    {
        // Disabled for now
        Debug.Log("Reset Week Clicked (Logic not implemented yet)");
    }

    private void OnContinueClicked()
    {
        // Disabled for now
        Debug.Log("Continue Hustle Clicked (Logic not implemented yet)");
    }
}