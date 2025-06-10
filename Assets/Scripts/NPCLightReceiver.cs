using UnityEngine;

public class NPCLightReceiver : MonoBehaviour
{
    public Light npcLight;
    public float currentEnergy = 0f;
    public float maxEnergy = 1000f;

    void Update()
    {
        npcLight.intensity = currentEnergy; 
    }

    public void ReceiveLight(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0f, maxEnergy);
    }
}
