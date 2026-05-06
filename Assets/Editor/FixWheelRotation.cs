using UnityEditor;
using UnityEngine;

public class FixWheelRotation
{
    public static void Execute()
    {
        GameObject leftWheel = GameObject.Find("New_Wooden_Cart/Left_Wheel");
        GameObject rightWheel = GameObject.Find("New_Wooden_Cart/Right_Wheel");

        if (leftWheel != null)
        {
            leftWheel.transform.localRotation = Quaternion.Euler(0, 90, 0);
        }

        if (rightWheel != null)
        {
            rightWheel.transform.localRotation = Quaternion.Euler(0, 90, 0);
        }

        Debug.Log("Wheel rotation fixed.");
    }
}