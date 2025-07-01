using UnityEngine;

public class NPCLightReceiver : MonoBehaviour
{
    public Light npcLight;
    public float currentEnergy = 0f;
    public float maxEnergy = 100f;

    [Header("God Logic")]
    public bool isGod = false; // ← new flag

    public GodTouchEndTrigger godTouchEndTrigger; // Optional reference for triggering

    void Update()
    {
        if (npcLight != null)
            npcLight.intensity = currentEnergy;
    }

    public void ReceiveLight(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0f, maxEnergy);

        if (isGod && godTouchEndTrigger != null)
        {
            godTouchEndTrigger.TriggerEnding(); // Manually call the fade/ending
        }
    }
}
