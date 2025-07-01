using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class LightTransferInteraction : MonoBehaviour
{
    [Header("Input Action")]
    public InputActionProperty gripAction;

    [Header("Setup")]
    public Transform chestLightTransform;
    public GameObject grabbedLightVisualPrefab;
    public PlayerLightController playerLightController;
    public float transferAmount = 100f;

    [Header("Environment Brightness")]
    public GlobalLightLevelController lightLevelController;
    public float brightnessPerTransfer = 0.2f;

    [Header("God Character Glow")]
    public GodGlowController godGlowController;

    private GameObject currentGrabbedLight;
    private bool isHoldingLight = false;

    void Update()
    {
        float gripValue = gripAction.action.ReadValue<float>();
        bool gripPressed = gripValue > 0.5f;

        Debug.DrawRay(transform.position, transform.forward * 10f, Color.green);

        if (gripPressed && !isHoldingLight && playerLightController.currentEnergy >= transferAmount)
        {
            GrabLight();
        }
        else if (!gripPressed && isHoldingLight)
        {
            ReleaseLight();
        }

        if (isHoldingLight && currentGrabbedLight)
        {
            currentGrabbedLight.transform.position = transform.position;
        }
    }

    void GrabLight()
    {
        isHoldingLight = true;
        currentGrabbedLight = Instantiate(grabbedLightVisualPrefab, chestLightTransform.position, Quaternion.identity);
    }

    void ReleaseLight()
    {
        isHoldingLight = false;

        RaycastHit hit;
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, 10f))
        {
            NPCLightReceiver npc = hit.collider.GetComponent<NPCLightReceiver>();
            if (npc != null)
            {
                float spaceAvailable = npc.maxEnergy - npc.currentEnergy;

                if (spaceAvailable >= transferAmount)
                {
                    bool success = playerLightController.TryConsumeEnergy(transferAmount);
                    if (success)
                    {
                        npc.ReceiveLight(transferAmount);
                        lightLevelController?.DecreaseExposure(brightnessPerTransfer);
                        godGlowController?.AddReceivedLight(transferAmount);

                        Debug.Log("[LightTransfer] Transferring light to NPC, destroying visual");

                        Destroy(currentGrabbedLight); // no position update
                        currentGrabbedLight = null;

                        return;
                    }

                    else
                    {
                        Debug.LogWarning("[LightTransfer] Not enough player energy!");
                        Destroy(currentGrabbedLight);
                        currentGrabbedLight = null;

                    }
                }
                else
                {
                    Debug.Log("[LightTransfer] NPC is full. Transfer and consumption blocked.");
                    Destroy(currentGrabbedLight);
                    currentGrabbedLight = null;
  
                }
            }
            else
            {
                Debug.Log("[LightTransfer] Raycast hit non-NPC object.");
                Destroy(currentGrabbedLight);
                currentGrabbedLight = null;
            }
        }
        else
        {
            Debug.Log("[LightTransfer] Raycast missed.");
            Destroy(currentGrabbedLight);
            currentGrabbedLight = null;
        }

        // If transfer fails or is blocked, destroy visual
        Destroy(currentGrabbedLight);
        currentGrabbedLight = null;
    }

}
