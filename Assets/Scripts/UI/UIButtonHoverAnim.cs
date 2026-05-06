using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Simple hover/press animation for Unity UI elements.
/// Attach to the button root (the object with the Image/Button).
/// </summary>
[DisallowMultipleComponent]
public class UIButtonHoverAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Scale")]
    public float hoverScale = 1.06f;
    public float pressedScale = 0.98f;

    [Header("Motion")]
    public float hoverYOffset = 4f; // UI units (pixels)

    [Header("Smoothing")]
    public float lerpSpeed = 18f;

    [Header("Target")]
    public RectTransform targetRect;

    RectTransform _rt;
    Vector3 _baseScale;
    Vector2 _baseAnchoredPos;

    bool _hovered;
    bool _pressed;

    void Awake()
    {
        _rt = targetRect != null ? targetRect : GetComponent<RectTransform>();
        if (_rt != null)
        {
            _baseScale = _rt.localScale;
            _baseAnchoredPos = _rt.anchoredPosition;
        }
    }

    void OnEnable()
    {
        // Re-cache in case layout changed.
        if (targetRect != null) _rt = targetRect;
        else if (_rt == null) _rt = GetComponent<RectTransform>();
        
        if (_rt != null)
        {
            _baseScale = _rt.localScale;
            _baseAnchoredPos = _rt.anchoredPosition;
        }

        _hovered = false;
        _pressed = false;
    }

    void Update()
    {
        if (_rt == null) return;

        float targetScaleMul = _pressed ? pressedScale : (_hovered ? hoverScale : 1f);
        Vector3 targetScale = _baseScale * targetScaleMul;

        float t = 1f - Mathf.Exp(-lerpSpeed * Time.unscaledDeltaTime);
        _rt.localScale = Vector3.Lerp(_rt.localScale, targetScale, t);

        if (animatePosition)
        {
            float targetYOffset = (_hovered && !_pressed) ? hoverYOffset : 0f;
            Vector2 targetPos = _baseAnchoredPos + new Vector2(0f, targetYOffset);
            _rt.anchoredPosition = Vector2.Lerp(_rt.anchoredPosition, targetPos, t);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) => _hovered = true;
    public void OnPointerExit(PointerEventData eventData)
    {
        _hovered = false;
        _pressed = false;
    }

    public void OnPointerDown(PointerEventData eventData) => _pressed = true;
    public void OnPointerUp(PointerEventData eventData) => _pressed = false;



    [Header("Settings")]
    public bool animatePosition = true;
}
