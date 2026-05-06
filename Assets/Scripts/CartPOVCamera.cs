using UnityEngine;

/// <summary>
/// Simple "pushing the cart" POV camera.
/// Places the camera behind/above the cart and looks forward (or at a point ahead).
/// </summary>
[DisallowMultipleComponent]
[ExecuteAlways]
public class CartPOVCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Rig")]
    [Tooltip("Local-space offset relative to the cart.")]
    public Vector3 localOffset = new Vector3(0f, 1.25f, -1.35f);

    [Tooltip("Local-space look target relative to the cart.")]
    public Vector3 localLookAt = new Vector3(0f, 1.0f, 3.5f);

    [Header("Smoothing")]
    [Tooltip("Higher = snappier. Set to 0 to disable smoothing.")]
    public float positionLerpSpeed = 12f;

    [Tooltip("Higher = snappier. Set to 0 to disable smoothing.")]
    public float rotationLerpSpeed = 16f;

    [Header("Camera")]
    public float fov = 60f;

    [Header("Startup")]
    [Tooltip("If true, the camera snaps to the POV immediately on enable/play.")]
    public bool snapOnEnable = true;

    [Tooltip("If true, also snap the Main Camera in Edit Mode (not just during Play Mode).")]
    public bool snapInEditMode = true;

    [Tooltip("If target is missing, try to find this object by name at runtime.")]
    public string fallbackTargetName = "New_Wooden_Cart";

    Camera _cam;

    void Awake()
    {
        _cam = GetComponent<Camera>();
        ApplyFov();
    }

    void OnEnable()
    {
        // In case play starts with a missing reference, attempt to find it.
        EnsureTarget();

        if (snapOnEnable && (Application.isPlaying || snapInEditMode))
            SnapNow();
    }

    void LateUpdate()
    {
        EnsureTarget();
        if (target == null) return;

        Vector3 desiredPos = target.TransformPoint(localOffset);
        Vector3 desiredLookPoint = target.TransformPoint(localLookAt);
        Quaternion desiredRot = Quaternion.LookRotation((desiredLookPoint - desiredPos).normalized, Vector3.up);

        if (positionLerpSpeed <= 0f)
            transform.position = desiredPos;
        else
            transform.position = Vector3.Lerp(transform.position, desiredPos, 1f - Mathf.Exp(-positionLerpSpeed * Time.deltaTime));

        if (rotationLerpSpeed <= 0f)
            transform.rotation = desiredRot;
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, 1f - Mathf.Exp(-rotationLerpSpeed * Time.deltaTime));

        ApplyFov();
    }

    void EnsureTarget()
    {
        if (target != null) return;
        if (string.IsNullOrWhiteSpace(fallbackTargetName)) return;

        var go = GameObject.Find(fallbackTargetName);
        if (go != null) target = go.transform;
    }

    [ContextMenu("Snap Now")]
    void SnapNow()
    {
        EnsureTarget();
        if (target == null) return;

        Vector3 desiredPos = target.TransformPoint(localOffset);
        Vector3 desiredLookPoint = target.TransformPoint(localLookAt);
        Quaternion desiredRot = Quaternion.LookRotation((desiredLookPoint - desiredPos).normalized, Vector3.up);

        transform.position = desiredPos;
        transform.rotation = desiredRot;

        ApplyFov();
    }

    void ApplyFov()
    {
        if (_cam == null) return;
        if (Mathf.Abs(_cam.fieldOfView - fov) > 0.01f)
            _cam.fieldOfView = fov;
    }
}
