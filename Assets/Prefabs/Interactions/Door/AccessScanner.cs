using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Objects.Interactables;

public class AccessScanner : MonoBehaviour, IInteractableConnected
{
    [SerializeField] private List<ItemData> _connectedItems;
    public List<ItemData> ConnectedItems => _connectedItems;

    [SerializeField] private ItemData MasterCard;

    [SerializeField] private string _interactionPrompt = "Press F to scan Access Card";


    [Header("Door Interaction")]
    [SerializeField] private OpenDoor door;

    // Interaction prompt (read-only external)
    public string InteractionPrompt
    {
        get
        {
            return _interactionPrompt;
        }
        set
        {
            // intentionally left empty
        }
    }

    public void Interact(Player player)
    {
        GameObject usedCardPrefab = player.HasOneOf(ConnectedItems) ? ConnectedItems[0].prefab :
                              player.HasItem(MasterCard) ? MasterCard.prefab : null;
        if (usedCardPrefab == null) return; // no valid card

        // get used card material
        Material usedCardMesh = usedCardPrefab.GetComponent<MeshRenderer>().sharedMaterial;

        // set card material for each side
        for (int i = 0; i < transform.childCount; i++)
        {
            var side = transform.GetChild(i);
            for (int j = 0; j < side.childCount; j++)
            {
                var card = side.GetChild(j);
                if (card.name != "AccessCard") continue;
                // set material on access card
                card.GetComponent<MeshRenderer>().material = usedCardMesh;
            }
        }
        // notify about suspicious action
        NPCEventManager.NotifyNPCsAboutSuspiciousAction(player.transform.position);
        
        // open the door
        door.Open();
    }
}