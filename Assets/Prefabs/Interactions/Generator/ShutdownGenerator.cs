using UnityEngine;
using System.Collections.Generic;
using Objects.Interactables;

public class ShutdownGenerator : MonoBehaviour, IInteractable
{
    public string InteractionPrompt
    {
        get => "Drücke F, um den Generator herunterzufahren.";
        set => InteractionPrompt = value;
    }

    public void Interact(Player player)
    {
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