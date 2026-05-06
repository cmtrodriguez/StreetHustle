using UnityEngine;
using System.Collections.Generic;

public class MoveForward : MonoBehaviour
{
    public float speed = 1.0f;
    public Transform playerHands;
    
    private List<Transform> fingerBones = new List<Transform>();
    private List<Quaternion> initialRotations = new List<Quaternion>();
    private List<Quaternion> targetRotations = new List<Quaternion>();
    
    private Vector3 initialHandsPos;
    private Vector3 targetHandsPos;
    
    private float transitionSpeed = 10f;
    private float currentTransition = 0f;

    void Start()
    {
        if (playerHands == null)
        {
            Transform hands = transform.Find("PlayerHands");
            if (hands != null)
            {
                playerHands = hands;
            }
        }
        
        if (playerHands != null)
        {
            initialHandsPos = playerHands.localPosition;
            targetHandsPos = initialHandsPos + new Vector3(0, 0, 0.3f); // Move hands forward when pushing
            
            FindFingerBones(playerHands);
        }
    }

    void FindFingerBones(Transform parent)
    {
        foreach (Transform child in parent)
        {
            string name = child.name.ToLower();
            if (name.Contains("index") || name.Contains("middle") || name.Contains("ring") || name.Contains("pinky") || name.Contains("thumb"))
            {
                fingerBones.Add(child);
                initialRotations.Add(child.localRotation);
                
                // Calculate a curled rotation for pushing
                // The exact axis depends on the rig, but usually X or Z curls the finger.
                // Let's try rotating around local X axis by 45 degrees for a generic curl.
                // We might need to adjust this based on the specific rig.
                Quaternion curl = Quaternion.Euler(45f, 0, 0);
                if (name.Contains("thumb"))
                {
                    curl = Quaternion.Euler(0, 45f, 0); // Thumb might curl differently
                }
                targetRotations.Add(child.localRotation * curl);
            }
            FindFingerBones(child);
        }
    }

    void Update()
    {
        bool isPushing = Input.GetKey(KeyCode.Space);
        
        if (isPushing)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            currentTransition = Mathf.MoveTowards(currentTransition, 1f, Time.deltaTime * transitionSpeed);
        }
        else
        {
            currentTransition = Mathf.MoveTowards(currentTransition, 0f, Time.deltaTime * transitionSpeed);
        }
        
        if (playerHands != null)
        {
            playerHands.localPosition = Vector3.Lerp(initialHandsPos, targetHandsPos, currentTransition);
            
            for (int i = 0; i < fingerBones.Count; i++)
            {
                fingerBones[i].localRotation = Quaternion.Lerp(initialRotations[i], targetRotations[i], currentTransition);
            }
        }
    }
}
