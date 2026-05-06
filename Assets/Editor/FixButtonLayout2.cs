using UnityEngine;
using UnityEditor;
using TMPro;

public class FixButtonLayout2
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

            RectTransform ccRect = contentContainer.GetComponent<RectTransform>();
            ccRect.anchorMin = new Vector2(0, 0);
            ccRect.anchorMax = new Vector2(1, 1);
            ccRect.offsetMin = new Vector2(0, 0);
            ccRect.offsetMax = new Vector2(0, 0);

            Transform icon = contentContainer.Find("Icon");
            if (icon != null)
            {
                RectTransform iconRect = icon.GetComponent<RectTransform>();
                iconRect.anchorMin = new Vector2(0, 0.5f);
                iconRect.anchorMax = new Vector2(0, 0.5f);
                iconRect.pivot = new Vector2(0.5f, 0.5f);
                iconRect.sizeDelta = new Vector2(40, 40);
                iconRect.anchoredPosition = new Vector2(60, 0);
            }

            Transform mainText = contentContainer.Find("MainText");
            if (mainText != null)
            {
                RectTransform mtRect = mainText.GetComponent<RectTransform>();
                mtRect.anchorMin = new Vector2(0, 0.5f);
                mtRect.anchorMax = new Vector2(1, 0.5f);
                mtRect.pivot = new Vector2(0, 0);
                mtRect.offsetMin = new Vector2(110, 0);
                mtRect.offsetMax = new Vector2(-20, 30);

                TextMeshProUGUI tmp = mainText.GetComponent<TextMeshProUGUI>();
                if (tmp != null)
                {
                    tmp.alignment = TextAlignmentOptions.BottomLeft;
                    tmp.fontSize = 22;
                }
            }

            Transform subText = contentContainer.Find("SubText");
            if (subText != null)
            {
                RectTransform stRect = subText.GetComponent<RectTransform>();
                stRect.anchorMin = new Vector2(0, 0.5f);
                stRect.anchorMax = new Vector2(1, 0.5f);
                stRect.pivot = new Vector2(0, 1);
                stRect.offsetMin = new Vector2(110, -30);
                stRect.offsetMax = new Vector2(-20, 0);

                TextMeshProUGUI tmp = subText.GetComponent<TextMeshProUGUI>();
                if (tmp != null)
                {
                    tmp.alignment = TextAlignmentOptions.TopLeft;
                    tmp.fontSize = 14;
                }
            }

            EditorUtility.SetDirty(btnObj);
        }
        
        Debug.Log("Button layouts fixed 2.");
    }
}
