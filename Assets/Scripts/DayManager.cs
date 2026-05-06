using UnityEngine;

public enum DayOfWeek { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday }

public class DayManager : MonoBehaviour
{
    public DayOfWeek CurrentDay { get; private set; }

    [Header("Daily Food Types Unlocked")]
    public string menuMonday = "Ice Scramble";
    public string menuTuesday = "Sorbetes";
    public string menuWednesday = "Tusok-Tusok";
    public string menuThursday = "Street Drinks";
    public string menuFriday = "Mixed Menu";
    public string menuSaturday = "Peak Day";

    public void ResetToMonday()
    {
        CurrentDay = DayOfWeek.Monday;
        GameManager.Instance.uiManager.UpdateDayUI(CurrentDay.ToString());
    }

    public bool AdvanceDay()
    {
        if (CurrentDay == DayOfWeek.Saturday)
        {
            return false; // Reached end of week
        }

        CurrentDay++;
        GameManager.Instance.uiManager.UpdateDayUI(CurrentDay.ToString());
        return true;
    }

    public string GetCurrentMenu()
    {
        switch (CurrentDay)
        {
            case DayOfWeek.Monday: return menuMonday;
            case DayOfWeek.Tuesday: return menuTuesday;
            case DayOfWeek.Wednesday: return menuWednesday;
            case DayOfWeek.Thursday: return menuThursday;
            case DayOfWeek.Friday: return menuFriday;
            case DayOfWeek.Saturday: return menuSaturday;
            default: return menuMonday;
        }
    }
}
