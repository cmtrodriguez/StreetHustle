using UnityEditor;
using UnityEngine;

public class AttachMoveForward
{
    public static void Execute()
    {
        GameObject cart = GameObject.Find("vintage_cart");
        if (cart != null)
        {
            if (cart.GetComponent<MoveForward>() == null)
            {
                cart.AddComponent<MoveForward>();
                Debug.Log("Added MoveForward to vintage_cart");
            }
        }
        else
        {
            Debug.Log("vintage_cart not found");
        }
    }
}