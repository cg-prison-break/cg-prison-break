using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Objects.Interactables;

public class AccessScanner : MonoBehaviour, IInteractableConnected
{
    [SerializeField] private ItemData _connectedItem;
    public ItemData ConnectedItem
    {
        get { return _connectedItem; }
        set { _connectedItem = value; }
    }

    [Header("Door Interaction")]
    [SerializeField] private OpenDoor door;

    // Interaction prompt (read-only external)
    public string InteractionPrompt
    {
        get
        {
            return "Press F to scan Access Card";
        }
        set
        {
            // intentionally left empty
        }
    }

    public void Interact(Player player)
    {
        // closed -> try to open
        if (ConnectedItem != null && !player.HasItem(ConnectedItem))
        {
            Debug.Log("Door is locked, you need the required item.");
            return;
        }

        // open the door
        door.Open();
    }
}