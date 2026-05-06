using UnityEngine;

public class PlaceholderModelGenerator : MonoBehaviour
{
    [Header("Generator Settings")]
    public bool generateOnStart = false;

    private void Start()
    {
        if (generateOnStart)
        {
            GenerateCart();
            GenerateCustomerPrefab();
        }
    }

    public void GenerateCart()
    {
        GameObject cart = new GameObject("VendorCart_Placeholder");
        
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.transform.SetParent(cart.transform);
        body.transform.localScale = new Vector3(2f, 1f, 1.5f);
        body.transform.localPosition = new Vector3(0, 0.5f, 0);
        body.GetComponent<Renderer>().material.color = Color.red; // Vendor cart color

        GameObject roof = GameObject.CreatePrimitive(PrimitiveType.Cube);
        roof.transform.SetParent(cart.transform);
        roof.transform.localScale = new Vector3(2.2f, 0.1f, 1.7f);
        roof.transform.localPosition = new Vector3(0, 2.0f, 0);
        roof.GetComponent<Renderer>().material.color = Color.yellow; // Umbrella/roof

        // Poles
        GameObject pole1 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        pole1.transform.SetParent(cart.transform);
        pole1.transform.localScale = new Vector3(0.1f, 0.5f, 0.1f);
        pole1.transform.localPosition = new Vector3(0.9f, 1.5f, 0.6f);

        GameObject pole2 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        pole2.transform.SetParent(cart.transform);
        pole2.transform.localScale = new Vector3(0.1f, 0.5f, 0.1f);
        pole2.transform.localPosition = new Vector3(-0.9f, 1.5f, -0.6f);

        Debug.Log("Generated Cart Placeholder Model.");
    }

    public void GenerateCustomerPrefab()
    {
        GameObject customer = new GameObject("Customer_Placeholder_Prefab");
        
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        body.transform.SetParent(customer.transform);
        body.transform.localPosition = new Vector3(0, 1f, 0);

        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        head.transform.SetParent(customer.transform);
        head.transform.localPosition = new Vector3(0, 2f, 0);
        head.GetComponent<Renderer>().material.color = Color.blue;

        Debug.Log("Generated Customer Placeholder Model. (Save as Prefab manually)");
    }
}
