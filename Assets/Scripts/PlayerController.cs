using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float baseMoveSpeed = 3.0f;
    public float rotateSpeed = 10.0f;
    
    [Header("References")]
    public StaminaSystem staminaSystem;
    private CharacterController controller;
    
    [Header("Mechanics")]
    public float bellRadius = 15f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if(staminaSystem == null) staminaSystem = GetComponent<StaminaSystem>();
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

        HandleMovement();
        HandleBellPing();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(h, 0, v).normalized;
        bool isMoving = moveDir.magnitude > 0.1f;

        if (isMoving)
        {
            // Apply Stamina Drain
            staminaSystem.DrainStamina(staminaSystem.drainRate);

            // Calculate Speed based on stamina
            float currentSpeed = baseMoveSpeed * staminaSystem.GetSpeedModifier();
            
            // Move
            controller.Move(moveDir * currentSpeed * Time.deltaTime);
            
            // Look direction
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }
        else
        {
            staminaSystem.RegenStamina();
        }
    }

    private void HandleBellPing()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Ding! Bell Rang. Attracting customers...");
            // Call AI system to pull nearby unassigned customers
            Collider[] hits = Physics.OverlapSphere(transform.position, bellRadius);
            foreach(var hit in hits)
            {
                CustomerAI customer = hit.GetComponent<CustomerAI>();
                if (customer != null)
                {
                    customer.AttractToVendor(transform.position);
                }
            }
        }
    }
}
