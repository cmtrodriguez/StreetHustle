using UnityEngine;
using UnityEditor;

public class SetupSorbetesCart
{
    [MenuItem("Tools/Setup Sorbetes Cart")]
    public static void Setup()
    {
        GameObject cart = GameObject.Find("Sorbetes Cart");
        if (cart == null)
        {
            Debug.LogError("Sorbetes Cart not found");
            return;
        }

        CartController controller = cart.GetComponent<CartController>();
        if (controller == null)
        {
            controller = cart.AddComponent<CartController>();
        }

        controller.forwardKey = KeyCode.W;
        controller.backKey = KeyCode.S;
        controller.turnLeftKey = KeyCode.A;
        controller.turnRightKey = KeyCode.D;

        controller.moveSpeed = 2.0f;
        controller.turnSpeedDegrees = 120f;
        controller.wheelRadius = 0.5f;

        Transform wheel1 = cart.transform.Find("wagon wheel 3d model");
        Transform wheel2 = cart.transform.Find("wagon wheel 3d model (1)");

        if (wheel1 != null && wheel2 != null)
        {
            controller.wheels = new Transform[] { wheel1, wheel2 };
        }
        else
        {
            Debug.LogError("Wheels not found");
        }

        EditorUtility.SetDirty(cart);
        Debug.Log("Sorbetes Cart setup complete");
    }
}
