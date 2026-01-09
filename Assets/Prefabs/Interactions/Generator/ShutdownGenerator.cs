using UnityEngine;
using System.Collections.Generic;
using Objects.Interactables;

public class ShutdownGenerator : MonoBehaviour, IInteractable
{
    [SerializeField] private List<ShutdownGenerator> generatorControllers;
    [SerializeField] private GameData gameData;
    public bool shutdown = false;

    public string InteractionPrompt
    {
        get => "Drücke F, um den Generator herunterzufahren.";
        set => InteractionPrompt = value;
    }

    public void Interact(Player player)
    {
        // make generator non-interactable
        shutdown = true;
        GameTelemetryLogger.LogTelemetryEvent(new GeneratorShutdownData());

        // check if any controller is still interactable
        if (generatorControllers.Exists(controller => !controller.shutdown))
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
    
    private void FixedUpdate()
    {
        var player = PlayerRegistry.Player;
        // check if the player is near to the object, then set the layer of the object and all of its children to "Interactable"
        if (Vector3.Distance(transform.position, player.transform.position) < gameData.interactableDisplayDistance)
        {
            gameObject.layer = LayerMask.NameToLayer(GetInteractableLayerName());
            if (!gameData.playWithInteractableShader)
            {
                // todo: implement logic for making lights on when shader is disabled
            }
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
    
    private string GetInteractableLayerName()
    {
        return gameData.playWithInteractableShader ? "Interactable" : "InteractableNoOutline";
    }
}