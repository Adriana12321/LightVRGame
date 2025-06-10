using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class LightTransferInteraction : MonoBehaviour
{
    [Header("Input Action")]
    public InputActionProperty gripAction; // replaces controller.inputDevice.TryGet...

    [Header("Setup")]
    public Transform chestLightTransform;
    public GameObject grabbedLightVisualPrefab;
    public PlayerLightController playerLightController;
    public float transferAmount = 100f;

    private GameObject currentGrabbedLight;
    private bool isHoldingLight = false;

    void Update()
    {
        bool gripPressed = gripAction.action.ReadValue<float>() > 0.5f;

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
            if (npc && playerLightController.TryConsumeEnergy(transferAmount))
            {
                npc.ReceiveLight(transferAmount);
                currentGrabbedLight.transform.position = npc.transform.position;
                Destroy(currentGrabbedLight, 1f);
                currentGrabbedLight = null;
                return;
            }
        }

        Destroy(currentGrabbedLight);
        currentGrabbedLight = null;
    }
}
