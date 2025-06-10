using UnityEngine;
using UnityEngine.UI;

public class PlayerLightUI : MonoBehaviour
{
    public PlayerLightController lightController; // Reference to your light controller
    public Slider energySlider;                  // Reference to UI slider

    void Start()
    {
        if (energySlider != null && lightController != null)
        {
            energySlider.maxValue = lightController.maxEnergy;
            energySlider.minValue = 0;
        }
    }

    void Update()
    {
        if (energySlider != null && lightController != null)
        {
            energySlider.value = lightController.GetCurrentEnergy();
        }
    }
}
