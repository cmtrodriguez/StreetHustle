using UnityEngine;
using UnityEditor;
using TMPro;

public class FixButtonLayout
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
            if (btnObj == null)
            {
                Debug.LogWarning("Could not find " + btnName);
                continue;
            }

            Transform contentContainer = btnObj.transform.Find("ContentContainer");
            if (contentContainer == null) continue;

            RectTransform ccRect = contentContainer.GetComponent<RectTransform>();
            ccRect.anchorMin = new Vector2(0, 0);
            ccRect.anchorMax = new Vector2(1, 1);
            ccRect.offsetMin = new Vector2(0, 0);
            ccRect.offsetMax = new Vector2(0, 0);
            ccRect.pivot = new Vector2(0.5f, 0.5f);

            Transform icon = contentContainer.Find("Icon");
            if (icon != null)
            {
                RectTransform iconRect = icon.GetComponent<RectTransform>();
                iconRect.anchorMin = new Vector2(0, 0.5f);
                iconRect.anchorMax = new Vector2(0, 0.5f);
                iconRect.pivot = new Vector2(0.5f, 0.5f);
                iconRect.sizeDelta = new Vector2(40, 40);
                iconRect.anchoredPosition = new Vector2(50, 0);
            }

            Transform mainText = contentContainer.Find("MainText");
            if (mainText != null)
            {
                RectTransform mtRect = mainText.GetComponent<RectTransform>();
                mtRect.anchorMin = new Vector2(0, 0.5f);
                mtRect.anchorMax = new Vector2(1, 0.5f);
                mtRect.pivot = new Vector2(0.5f, 0.5f);
                mtRect.offsetMin = new Vector2(90, 0);
                mtRect.offsetMax = new Vector2(-30, 30);

                TextMeshProUGUI tmp = mainText.GetComponent<TextMeshProUGUI>();
                if (tmp != null)
                {
                    tmp.alignment = TextAlignmentOptions.BottomLeft;
                }
            }

            Transform subText = contentContainer.Find("SubText");
            if (subText != null)
            {
                RectTransform stRect = subText.GetComponent<RectTransform>();
                stRect.anchorMin = new Vector2(0, 0.5f);
                stRect.anchorMax = new Vector2(1, 0.5f);
                stRect.pivot = new Vector2(0.5f, 0.5f);
                stRect.offsetMin = new Vector2(90, -30);
                stRect.offsetMax = new Vector2(-30, 0);

                TextMeshProUGUI tmp = subText.GetComponent<TextMeshProUGUI>();
                if (tmp != null)
                {
                    tmp.alignment = TextAlignmentOptions.TopLeft;
                }
            }

            EditorUtility.SetDirty(btnObj);
        }
        
        Debug.Log("Button layouts fixed.");
    }
}
