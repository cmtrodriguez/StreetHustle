using UnityEngine;

public class RotateWheel : MonoBehaviour
{
    public float speed = 1.0f; // Should match the cart's speed
    public float wheelRadius = 0.5f; // Adjust based on scale

    void Update()
    {
        // Calculate rotation in degrees based on speed and radius
        // Circumference = 2 * pi * r
        // Distance per second = speed
        // Rotations per second = speed / circumference
        // Degrees per second = (speed / circumference) * 360
        
        float circumference = 2 * Mathf.PI * wheelRadius;
        float degreesPerSecond = (speed / circumference) * 360f;

        // Rotate around the X axis
        transform.Rotate(Vector3.right * degreesPerSecond * Time.deltaTime);
    }
}