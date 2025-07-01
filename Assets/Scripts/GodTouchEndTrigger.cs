using UnityEngine;
using UnityEngine.SceneManagement;

public class GodTouchEndTrigger : MonoBehaviour
{
    [Tooltip("Tag of the player that triggers the end.")]
    public string playerTag = "Player";

    [Tooltip("Reference to the ScreenFader script.")]
    public ScreenFaderStarter screenFaderStarter;

    private bool triggered = false;

    void Start()
    {
        if (screenFaderStarter == null)
        {
            screenFaderStarter = FindObjectOfType<ScreenFaderStarter>();
            if (screenFaderStarter == null)
                Debug.LogWarning("[GodTouchEndTrigger] No ScreenFaderStarter found!");
        }
    }

    public void TriggerEnding()
    {
        if (triggered) return;

        triggered = true;
        Debug.Log("[GodTouchEndTrigger] Triggered by light transfer or collision.");

        if (screenFaderStarter != null)
        {
            screenFaderStarter.FadeToWhite();
            Debug.Log("[GodTouchEndTrigger] Fade to white triggered.");
        }

        Invoke(nameof(EndGame), 3f); // Match fade duration
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        TriggerEnding(); // Reuse the logic
    }

    void EndGame()
    {
        Debug.Log("[GodTouchEndTrigger] Game Ended.");
        // SceneManager.LoadScene("EndScene");
        // Application.Quit();
    }
}
