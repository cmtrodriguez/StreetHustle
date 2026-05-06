using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Adds a visual tilt to a cart's body whenever it moves or turns, similar to a
/// real two-wheeled cart that pitches forward/back on its axle and banks when
/// turning. Wheels themselves are NOT tilted so they still roll correctly.
///
/// At Start(), this component automatically creates a child transform called
/// "TiltBody" and reparents every non-wheel child of the cart under it. The
/// TiltBody is then rotated every frame based on the CartController's input
/// (or the root's velocity as a fallback). All objects that ride on the cart
/// (ice cream cans, cones, props, etc.) tilt together as a single rigid body.
/// </summary>
[DisallowMultipleComponent]
public class CartTiltEffect : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Optional: CartController on the same GameObject. If present, its input drives the tilt. Otherwise velocity is used.")]
    public CartController cartController;

    [Tooltip("Transforms that should NOT be reparented under the tilt body (typically wheels so they keep their rolling pivot).")]
    public List<Transform> excludedFromTilt = new List<Transform>();

    [Header("Tilt Amounts (degrees)")]
    [Tooltip("How many degrees the body pitches forward (nose-down) when moving. Applied the same way for forward AND backward movement.")]
    public float pitchWhenMoving = 6f;

    [Tooltip("How many degrees the body pitches forward when turning in place (no translation).")]
    public float pitchWhenTurning = 4f;

    [Tooltip("How many degrees the body rocks side-to-side while turning. Positive = rocks toward the turn direction.")]
    public float turnRockAmplitude = 3f;

    [Tooltip("How fast the side-to-side rock oscillates while turning.")]
    public float turnRockSpeed = 4f;

    [Header("Response")]
    [Tooltip("How quickly the tilt reacts to changes in input. Higher = snappier.")]
    public float tiltResponse = 6f;

    [Tooltip("Extra idle sway/bobbing amplitude in degrees while moving (simulates uneven ground).")]
    public float idleSwayAmplitude = 1.5f;

    [Tooltip("Speed of the idle sway oscillation.")]
    public float idleSwaySpeed = 4f;

    [Header("Pivot")]
    [Tooltip("Local offset (relative to cart root) for the tilt pivot. Y should roughly match the wheel axle height so the tilt pivots around the axle like a real cart.")]
    public Vector3 pivotLocalOffset = Vector3.zero;

    Transform _tiltBody;
    float _currentPitch;
    float _currentRoll;
    float _swayTime;
    float _turnRockTime;

    Vector3 _lastPosition;
    float _fallbackMoveInput;
    float _fallbackTurnInput;

    void Awake()
    {
        if (cartController == null)
            cartController = GetComponent<CartController>();

        // Auto-exclude the wheels declared on the CartController.
        if (cartController != null && cartController.wheels != null)
        {
            foreach (var w in cartController.wheels)
            {
                if (w != null && !excludedFromTilt.Contains(w))
                    excludedFromTilt.Add(w);
            }
        }
    }

    void Start()
    {
        BuildTiltBody();
        _lastPosition = transform.position;
    }

    void BuildTiltBody()
    {
        // Create a dedicated child that will carry all the non-wheel body parts.
        var go = new GameObject("TiltBody");
        _tiltBody = go.transform;
        _tiltBody.SetParent(transform, false);
        _tiltBody.localPosition = pivotLocalOffset;
        _tiltBody.localRotation = Quaternion.identity;
        _tiltBody.localScale = Vector3.one;

        // Collect current top-level children (excluding wheels and ourselves).
        var toReparent = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child == _tiltBody) continue;
            if (IsExcluded(child)) continue;
            toReparent.Add(child);
        }

        // Reparent while preserving world position/rotation/scale.
        foreach (var child in toReparent)
        {
            // Offset by pivot so the world pose stays exactly the same.
            child.SetParent(_tiltBody, true);
        }
    }

    bool IsExcluded(Transform t)
    {
        if (t == null) return true;
        foreach (var ex in excludedFromTilt)
        {
            if (ex == null) continue;
            if (t == ex || t.IsChildOf(ex)) return true;
        }
        return false;
    }

    void LateUpdate()
    {
        if (_tiltBody == null) return;

        float moveInput;
        float turnInput;
        ReadInputs(out moveInput, out turnInput);

        // Target angles. Forward motion tips the nose down. We use |moveInput| so
        // BACKWARD movement tilts the same direction as forward movement (both
        // lean the cart toward its front end rather than flipping back).
        float absMove = Mathf.Abs(moveInput);
        float absTurn = Mathf.Abs(turnInput);

        // The turn contribution also pitches the body forward (same axis as
        // forward movement) so it looks like the pusher is leaning into the
        // turn, rather than banking sideways.
        float targetPitch = absMove * pitchWhenMoving + absTurn * pitchWhenTurning;

        // We zero out roll and instead add a controlled side-to-side oscillation
        // while turning. This gives a "rocking" feel instead of an uncontrolled
        // continuous lean.
        float targetRoll = 0f;

        // Smoothly approach the target tilt.
        float t = 1f - Mathf.Exp(-tiltResponse * Time.deltaTime);
        _currentPitch = Mathf.Lerp(_currentPitch, targetPitch, t);
        _currentRoll = Mathf.Lerp(_currentRoll, targetRoll, t);

        // Add a subtle forward/back sway while moving (uneven ground feel).
        float sway = 0f;
        if (absMove > 0.01f && idleSwayAmplitude > 0f)
        {
            _swayTime += Time.deltaTime * idleSwaySpeed;
            sway = Mathf.Sin(_swayTime) * idleSwayAmplitude;
        }
        else
        {
            _swayTime = 0f;
        }

        // Controlled side-to-side rock while turning. Direction follows the
        // turn input so it rocks toward the way you're turning, but it's a
        // bounded sine wave instead of a continuous lean.
        float turnRock = 0f;
        if (absTurn > 0.01f && turnRockAmplitude > 0f)
        {
            _turnRockTime += Time.deltaTime * turnRockSpeed;
            turnRock = Mathf.Sin(_turnRockTime) * turnRockAmplitude * Mathf.Sign(turnInput);
        }
        else
        {
            _turnRockTime = 0f;
        }

        _tiltBody.localRotation = Quaternion.Euler(
            _currentPitch + sway * 0.5f,
            0f,
            _currentRoll + turnRock);
    }

    void ReadInputs(out float moveInput, out float turnInput)
    {
        if (cartController != null)
        {
            // Mirror the exact same logic the controller uses so the visuals
            // follow whatever keys it's configured to listen for.
            float m = (Input.GetKey(cartController.forwardKey) ? 1f : 0f)
                    + (Input.GetKey(cartController.backKey) ? -1f : 0f);
            float tr = (Input.GetKey(cartController.turnRightKey) ? 1f : 0f)
                     + (Input.GetKey(cartController.turnLeftKey) ? -1f : 0f);
            moveInput = Mathf.Clamp(m, -1f, 1f);
            turnInput = Mathf.Clamp(tr, -1f, 1f);
            return;
        }

        // Fallback: derive from actual movement so this still works without a controller.
        Vector3 delta = transform.position - _lastPosition;
        _lastPosition = transform.position;

        float forwardSpeed = Vector3.Dot(delta / Mathf.Max(Time.deltaTime, 0.0001f), transform.forward);
        _fallbackMoveInput = Mathf.Lerp(_fallbackMoveInput, Mathf.Clamp(forwardSpeed, -1f, 1f), 0.2f);

        // Turn input fallback: not available without a controller, leave as 0.
        _fallbackTurnInput = 0f;

        moveInput = _fallbackMoveInput;
        turnInput = _fallbackTurnInput;
    }
}
