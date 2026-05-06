using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class BuildMainMenuUIV17
{
    public static void Execute()
    {
        // Delete existing
        GameObject existing = GameObject.Find("MainMenuCanvasV2");
        if (existing != null) GameObject.DestroyImmediate(existing);

        // Create Canvas
        GameObject canvasGO = new GameObject("MainMenuCanvasV2");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1344, 768);
        canvasGO.AddComponent<GraphicRaycaster>();

        // Load Sprites
        Sprite bgSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/BackgroundClean.png");
        Sprite logoSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/Logo.png");
        Sprite startBtnSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/StartButtonClean.png");
        Sprite normalBtnSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/NormalButtonClean.png");
        
        Sprite iconPlay = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/IconPlay.png");
        Sprite iconBook = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/IconBook.png");
        Sprite iconGear = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/IconGear.png");
        Sprite iconExit = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/IconExit.png");

        // Background
        GameObject bgGO = new GameObject("Background");
        bgGO.transform.SetParent(canvasGO.transform, false);
        Image bgImg = bgGO.AddComponent<Image>();
        bgImg.sprite = bgSprite;
        bgImg.type = Image.Type.Simple;
        RectTransform bgRect = bgGO.GetComponent<RectTransform>();
        bgRect.sizeDelta = new Vector2(1344, 768);
        bgRect.anchoredPosition = new Vector2(0, 0);

        // Logo
        GameObject logoGO = new GameObject("Logo");
        logoGO.transform.SetParent(canvasGO.transform, false);
        Image logoImg = logoGO.AddComponent<Image>();
        logoImg.sprite = logoSprite;
        logoImg.type = Image.Type.Simple;
        RectTransform logoRect = logoGO.GetComponent<RectTransform>();
        logoRect.sizeDelta = new Vector2(500, 320); // Scaled down slightly
        logoRect.anchoredPosition = new Vector2(-380, 180); // Positioned on the left

        // Buttons
        CreateButton(canvasGO.transform, "StartWeekButton", "START WEEK", "Begin your 6-day hustle", new Vector2(-380, -30), new Vector2(400, 80), startBtnSprite, iconPlay);
        CreateButton(canvasGO.transform, "InstructionsButton", "INSTRUCTIONS", "How to play", new Vector2(-380, -120), new Vector2(400, 80), normalBtnSprite, iconBook);
        CreateButton(canvasGO.transform, "SettingsButton", "SETTINGS", "Audio, Video, Controls", new Vector2(-380, -210), new Vector2(400, 80), normalBtnSprite, iconGear);
        CreateButton(canvasGO.transform, "ExitButton", "EXIT", "Quit Game", new Vector2(-380, -300), new Vector2(400, 80), normalBtnSprite, iconExit);
    }

    static void CreateButton(Transform parent, string name, string mainText, string subText, Vector2 pos, Vector2 size, Sprite bgSprite, Sprite iconSprite)
    {
        GameObject btnGO = new GameObject(name);
        btnGO.transform.SetParent(parent, false);
        Image btnImg = btnGO.AddComponent<Image>();
        btnImg.sprite = bgSprite;
        btnImg.type = Image.Type.Sliced;
        Button btn = btnGO.AddComponent<Button>();
        
        RectTransform btnRect = btnGO.GetComponent<RectTransform>();
        btnRect.sizeDelta = size;
        btnRect.anchoredPosition = pos;

        // Icon
        if (iconSprite != null)
        {
            GameObject iconGO = new GameObject("Icon");
            iconGO.transform.SetParent(btnGO.transform, false);
            Image iconImg = iconGO.AddComponent<Image>();
            iconImg.sprite = iconSprite;
            iconImg.type = Image.Type.Simple;
            
            RectTransform iconRect = iconGO.GetComponent<RectTransform>();
            iconRect.anchorMin = new Vector2(0, 0.5f);
            iconRect.anchorMax = new Vector2(0, 0.5f);
            iconRect.sizeDelta = new Vector2(40, 40);
            iconRect.anchoredPosition = new Vector2(40, 0); // 40px from the left edge
        }

        // Main Text
        GameObject mainTextGO = new GameObject("MainText");
        mainTextGO.transform.SetParent(btnGO.transform, false);
        TextMeshProUGUI mainTmp = mainTextGO.AddComponent<TextMeshProUGUI>();
        mainTmp.text = mainText;
        mainTmp.fontSize = 22;
        mainTmp.fontStyle = FontStyles.Bold;
        mainTmp.alignment = TextAlignmentOptions.Left;
        mainTmp.color = Color.white;
        
        RectTransform mainTextRect = mainTextGO.GetComponent<RectTransform>();
        mainTextRect.anchorMin = new Vector2(0, 0.5f);
        mainTextRect.anchorMax = new Vector2(1, 0.5f);
        mainTextRect.sizeDelta = new Vector2(-100, 30); // Padding
        mainTextRect.anchoredPosition = new Vector2(100, 10); // Shifted right to make room for icon, and up slightly

        // Sub Text
        GameObject subTextGO = new GameObject("SubText");
        subTextGO.transform.SetParent(btnGO.transform, false);
        TextMeshProUGUI subTmp = subTextGO.AddComponent<TextMeshProUGUI>();
        subTmp.text = subText;
        subTmp.fontSize = 13;
        subTmp.alignment = TextAlignmentOptions.Left;
        subTmp.color = new Color(0.9f, 0.9f, 0.9f, 1f);
        
        RectTransform subTextRect = subTextGO.GetComponent<RectTransform>();
        subTextRect.anchorMin = new Vector2(0, 0.5f);
        subTextRect.anchorMax = new Vector2(1, 0.5f);
        subTextRect.sizeDelta = new Vector2(-100, 30); // Padding
        subTextRect.anchoredPosition = new Vector2(100, -15); // Shifted right to make room for icon, and down slightly
    }
}