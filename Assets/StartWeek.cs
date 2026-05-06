using UnityEngine;
using UnityEngine.SceneManagement;

public class StartWeekButton : MonoBehaviour
{
    public TransitionVideoOverlay transition;

    public void OnStartWeekPressed()
    {
        Debug.Log("Start Week clicked");

        if (transition == null)
        {
            Debug.LogError("Transition reference missing!");
            return;
        }

        transition.Play(
            () =>
            {
                Debug.Log("Loading next scene...");
                SceneManager.LoadScene("GameScene"); // CHANGE THIS
            },
            () =>
            {
                Debug.Log("Transition complete");
            }
        );
    }
}