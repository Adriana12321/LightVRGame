using UnityEngine;

public class GodGlowController : MonoBehaviour
{
    [Header("Material Glow")]
    public Renderer targetRenderer; // Mesh with HDRP Lit material
    public string emissiveColorProperty = "_EmissiveColor";
    public float maxGlowIntensity = 10000f;
    public float glowPerLight = 1000f;

    [Header("Light Source (Optional)")]
    public Light godLight; // Optional point light for extra glow
    public float maxLightIntensity = 5000f;

    private float accumulatedLight = 0f;
    private MaterialPropertyBlock propertyBlock;

    void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();

        if (targetRenderer == null)
        {
            targetRenderer = GetComponentInChildren<Renderer>();
        }

        if (targetRenderer != null)
        {
            Debug.Log("[GodGlowController] Renderer assigned: " + targetRenderer.gameObject.name);
        }
        else
        {
            Debug.LogWarning("[GodGlowController] No renderer found! Glow effect will not apply.");
        }

        if (godLight != null)
        {
            Debug.Log("[GodGlowController] Optional god light assigned: " + godLight.name);
        }
    }

    public void AddReceivedLight(float amount)
    {
        accumulatedLight += amount;

        float glowStrength = Mathf.Clamp01(accumulatedLight / glowPerLight);
        float emissiveIntensity = Mathf.Lerp(0f, maxGlowIntensity, glowStrength);

        Debug.Log($"[GodGlowController] Received {amount} light. Total: {accumulatedLight}. Glow strength: {glowStrength:F2}, Emissive: {emissiveIntensity}");

        // Apply emissive color
        if (targetRenderer != null)
        {
            targetRenderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor(emissiveColorProperty, Color.white * emissiveIntensity);
            targetRenderer.SetPropertyBlock(propertyBlock);
        }

        // Optionally update point light intensity
        if (godLight != null)
        {
            float lightIntensity = Mathf.Lerp(0f, maxLightIntensity, glowStrength);
            godLight.intensity = lightIntensity;
            Debug.Log($"[GodGlowController] Updated god light intensity to: {lightIntensity}");
        }
    }
}
