using UnityEngine;
using System.Collections.Generic;
using Objects.Interactables;

public class ShutdownGenerator : MonoBehaviour, IInteractable
{
    [SerializeField] private List<GameObject> generatorControllers;
    public string InteractionPrompt
    {
        get => "Drücke F, um den Generator herunterzufahren.";
        set => InteractionPrompt = value;
    }

    public void Interact(Player player)
    {
        // make generator non-interactable
        gameObject.layer = LayerMask.NameToLayer("Default");
        GameTelemetryLogger.LogTelemetryEvent(new GeneratorShutdownData());

        // check if any controller is still interactable
        if (generatorControllers.Exists(controller => controller.layer == LayerMask.NameToLayer("Interactable")))
        {
            Debug.Log("Cannot shut down generator: controllers are still active.");
            return;
        }

        Debug.Log("Generator shut down successfully. Opening doors and alerting guards.");
        OpenDoors();
        NPCEventManager.MakeAllPrisonGuardsAlwaysSuspicious();
    }

    private void OpenDoors()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        Debug.LogWarning($"Found {doors.Length} doors to open.");
        foreach (GameObject door in doors)
        {
            OpenDoor openDoor = door.GetComponent<OpenDoor>();
            if (openDoor != null)
            {
                openDoor.OpenInstantly();
            }
        }
    }
}