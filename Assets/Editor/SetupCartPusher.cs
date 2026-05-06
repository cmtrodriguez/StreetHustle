using UnityEngine;
using UnityEditor;

public class SetupCartPusher
{
    public static void Execute()
    {
        GameObject cart = GameObject.Find("New_Wooden_Cart");
        if (cart == null) return;

        Transform oldHands = cart.transform.Find("PlayerHands");
        if (oldHands != null) GameObject.DestroyImmediate(oldHands.gameObject);

        GameObject handsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/HandyHands/hand_leftandright_prefab.prefab");
        if (handsPrefab != null)
        {
            GameObject hands = PrefabUtility.InstantiatePrefab(handsPrefab) as GameObject;
            hands.name = "PlayerHands";
            hands.transform.SetParent(cart.transform);
            
            hands.transform.localPosition = new Vector3(0, 0.8f, -1.2f); 
            hands.transform.localRotation = Quaternion.Euler(0, 0, 0);
            
            Transform leftHand = hands.transform.Find("hand_left");
            Transform rightHand = hands.transform.Find("hand_right");
            
            if (leftHand != null)
            {
                leftHand.localPosition = new Vector3(-0.3f, 0, 0);
                leftHand.localRotation = Quaternion.Euler(0, 90, 0); 
            }
            if (rightHand != null)
            {
                rightHand.localPosition = new Vector3(0.3f, 0, 0);
                rightHand.localRotation = Quaternion.Euler(0, -90, 0); 
            }
        }
    }
}