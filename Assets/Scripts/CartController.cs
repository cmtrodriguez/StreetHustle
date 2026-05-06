using UnityEngine;

public class CartController : MonoBehaviour
{
    [Header("Input (Arrow Keys)")]
    public KeyCode forwardKey = KeyCode.UpArrow;
    public KeyCode backKey = KeyCode.DownArrow;
    public KeyCode turnRightKey = KeyCode.RightArrow;
    public KeyCode turnLeftKey = KeyCode.LeftArrow;

    [Header("Movement")]
    public float moveSpeed = 2.0f;

    [Header("Turning")]
    public float turnSpeedDegrees = 120f;

    [Header("Wheels")]
    public float wheelRadius = 0.5f;
    public Transform[] wheels;

    [Header("Hands (optional)")]
    public Transform rightHand;
    public Transform leftHand;

    Rigidbody _rb;

    Vector3 _rightHandStartPos;
    Vector3 _leftHandStartPos;
    float _handAnimTime;

    float _moveInput; // -1..1
    float _turnInput; // -1..1

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        if (rightHand != null) _rightHandStartPos = rightHand.localPosition;
        if (leftHand != null) _leftHandStartPos = leftHand.localPosition;
    }

    void Update()
    {
        _moveInput = (Input.GetKey(forwardKey) ? 1f : 0f) + (Input.GetKey(backKey) ? -1f : 0f);
        _turnInput = (Input.GetKey(turnRightKey) ? 1f : 0f) + (Input.GetKey(turnLeftKey) ? -1f : 0f);

        // Clamp so holding both keys cancels out.
        _moveInput = Mathf.Clamp(_moveInput, -1f, 1f);
        _turnInput = Mathf.Clamp(_turnInput, -1f, 1f);

        // Visual animation runs in Update.
        if (Mathf.Abs(_moveInput) > 0.0001f)
        {
            RotateWheels(moveSpeed * Mathf.Sign(_moveInput));
            AnimateHands(true);
        }
        else
        {
            AnimateHands(false);
            _handAnimTime = 0f;
        }
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        // Rotation
        float turnDegrees = _turnInput * turnSpeedDegrees * dt;
        if (Mathf.Abs(turnDegrees) > 0.0001f)
        {
            var deltaRot = Quaternion.Euler(0f, turnDegrees, 0f);

            if (_rb != null && !_rb.isKinematic)
                _rb.MoveRotation(_rb.rotation * deltaRot);
            else
                transform.rotation = transform.rotation * deltaRot;
        }

        // Translation
        float move = _moveInput * moveSpeed;
        if (_rb != null && !_rb.isKinematic)
        {
            // Use velocity so it keeps moving smoothly while key is held.
            if (Mathf.Abs(move) > 0.0001f)
                _rb.velocity = transform.forward * move;
            else
                _rb.velocity = Vector3.zero;

            // Prevent spin drift.
            _rb.angularVelocity = Vector3.zero;
        }
        else
        {
            if (Mathf.Abs(move) > 0.0001f)
                transform.position += transform.forward * (move * dt);
        }
    }

    void RotateWheels(float signedSpeed)
    {
        if (wheels == null || wheels.Length == 0) return;

        float circumference = 2f * Mathf.PI * wheelRadius;
        float degreesPerSecond = (circumference <= 0.0001f) ? 0f : (Mathf.Abs(signedSpeed) / circumference) * 360f;
        float direction = Mathf.Sign(signedSpeed);

        foreach (Transform wheel in wheels)
        {
            if (wheel == null) continue;
            wheel.Rotate(Vector3.right * (degreesPerSecond * direction) * Time.deltaTime, Space.Self);
        }
    }

    void AnimateHands(bool moving)
    {
        if (rightHand == null && leftHand == null) return;

        if (moving)
        {
            _handAnimTime += Time.deltaTime * 5f;
            float pushOffset = Mathf.Sin(_handAnimTime) * 0.05f;

            if (rightHand != null)
                rightHand.localPosition = _rightHandStartPos + new Vector3(0, 0, pushOffset);
            if (leftHand != null)
                leftHand.localPosition = _leftHandStartPos + new Vector3(0, 0, pushOffset);
        }
        else
        {
            if (rightHand != null)
                rightHand.localPosition = Vector3.Lerp(rightHand.localPosition, _rightHandStartPos, Time.deltaTime * 5f);
            if (leftHand != null)
                leftHand.localPosition = Vector3.Lerp(leftHand.localPosition, _leftHandStartPos, Time.deltaTime * 5f);
        }
    }
}
