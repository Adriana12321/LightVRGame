using UnityEngine;

public class PlayerLightController : MonoBehaviour
{
    public Light chestLight;
    public float maxEnergy = 1000f;
    public float currentEnergy = 1000f;

    void Update()
    {
        chestLight.intensity = currentEnergy;
    }

    public bool TryConsumeEnergy(float amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            return true;
        }
        return false;
    }

    public float GetCurrentEnergy()
    {
        return currentEnergy;
    }
}
