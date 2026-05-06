using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[DisallowMultipleComponent]
public class TransitionVideoOverlay : MonoBehaviour
{
    [Header("Video")]
    public VideoClip clip;

    [Range(0f, 1f)]
    public float midpointNormalized = 0.5f;

    [Header("UI")]
    public Canvas overlayCanvas;
    public RawImage rawImage;
    public CanvasGroup canvasGroup;

    [Header("Optional Effects")]
    public Material chromaKeyMaterial;

    [Header("RenderTexture")]
    public Vector2Int renderTextureSize = new Vector2Int(1280, 720);

    private VideoPlayer _vp;
    private RenderTexture _rt;
    private Coroutine _routine;
    private bool _prepared;

    void Awake()
    {
        Debug.Log("[Transition] Awake");

        _vp = GetComponent<VideoPlayer>();
        if (_vp == null)
            _vp = gameObject.AddComponent<VideoPlayer>();

        // Configure VideoPlayer
        _vp.playOnAwake = false;
        _vp.isLooping = false;
        _vp.renderMode = VideoRenderMode.RenderTexture;
        _vp.audioOutputMode = VideoAudioOutputMode.None;

        _vp.waitForFirstFrame = true;
        _vp.skipOnDrop = true;

        _vp.prepareCompleted -= OnPrepared;
        _vp.prepareCompleted += OnPrepared;

        EnsureUIWiring();
        EnsureRenderTexture();

        HideImmediate();
        PrepareClip();
    }

    void OnDestroy()
    {
        if (_rt != null)
        {
            _rt.Release();
            Destroy(_rt);
        }
    }

    void EnsureUIWiring()
    {
        if (overlayCanvas == null)
            overlayCanvas = GetComponentInChildren<Canvas>(true);

        if (rawImage == null)
            rawImage = GetComponentInChildren<RawImage>(true);

        if (canvasGroup == null)
            canvasGroup = GetComponentInChildren<CanvasGroup>(true);

        if (rawImage != null && chromaKeyMaterial != null)
            rawImage.material = chromaKeyMaterial;
    }

    void EnsureRenderTexture()
    {
        if (_rt == null)
        {
            _rt = new RenderTexture(
                Mathf.Max(16, renderTextureSize.x),
                Mathf.Max(16, renderTextureSize.y),
                0,
                RenderTextureFormat.ARGB32
            );

            _rt.name = "TransitionVideoRT";
            _rt.Create();
        }

        _vp.targetTexture = _rt;

        if (rawImage != null)
            rawImage.texture = _rt;
    }

    public void Play(Action onMidpoint = null, Action onComplete = null)
    {
        Debug.Log("[Transition] Play called");

        if (clip == null)
        {
            Debug.LogError("[Transition] No VideoClip assigned!");
            return;
        }

        if (_routine != null)
            StopCoroutine(_routine);

        _routine = StartCoroutine(PlayRoutine(onMidpoint, onComplete));
    }

    void OnPrepared(VideoPlayer vp)
    {
        Debug.Log("[Transition] Video Prepared");
        _prepared = true;
    }

    void PrepareClip()
    {
        if (clip == null) return;

        _vp.clip = clip;
        _vp.time = 0;
        _prepared = false;

        Debug.Log("[Transition] Preparing video...");
        _vp.Prepare();
    }

    IEnumerator PlayRoutine(Action onMidpoint, Action onComplete)
    {
        Debug.Log("[Transition] Starting routine");

        ShowImmediate();

        if (_vp.clip != clip)
            PrepareClip();

        while (!_prepared && !_vp.isPrepared)
        {
            Debug.Log("[Transition] Waiting for prepare...");
            yield return null;
        }

        Debug.Log("[Transition] Playing video");
        _vp.time = 0;
        _vp.Play();

        bool midFired = false;
        double length = _vp.length;

        if (length <= 0.01)
            length = clip.length;

        if (length <= 0.01)
            length = 1.0;

        while (_vp.isPlaying)
        {
            double normalized = _vp.time / length;

            if (!midFired && normalized >= midpointNormalized)
            {
                midFired = true;
                Debug.Log("[Transition] Midpoint reached");
                onMidpoint?.Invoke();
            }

            yield return null;
        }

        if (!midFired)
        {
            Debug.Log("[Transition] Midpoint forced");
            onMidpoint?.Invoke();
        }

        Debug.Log("[Transition] Video finished");

        HideImmediate();
        onComplete?.Invoke();

        _routine = null;
    }

    void ShowImmediate()
    {
        Debug.Log("[Transition] Show");

        if (overlayCanvas != null)
            overlayCanvas.enabled = true;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        if (rawImage != null)
            rawImage.enabled = true;
    }

    void HideImmediate()
    {
        Debug.Log("[Transition] Hide");

        if (rawImage != null)
            rawImage.enabled = false;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        if (overlayCanvas != null)
            overlayCanvas.enabled = false;
    }
}