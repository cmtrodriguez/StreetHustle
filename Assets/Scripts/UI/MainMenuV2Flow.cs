using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Minimal main menu flow for SampleScene:
/// - Ensures an EventSystem exists
/// - Wires up the MainMenuCanvasV2 buttons
/// - StartWeek: hides menu + enables cart POV camera + enables cart movement
/// </summary>
public class MainMenuV2Flow : MonoBehaviour
{
    [Header("UI")]
    public Canvas menuCanvas;
    public Button startWeekButton;

    [Header("World")]
    public CartController cartController;
    public CartPOVCamera povCamera;

    [Header("Menu State")]
    public bool disablePOVCameraWhileInMenu = true;

    [Header("Debug")]
    public bool logClicks;

    void Reset()
    {
        menuCanvas = GetComponent<Canvas>();
    }

    void Awake()
    {
        EnsureEventSystem();

        // Resolve common references automatically if they weren't assigned.
        if (menuCanvas == null) menuCanvas = GetComponent<Canvas>();
        if (startWeekButton == null)
        {
            var go = GameObject.Find("MainMenuCanvasV2/StartWeekButton");
            if (go != null) startWeekButton = go.GetComponent<Button>();
        }

        if (cartController == null)
        {
            var go = GameObject.Find("New_Wooden_Cart");
            if (go != null) cartController = go.GetComponent<CartController>();
        }

        if (povCamera == null)
        {
            var cam = Camera.main;
            if (cam != null) povCamera = cam.GetComponent<CartPOVCamera>();
        }

        WireButtons();
        EnterMenuState();
    }

    void WireButtons()
    {
        if (startWeekButton == null) return;

        // Prevent duplicate listeners across domain reloads.
        startWeekButton.onClick.RemoveListener(OnStartWeek);
        startWeekButton.onClick.AddListener(OnStartWeek);
    }

    void EnterMenuState()
    {
        // Menu visible.
        if (menuCanvas != null)
            menuCanvas.gameObject.SetActive(true);

        // Prevent cart from moving during menu.
        if (cartController != null)
            cartController.enabled = false;

        // Keep POV camera off if requested, so you stay on the menu view.
        if (povCamera != null && disablePOVCameraWhileInMenu)
            povCamera.enabled = false;
    }

    void OnStartWeek()
    {
        if (logClicks) Debug.Log("StartWeek clicked");

        // If we have a transition overlay, play it and switch to gameplay at the midpoint.
        var transition = FindObjectOfType<TransitionVideoOverlay>(true);
        if (transition != null && transition.clip != null)
        {
            transition.Play(
                onMidpoint: () =>
                {
                    // Hide menu entirely (disables all raycasters too).
                    if (menuCanvas != null)
                        menuCanvas.gameObject.SetActive(false);

                    // Switch camera to cart POV.
                    if (povCamera != null)
                    {
                        povCamera.enabled = true;
                        povCamera.SendMessage("SnapNow", SendMessageOptions.DontRequireReceiver);
                    }

                    // Enable cart movement.
                    if (cartController != null)
                        cartController.enabled = true;

                    // Clear UI selection so keyboard input goes to gameplay.
                    if (EventSystem.current != null)
                        EventSystem.current.SetSelectedGameObject(null);
                },
                onComplete: null);

            return;
        }

        // Fallback: no transition
        if (menuCanvas != null)
            menuCanvas.gameObject.SetActive(false);

        if (povCamera != null)
        {
            povCamera.enabled = true;
            povCamera.SendMessage("SnapNow", SendMessageOptions.DontRequireReceiver);
        }

        if (cartController != null)
            cartController.enabled = true;

        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);
    }

    static void EnsureEventSystem()
    {
        if (EventSystem.current != null)
            return;

        var es = new GameObject("EventSystem");
        es.AddComponent<EventSystem>();
        es.AddComponent<StandaloneInputModule>();
    }
}
