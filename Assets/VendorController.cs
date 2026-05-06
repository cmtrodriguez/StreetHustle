using UnityEngine;
using UnityEngine.UI; // For the Stamina Bar later

public class VendorController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float baseSpeed = 5f;
    public float turnSpeed = 150f;
    
    [Header("Stamina System")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 10f; // Drains while pushing
    public float recoveryRate = 15f;    // Recovers while resting
    
    [Header("Cart State")]
    public bool isPushing = false;
    public bool isRingingBell = false;

    void Start() {
        currentStamina = maxStamina;
    }

    void Update() {
        HandleMovement();
        HandleBell();
    }

    void HandleMovement() {
        float moveInput = Input.GetAxis("Vertical"); // W/S
        float turnInput = Input.GetAxis("Horizontal"); // A/D

        // You can only push if you have stamina
        if (Mathf.Abs(moveInput) > 0 && currentStamina > 0) {
            isPushing = true;
            
            // Move and Rotate
            transform.Translate(Vector3.forward * moveInput * baseSpeed * Time.deltaTime);
            transform.Rotate(Vector3.up * turnInput * turnSpeed * Time.deltaTime);

            // Drain stamina
            currentStamina -= staminaDrainRate * Time.deltaTime;
        } else {
            isPushing = false;
            // Recover stamina when standing still
            if (currentStamina < maxStamina) {
                currentStamina += recoveryRate * Time.deltaTime;
            }
        }

        // Penalty: Slow down if "Pagod" (Exhausted)
        if (currentStamina < 20) {
            baseSpeed = 2.5f; 
        } else {
            baseSpeed = 5f;
        }
    }

    void HandleBell() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            isRingingBell = true;
            Debug.Log("Cling Cling! Customers alerted!");
            // TODO: Trigger animation and sound here
        }
        if (Input.GetKeyUp(KeyCode.Space)) isRingingBell = false;
    }
}