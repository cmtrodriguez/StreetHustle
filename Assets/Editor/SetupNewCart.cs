using UnityEditor;
using UnityEngine;

public class SetupNewCart
{
    public static void Execute()
    {
        GameObject stallPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Models/wooden market stall 3d model.glb");
        GameObject wheelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Models/wooden wagon wheel 3d model.glb");

        if (stallPrefab == null || wheelPrefab == null)
        {
            Debug.LogError("Could not find the imported models.");
            return;
        }

        // Create root object
        GameObject cartRoot = new GameObject("New_Wooden_Cart");
        
        // Add MoveForward script
        MoveForward moveScript = cartRoot.AddComponent<MoveForward>();
        moveScript.speed = 1.0f;

        // Instantiate stall
        GameObject stall = PrefabUtility.InstantiatePrefab(stallPrefab) as GameObject;
        stall.transform.SetParent(cartRoot.transform);
        stall.transform.localPosition = Vector3.zero;
        stall.transform.localRotation = Quaternion.identity;
        stall.transform.localScale = Vector3.one;

        // Instantiate wheels
        float wheelScale = 0.4f;
        float wheelRadius = 0.5f * wheelScale;
        float wheelXOffset = 0.35f;
        float wheelYOffset = -0.435f;
        float wheelZOffset = 0.0f;

        // Left Wheel
        GameObject leftWheel = PrefabUtility.InstantiatePrefab(wheelPrefab) as GameObject;
        leftWheel.name = "Left_Wheel";
        leftWheel.transform.SetParent(cartRoot.transform);
        leftWheel.transform.localPosition = new Vector3(-wheelXOffset, wheelYOffset, wheelZOffset);
        leftWheel.transform.localRotation = Quaternion.identity;
        leftWheel.transform.localScale = new Vector3(wheelScale, wheelScale, wheelScale);
        
        RotateWheel leftRot = leftWheel.AddComponent<RotateWheel>();
        leftRot.speed = moveScript.speed;
        leftRot.wheelRadius = wheelRadius;

        // Right Wheel
        GameObject rightWheel = PrefabUtility.InstantiatePrefab(wheelPrefab) as GameObject;
        rightWheel.name = "Right_Wheel";
        rightWheel.transform.SetParent(cartRoot.transform);
        rightWheel.transform.localPosition = new Vector3(wheelXOffset, wheelYOffset, wheelZOffset);
        rightWheel.transform.localRotation = Quaternion.identity;
        rightWheel.transform.localScale = new Vector3(wheelScale, wheelScale, wheelScale);
        
        RotateWheel rightRot = rightWheel.AddComponent<RotateWheel>();
        rightRot.speed = moveScript.speed;
        rightRot.wheelRadius = wheelRadius;

        // Position the new cart where the old one was
        GameObject oldCart = GameObject.Find("vintage_cart");
        if (oldCart != null)
        {
            cartRoot.transform.position = oldCart.transform.position;
            cartRoot.transform.rotation = oldCart.transform.rotation;
            oldCart.SetActive(false);
        }

        Debug.Log("New cart setup complete.");
    }
}