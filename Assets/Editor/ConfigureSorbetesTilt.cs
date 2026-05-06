using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class ConfigureSorbetesTilt
{
    public static void Execute()
    {
        var cartGo = GameObject.Find("Sorbetes Cart");
        if (cartGo == null)
        {
            Debug.LogError("Sorbetes Cart not found in the scene.");
            return;
        }

        var tilt = cartGo.GetComponent<CartTiltEffect>();
        if (tilt == null)
        {
            tilt = cartGo.AddComponent<CartTiltEffect>();
            Debug.Log("Added CartTiltEffect to Sorbetes Cart.");
        }

        // Determine pivot from the wheels: average their local position so the
        // tilt rotates around the wheel axle like a real 2-wheeled cart.
        var ctrl = cartGo.GetComponent<CartController>();
        Vector3 pivot = Vector3.zero;
        int count = 0;
        if (ctrl != null && ctrl.wheels != null)
        {
            foreach (var w in ctrl.wheels)
            {
                if (w == null) continue;
                // Convert world pos to cart-local pos.
                pivot += cartGo.transform.InverseTransformPoint(w.position);
                count++;
            }
        }
        if (count > 0) pivot /= count;

        tilt.pivotLocalOffset = pivot;
        tilt.pitchWhenMoving = 7f;
        tilt.pitchWhenTurning = 4f;
        tilt.turnRockAmplitude = 3f;
        tilt.turnRockSpeed = 4f;
        tilt.tiltResponse = 6f;
        tilt.idleSwayAmplitude = 1.2f;
        tilt.idleSwaySpeed = 5f;

        Debug.Log($"CartTiltEffect configured. Pivot (local) = {pivot}, pitchWhenMoving = {tilt.pitchWhenMoving}, pitchWhenTurning = {tilt.pitchWhenTurning}, turnRockAmplitude = {tilt.turnRockAmplitude}");

        EditorUtility.SetDirty(cartGo);
        EditorSceneManager.MarkSceneDirty(cartGo.scene);
        EditorSceneManager.SaveScene(cartGo.scene);
    }
}
