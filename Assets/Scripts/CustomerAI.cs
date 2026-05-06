using UnityEngine;
using UnityEngine.AI;

public enum CustomerType { ExactPayer, NoChange, Haggler, RushBuyer, Regular }

[RequireComponent(typeof(NavMeshAgent))]
public class CustomerAI : MonoBehaviour
{
    [Header("Behavior")]
    public CustomerType customerType;
    public float patienceTime = 30f;
    
    private NavMeshAgent agent;
    private bool isAttracted = false;
    private bool isWaitingForOrder = false;
    private float currentPatience;

    private Transform vendorTarget;
    private string desiredOrder;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        // Randomize type
        customerType = (CustomerType)Random.Range(0, 5);
        
        if (customerType == CustomerType.RushBuyer)
            patienceTime = 15f;
    }

    private void Update()
    {
        if (isWaitingForOrder)
        {
            currentPatience -= Time.deltaTime;
            if (currentPatience <= 0)
            {
                Leave(false); // Left angry
            }
        }
        else if (isAttracted && vendorTarget != null)
        {
            // Simple logic: if arrived near vendor, start waiting
            if (Vector3.Distance(transform.position, vendorTarget.position) < 3.0f)
            {
                agent.isStopped = true;
                StartWaiting();
            }
        }
    }

    public void AttractToVendor(Vector3 vendorPos)
    {
        if (!isAttracted && !isWaitingForOrder)
        {
            isAttracted = true;
            vendorTarget = GameManager.Instance.dayManager.transform; // Simplified reference, usually would be player
            agent.SetDestination(vendorPos);
            agent.isStopped = false;
        }
    }

    private void StartWaiting()
    {
        isAttracted = false;
        isWaitingForOrder = true;
        currentPatience = patienceTime;
        GenerateOrder();
    }

    private void GenerateOrder()
    {
        // Generates an order based on current day
        desiredOrder = GameManager.Instance.dayManager.GetCurrentMenu();
        Debug.Log($"Customer wants: {desiredOrder} (Type: {customerType})");
        // In a full game, link this to UI
    }

    public void ReceiveFood(string foodName, float qualityScore)
    {
        if (!isWaitingForOrder) return;

        bool isCorrect = (foodName == desiredOrder);
        float payment = 10f; // Base cost

        if (isCorrect)
        {
            if (customerType == CustomerType.Haggler) payment *= 0.8f;
            else if (customerType == CustomerType.NoChange) payment = 20f; // Requires change logic in economy
            
            GameManager.Instance.economySystem.AddMoney(payment);
            Debug.Log($"Customer Satisfied! Score: {qualityScore}, Paid: {payment}");
            Leave(true);
        }
        else
        {
            Debug.Log("Wrong food! Customer Angry!");
            Leave(false);
        }
    }

    private void Leave(bool satisfied)
    {
        isWaitingForOrder = false;
        // Move away logic
        agent.isStopped = false;
        agent.SetDestination(transform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)));
        Destroy(gameObject, 5f); // Cleanup after leaving
    }
}
