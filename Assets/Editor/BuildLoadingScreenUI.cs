using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine.Events;

public class BuildLoadingScreenUI
{
    [MenuItem("Tools/Build Loading Screen")]
    public static void Run()
    {
        // Find existing LoadingScreenCanvas or create it
        GameObject canvasObj = GameObject.Find("LoadingScreenCanvas");
        if (canvasObj != null) GameObject.DestroyImmediate(canvasObj);
        
        canvasObj = new GameObject("LoadingScreenCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10;
        
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        canvasObj.AddComponent<GraphicRaycaster>();
        
        CanvasGroup canvasGroup = canvasObj.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        
        LoadingScreen loadingScreen = canvasObj.AddComponent<LoadingScreen>();
        
        // Background (Fullscreen Image with EnvelopeParent to cover screen without stretching)
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(canvasObj.transform, false);
        Image bgImg = bgObj.AddComponent<Image>();
        Sprite bgSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/BackgroundClean.png");
        bgImg.sprite = bgSprite;
        bgImg.type = Image.Type.Simple;
        
        RectTransform bgRT = bgObj.GetComponent<RectTransform>();
        bgRT.anchorMin = new Vector2(0.5f, 0.5f);
        bgRT.anchorMax = new Vector2(0.5f, 0.5f);
        bgRT.pivot = new Vector2(0.5f, 0.5f);
        
        AspectRatioFitter aspectFitter = bgObj.AddComponent<AspectRatioFitter>();
        aspectFitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
        if (bgSprite != null) {
            aspectFitter.aspectRatio = bgSprite.rect.width / bgSprite.rect.height;
        }
        
        // Glass Container (Translucent Panel over 3D scene/background)
        GameObject containerObj = new GameObject("GlassContainer");
        containerObj.transform.SetParent(bgObj.transform, false);
        Image containerImg = containerObj.AddComponent<Image>();
        containerImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_UI_COPLAY_GENERATED/LoadingScreen/Sprites/LoadingContainer.png");
        containerImg.type = Image.Type.Simple;
        containerImg.preserveAspect = true;
        containerImg.color = new Color(1f, 1f, 1f, 1f); // Full white to show native image colors
        RectTransform containerRT = containerObj.GetComponent<RectTransform>();
        containerRT.anchorMin = new Vector2(0.5f, 0.5f);
        containerRT.anchorMax = new Vector2(0.5f, 0.5f);
        containerRT.sizeDelta = new Vector2(1300, 800);
        containerRT.anchoredPosition = new Vector2(0, 0);
        
        // Logo Inside Container
        GameObject logoObj = new GameObject("Logo");
        logoObj.transform.SetParent(containerObj.transform, false);
        Image logoImg = logoObj.AddComponent<Image>();
        logoImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_UI_COPLAY_GENERATED/MainMenuV2/Sprites/Logo.png");
        logoImg.preserveAspect = true;
        RectTransform logoRT = logoObj.GetComponent<RectTransform>();
        logoRT.anchorMin = new Vector2(0.5f, 1f);
        logoRT.anchorMax = new Vector2(0.5f, 1f);
        logoRT.pivot = new Vector2(0.5f, 1f);
        logoRT.anchoredPosition = new Vector2(0, -50); 
        logoRT.sizeDelta = new Vector2(500, 250);
        
        // Title Text: "HEATING UP THE GRILL..."
        GameObject titleObj = new GameObject("TitleText");
        titleObj.transform.SetParent(containerObj.transform, false);
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "HEATING UP THE GRILL...";
        titleText.fontSize = 50; // Bigger font
        titleText.fontStyle = FontStyles.Bold;
        titleText.alignment = TextAlignmentOptions.Center;
        // Warm yellowish/orange color
        titleText.color = new Color(0.95f, 0.85f, 0.3f);
        
        // Setup embossed 3D look with Underlay on a new material instance
        if (titleText.fontSharedMaterial != null) {
            Material fontMat = new Material(titleText.fontSharedMaterial);
            fontMat.EnableKeyword("UNDERLAY_ON");
            fontMat.SetFloat("_UnderlayOffsetX", 0.6f);
            fontMat.SetFloat("_UnderlayOffsetY", -0.6f);
            fontMat.SetFloat("_UnderlayDilate", 0.3f);
            fontMat.SetColor("_UnderlayColor", new Color(0, 0, 0, 0.7f));
            titleText.fontMaterial = fontMat;
        }

        RectTransform titleRT = titleObj.GetComponent<RectTransform>();
        titleRT.anchorMin = new Vector2(0.5f, 0.5f);
        titleRT.anchorMax = new Vector2(0.5f, 0.5f);
        titleRT.anchoredPosition = new Vector2(0, -30); // Below logo
        titleRT.sizeDelta = new Vector2(800, 70);
        
        // Progress Bar
        GameObject sliderObj = new GameObject("ProgressBar");
        sliderObj.transform.SetParent(containerObj.transform, false);
        Slider slider = sliderObj.AddComponent<Slider>();
        slider.interactable = false;
        slider.transition = Selectable.Transition.None;
        RectTransform sliderRT = sliderObj.GetComponent<RectTransform>();
        sliderRT.anchorMin = new Vector2(0.5f, 0.5f);
        sliderRT.anchorMax = new Vector2(0.5f, 0.5f);
        sliderRT.anchoredPosition = new Vector2(0, -110);
        sliderRT.sizeDelta = new Vector2(750, 40); 
        
        GameObject sliderBgObj = new GameObject("Background");
        sliderBgObj.transform.SetParent(sliderObj.transform, false);
        Image sliderBgImg = sliderBgObj.AddComponent<Image>();
        sliderBgImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_UI_COPLAY_GENERATED/LoadingScreen/Sprites/ProgressBarBG.png");
        sliderBgImg.type = Image.Type.Sliced;
        RectTransform sbgRT = sliderBgObj.GetComponent<RectTransform>();
        sbgRT.anchorMin = Vector2.zero; sbgRT.anchorMax = Vector2.one;
        sbgRT.offsetMin = Vector2.zero; sbgRT.offsetMax = Vector2.zero;
        
        GameObject fillAreaObj = new GameObject("Fill Area");
        fillAreaObj.transform.SetParent(sliderObj.transform, false);
        RectTransform faRT = fillAreaObj.AddComponent<RectTransform>();
        faRT.anchorMin = Vector2.zero; faRT.anchorMax = Vector2.one;
        faRT.offsetMin = new Vector2(5, 5); faRT.offsetMax = new Vector2(-5, -5); 
        
        GameObject fillObj = new GameObject("Fill");
        fillObj.transform.SetParent(fillAreaObj.transform, false);
        Image fillImg = fillObj.AddComponent<Image>();
        fillImg.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_UI_COPLAY_GENERATED/LoadingScreen/Sprites/ProgressBarFill.png");
        fillImg.type = Image.Type.Sliced;
        fillImg.color = new Color(1.0f, 0.6f, 0.1f); 
        RectTransform fRT = fillObj.GetComponent<RectTransform>();
        fRT.anchorMin = Vector2.zero; fRT.anchorMax = Vector2.one;
        fRT.offsetMin = Vector2.zero; fRT.offsetMax = Vector2.zero;
        
        slider.fillRect = fRT;
        slider.value = 0.5f; // Initial preview value
        
        // Detail Text
        GameObject detailObj = new GameObject("DetailText");
        detailObj.transform.SetParent(containerObj.transform, false);
        TextMeshProUGUI detailText = detailObj.AddComponent<TextMeshProUGUI>();
        detailText.text = "Loading 50%... Day 1 - Monday";
        detailText.fontSize = 20;
        detailText.alignment = TextAlignmentOptions.Center;
        detailText.color = new Color(0.9f, 0.9f, 0.9f);
        RectTransform detailRT = detailObj.GetComponent<RectTransform>();
        detailRT.anchorMin = new Vector2(0.5f, 0.5f);
        detailRT.anchorMax = new Vector2(0.5f, 0.5f);
        detailRT.anchoredPosition = new Vector2(0, -160);
        detailRT.sizeDelta = new Vector2(700, 30);

        // Tip Container
        GameObject tipContainerObj = new GameObject("TipContainer");
        // Attach to full screen background, so it spans the bottom of the screen
        tipContainerObj.transform.SetParent(bgObj.transform, false); 
        Image tipContainerImg = tipContainerObj.AddComponent<Image>();
        tipContainerImg.color = new Color(0f, 0f, 0f, 0.75f); // Dark translucent highlight
        RectTransform tipContainerRT = tipContainerObj.GetComponent<RectTransform>();
        tipContainerRT.anchorMin = new Vector2(0f, 0f);
        tipContainerRT.anchorMax = new Vector2(1f, 0f);
        tipContainerRT.pivot = new Vector2(0.5f, 0f);
        tipContainerRT.anchoredPosition = new Vector2(0, 30); // 30px from bottom
        tipContainerRT.sizeDelta = new Vector2(0, 45); // height 45, stretches width

        // Tip Text
        GameObject tipObj = new GameObject("TipText");
        tipObj.transform.SetParent(tipContainerObj.transform, false);
        TextMeshProUGUI tipText = tipObj.AddComponent<TextMeshProUGUI>();
        tipText.text = "TIP: Strategize and fit the food demand to the selected weather for bigger profit!";
        tipText.fontSize = 22;
        tipText.alignment = TextAlignmentOptions.Center;
        tipText.color = new Color(0.95f, 0.95f, 0.95f);
        RectTransform tipRT = tipObj.GetComponent<RectTransform>();
        tipRT.anchorMin = Vector2.zero; tipRT.anchorMax = Vector2.one;
        tipRT.offsetMin = new Vector2(20, 0); tipRT.offsetMax = new Vector2(-20, 0);
        
        // Setup LoadingScreen.cs
        loadingScreen.loadingPanel = bgObj; 
        loadingScreen.canvasGroup = canvasGroup;
        loadingScreen.progressBar = slider;
        loadingScreen.progressText = detailText; 
        loadingScreen.titleText = titleText;
        loadingScreen.tipText = tipText;
        
        // Hook up MainMenu.cs
        GameObject mainMenuCanvas = GameObject.Find("MainMenuCanvasV2");
        if (mainMenuCanvas != null)
        {
            MainMenu mainMenu = mainMenuCanvas.GetComponent<MainMenu>();
            if (mainMenu == null) mainMenu = mainMenuCanvas.AddComponent<MainMenu>();
            
            mainMenu.gameSceneName = "SampleScene"; 
            
            Transform startBtnT = mainMenuCanvas.transform.Find("StartWeekButton");
            if (startBtnT != null)
            {
                Button startBtn = startBtnT.GetComponent<Button>();
                if (startBtn != null)
                {
                    while(startBtn.onClick.GetPersistentEventCount() > 0) {
                        UnityEventTools.RemovePersistentListener(startBtn.onClick, 0);
                    }
                    UnityAction action = new UnityAction(mainMenu.OnStartWeekClicked);
                    UnityEventTools.AddPersistentListener(startBtn.onClick, action);
                }
            }
        }
        
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        Debug.Log("Loading Screen UI Generated successfully matching the mockup.");
    }
}
