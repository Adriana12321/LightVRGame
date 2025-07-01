using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class GlobalLightLevelController : MonoBehaviour
{
    public Volume globalVolume;
    public float maxExposure = 15f; // start at 15 = darkest
    public float minExposure = 11f;  // 0 = brightest
    public float exposureStep = 0.5f;

    private Exposure exposure;

    void Start()
    {
        if (globalVolume == null)
        {
            Debug.LogError("[GlobalLightLevelController] No volume assigned.");
            return;
        }

        if (globalVolume.profile.TryGet<Exposure>(out exposure))
        {
            exposure.fixedExposure.value = maxExposure;
        }
        else
        {
            Debug.LogError("[GlobalLightLevelController] Exposure override not found on Volume!");
        }
    }

    public void DecreaseExposure(float amount)
    {
        if (exposure != null)
        {
            exposure.fixedExposure.value = Mathf.Clamp(
                exposure.fixedExposure.value - amount,
                minExposure,
                maxExposure
            );
            Debug.Log($"[GlobalLightLevelController] Exposure now: {exposure.fixedExposure.value}");
        }
    }
}
