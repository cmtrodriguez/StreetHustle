using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class BuildMainMenuUI
{
    public static void Execute()
    {
        // Delete existing
        GameObject existing = GameObject.Find("MainMenuCanvas");
        if (existing != null) GameObject.DestroyImmediate(existing);

        // Create Canvas
        GameObject canvasGO = new GameObject("MainMenuCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1536, 857);
        canvasGO.AddComponent<GraphicRaycaster>();

        // Load Sprites
        Sprite bgSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_UI_COPLAY_GENERATED/MainMenu/Sprites/Background.png");
        Sprite btnSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_UI_COPLAY_GENERATED/MainMenu/Sprites/Button.png");

        // Background
        GameObject bgGO = new GameObject("Background");
        bgGO.transform.SetParent(canvasGO.transform, false);
        Image bgImg = bgGO.AddComponent<Image>();
        bgImg.sprite = bgSprite;
        bgImg.type = Image.Type.Simple;
        RectTransform bgRect = bgGO.GetComponent<RectTransform>();
        bgRect.sizeDelta = new Vector2(1536, 857);
        bgRect.anchoredPosition = new Vector2(0, 0);

        // Title
        GameObject titleGO = new GameObject("TitleText");
        titleGO.transform.SetParent(canvasGO.transform, false);
        TextMeshProUGUI titleText = titleGO.AddComponent<TextMeshProUGUI>();
        titleText.text = "STREET\nHUSTLE";
        titleText.fontSize = 130;
        titleText.fontStyle = FontStyles.Bold;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.enableVertexGradient = true;
        titleText.colorGradient = new VertexGradient(
            new Color32(255, 255, 255, 255), // Top Left (White)
            new Color32(255, 255, 255, 255), // Top Right
            new Color32(255, 200, 0, 255),   // Bottom Left (Yellow/Orange)
            new Color32(255, 200, 0, 255)    // Bottom Right
        );
        
        Shadow titleShadow = titleGO.AddComponent<Shadow>();
        titleShadow.effectColor = new Color32(200, 80, 0, 255);
        titleShadow.effectDistance = new Vector2(4, -6);
        
        Outline titleOutline = titleGO.AddComponent<Outline>();
        titleOutline.effectColor = new Color32(150, 50, 0, 255);
        titleOutline.effectDistance = new Vector2(3, -3);

        RectTransform titleRect = titleGO.GetComponent<RectTransform>();
        titleRect.sizeDelta = new Vector2(800, 300);
        titleRect.anchoredPosition = new Vector2(0, 260);

        // Buttons
        CreateButton(canvasGO.transform, "StartWeekButton", "START WEEK", new Vector2(0, 100), new Vector2(450, 75), btnSprite);
        CreateButton(canvasGO.transform, "InstructionsButton", "INSTRUCTIONS", new Vector2(0, 0), new Vector2(450, 75), btnSprite);
        CreateButton(canvasGO.transform, "SettingsButton", "SETTINGS", new Vector2(0, -100), new Vector2(450, 75), btnSprite);
        CreateButton(canvasGO.transform, "ExitButton", "EXIT", new Vector2(0, -200), new Vector2(450, 75), btnSprite);
    }

    static void CreateButton(Transform parent, string name, string text, Vector2 pos, Vector2 size, Sprite sprite)
    {
        GameObject btnGO = new GameObject(name);
        btnGO.transform.SetParent(parent, false);
        Image btnImg = btnGO.AddComponent<Image>();
        btnImg.sprite = sprite;
        btnImg.type = Image.Type.Sliced;
        Button btn = btnGO.AddComponent<Button>();
        
        RectTransform btnRect = btnGO.GetComponent<RectTransform>();
        btnRect.sizeDelta = size;
        btnRect.anchoredPosition = pos;

        GameObject textGO = new GameObject("Text");
        textGO.transform.SetParent(btnGO.transform, false);
        TextMeshProUGUI tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = 36;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        
        Shadow shadow = textGO.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.5f);
        shadow.effectDistance = new Vector2(2, -2);

        RectTransform textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;
    }
}