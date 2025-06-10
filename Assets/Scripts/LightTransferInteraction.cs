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

    private GameObject currentGrabbedLight;
    private bool isHoldingLight = false;

    void Update()
    {
        float gripValue = gripAction.action.ReadValue<float>();
        bool gripPressed = gripValue > 0.5f;

        Debug.DrawRay(transform.position, transform.forward * 10f, Color.green); // Visual debug line

        if (gripPressed && !isHoldingLight && playerLightController.currentEnergy >= transferAmount)
        {
            Debug.Log($"Grip detected (value: {gripValue}), grabbing light.");
            GrabLight();
        }
        else if (!gripPressed && isHoldingLight)
        {
            Debug.Log("Grip released, attempting to transfer light.");
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
        Debug.Log("Light grabbed and visual instantiated.");
    }

    void ReleaseLight()
    {
        isHoldingLight = false;

        RaycastHit hit;
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, 10f))
        {
            Debug.Log($"Raycast hit: {hit.collider.gameObject.name}");

            NPCLightReceiver npc = hit.collider.GetComponent<NPCLightReceiver>();
            if (npc != null)
            {
                Debug.Log("NPC detected. Attempting to transfer energy.");
                bool success = playerLightController.TryConsumeEnergy(transferAmount);
                if (success)
                {
                    npc.ReceiveLight(transferAmount);
                    Debug.Log($"Energy transferred: {transferAmount}. Remaining energy: {playerLightController.GetCurrentEnergy()}");

                    currentGrabbedLight.transform.position = npc.transform.position;
                    Destroy(currentGrabbedLight, 1f);
                    currentGrabbedLight = null;
                    return;
                }
                else
                {
                    Debug.LogWarning("Not enough energy to transfer!");
                }
            }
            else
            {
                Debug.Log("Hit object is not an NPC.");
            }
        }
        else
        {
            Debug.Log("Raycast missed. No NPC hit.");
        }

        Destroy(currentGrabbedLight);
        currentGrabbedLight = null;
    }
}
