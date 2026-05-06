using UnityEditor;
using UnityEngine;

public class AdjustCart
{
    public static void Execute()
    {
        GameObject cartRoot = GameObject.Find("New_Wooden_Cart");
        if (cartRoot == null)
        {
            Debug.LogError("New_Wooden_Cart not found");
            return;
        }

        float wheelXOffset = 0.27f; // Closer to the stall (was 0.35)
        float wheelYOffset = -0.435f;
        float wheelZOffsetFront = 0.35f;
        float wheelZOffsetBack = -0.35f;

        GameObject flWheel = GameObject.Find("New_Wooden_Cart/Left_Wheel");
        if (flWheel != null) {
            flWheel.name = "Front_Left_Wheel";
            flWheel.transform.localPosition = new Vector3(-wheelXOffset, wheelYOffset, wheelZOffsetFront);
        } else {
            flWheel = GameObject.Find("New_Wooden_Cart/Front_Left_Wheel");
            if (flWheel != null) flWheel.transform.localPosition = new Vector3(-wheelXOffset, wheelYOffset, wheelZOffsetFront);
        }

        GameObject frWheel = GameObject.Find("New_Wooden_Cart/Right_Wheel");
        if (frWheel != null) {
            frWheel.name = "Front_Right_Wheel";
            frWheel.transform.localPosition = new Vector3(wheelXOffset, wheelYOffset, wheelZOffsetFront);
        } else {
            frWheel = GameObject.Find("New_Wooden_Cart/Front_Right_Wheel");
            if (frWheel != null) frWheel.transform.localPosition = new Vector3(wheelXOffset, wheelYOffset, wheelZOffsetFront);
        }

        // Create back wheels if they don't exist
        GameObject blWheel = GameObject.Find("New_Wooden_Cart/Back_Left_Wheel");
        if (blWheel == null && flWheel != null) {
            blWheel = Object.Instantiate(flWheel, cartRoot.transform);
            blWheel.name = "Back_Left_Wheel";
        }
        if (blWheel != null) {
            blWheel.transform.localPosition = new Vector3(-wheelXOffset, wheelYOffset, wheelZOffsetBack);
        }

        GameObject brWheel = GameObject.Find("New_Wooden_Cart/Back_Right_Wheel");
        if (brWheel == null && frWheel != null) {
            brWheel = Object.Instantiate(frWheel, cartRoot.transform);
            brWheel.name = "Back_Right_Wheel";
        }
        if (brWheel != null) {
            brWheel.transform.localPosition = new Vector3(wheelXOffset, wheelYOffset, wheelZOffsetBack);
        }

        // Handle Bilao
        GameObject bilao = GameObject.Find("banana leaf plate 3d model");
        if (bilao != null) {
            bilao.transform.SetParent(cartRoot.transform);
            // Estimate table height. Let's try Y = 0.05f
            bilao.transform.localPosition = new Vector3(0, 0.05f, 0);
        }
        
        Debug.Log("Cart adjusted.");
    }
}