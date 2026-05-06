using UnityEngine;

public static class InspectTilt
{
    public static void Execute()
    {
        var tiltBody = GameObject.Find("Sorbetes Cart/TiltBody");
        if (tiltBody == null)
        {
            Debug.LogError("TiltBody not found.");
            return;
        }
        var cart = GameObject.Find("Sorbetes Cart");
        Debug.Log($"TiltBody localRotation(euler) = {tiltBody.transform.localRotation.eulerAngles}");
        Debug.Log($"TiltBody localPosition = {tiltBody.transform.localPosition}");
        Debug.Log($"TiltBody has {tiltBody.transform.childCount} children");
        Debug.Log($"Sorbetes Cart has {cart.transform.childCount} direct children (should be wheels + TiltBody)");
        for (int i = 0; i < cart.transform.childCount; i++)
        {
            Debug.Log($"  child[{i}] = {cart.transform.GetChild(i).name}");
        }
    }
}
