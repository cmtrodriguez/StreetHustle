using UnityEngine;
using UnityEditor;
using TMPro;

public class FixButtonLayout5
{
    public static void Execute()
    {
        string[] buttonNames = new string[]
        {
            "MainMenuCanvasV2/StartWeekButton",
            "MainMenuCanvasV2/InstructionsButton",
            "MainMenuCanvasV2/SettingsButton",
            "MainMenuCanvasV2/ExitButton"
        };

        foreach (string btnName in buttonNames)
        {
            GameObject btnObj = GameObject.Find(btnName);
            if (btnObj == null) continue;

            Transform contentContainer = btnObj.transform.Find("ContentContainer");
            if (contentContainer == null) continue;

            Transform icon = contentContainer.Find("Icon");
            if (icon != null)
            {
                RectTransform iconRect = icon.GetComponent<RectTransform>();
                iconRect.anchoredPosition = new Vector2(30, 0);
            }

            Transform mainText = contentContainer.Find("MainText");
            if (mainText != null)
            {
                RectTransform mtRect = mainText.GetComponent<RectTransform>();
                mtRect.offsetMin = new Vector2(70, 0);
                mtRect.offsetMax = new Vector2(-20, 30);
            }

            Transform subText = contentContainer.Find("SubText");
            if (subText != null)
            {
                RectTransform stRect = subText.GetComponent<RectTransform>();
                stRect.offsetMin = new Vector2(70, -30);
                stRect.offsetMax = new Vector2(-20, 0);
            }

            EditorUtility.SetDirty(btnObj);
        }
        
        Debug.Log("Button layouts fixed 5.");
    }
}
