using UnityEngine;
using System.Collections;

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

    [SerializeField] private Material lockedMat;
    [SerializeField] private Material unlockedMat;

    [SerializeField] private float lockedIntensity = 0.5f;
    [SerializeField] private float unlockedIntensity = 0.5f;

    private bool locked = true;

    // Interaction prompt (read-only external)
    public string InteractionPrompt
    {
        get
        {
            if (locked) return "Locked";
            return "Press F to open door";
        }
        set
        {
            // intentionally left empty
        }
    }

    void Start()
    {
        locked = ConnectedItem != null; // start locked if door requires an item
    }

    void Update()
    {
        // update material based on locked state
        if (locked)
        {
            lockedMat.SetColor("_EmissionColor", lockedMat.color * lockedIntensity);
        }
        else
        {
            unlockedMat.SetColor("_EmissionColor", unlockedMat.color * unlockedIntensity);
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
