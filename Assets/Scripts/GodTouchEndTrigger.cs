using UnityEngine;
using UnityEngine.SceneManagement;

public class GodTouchEndTrigger : MonoBehaviour
{

    [Tooltip("Tag of the player that triggers the end.")]
    public string playerTag = "Player";

    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag(playerTag)) return;

        Debug.Log("[GodTouchEndTrigger] Player touched God. Triggering end sequence.");
        triggered = true;

        if (endingObject != null)
        {
            // Try to fade to white
            ScreenFader fader = endingObject.GetComponent<ScreenFader>();
            if (fader != null)
            {
                fader.FadeToWhite();
                Debug.Log("[GodTouchEndTrigger] Fade to white triggered.");
            }
            else
            {
                Debug.LogWarning("[GodTouchEndTrigger] ScreenFader component not found on endingObject.");
            }
        }
        else
        {
            Debug.LogWarning("[GodTouchEndTrigger] No endingObject assigned.");
        }

        // End game logic delayed until after fade
        Invoke(nameof(EndGame), 3f); // Adjust time to match fadeDuration
    }

    void EndGame()
    {
        Debug.Log("[GodTouchEndTrigger] Game Ended. (Override this to load end scene or quit)");
        // Uncomment and replace with your actual logic:
        // SceneManager.LoadScene("EndScene");
        // Application.Quit();
    }
}
