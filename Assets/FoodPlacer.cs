using UnityEngine;

public class FoodPlacer : MonoBehaviour
{
    public Camera cam;
    public LayerMask placementLayer;
    public GameObject foodPrefab;

    private GameObject currentFood;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentFood = Instantiate(foodPrefab);
        }

        if (currentFood == null) return;

        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 200f, placementLayer))
            {
                // Position slightly above surface
                currentFood.transform.position = hit.point + hit.normal * 0.01f;

                // Align to surface
                Quaternion align = Quaternion.FromToRotation(Vector3.up, hit.normal);
                currentFood.transform.rotation = align;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            currentFood = null;
        }
    }
}