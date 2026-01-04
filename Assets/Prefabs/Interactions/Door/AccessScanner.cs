using UnityEngine;
using System.Collections.Generic;
using Objects.Interactables;

public class AccessScanner : MonoBehaviour, IInteractableConnected
{
    [SerializeField] private List<ItemData> _connectedItems;
    public List<ItemData> ConnectedItems => _connectedItems;

    [SerializeField] private ItemData MasterCard;

    [Header("Door Interaction")]
    [SerializeField] private OpenDoor door;
    
    [SerializeField] private GameData gameData;

    // Interaction prompt (read-only external)
    public string InteractionPrompt
    {
        get
        {
            var interactionPrompt = "";
            var playerForItemChecking = PlayerRegistry.Player;
            if (playerForItemChecking == null)
            {
                Debug.LogError("Player was not found.");
            }
            if (playerForItemChecking.HasOneOf(ConnectedItems) || playerForItemChecking.HasItem(MasterCard))
            {
                interactionPrompt = "Drücke F, um mit der Sicherheitskarte die Tür zu öffnen.";
            }
            else
            {
                interactionPrompt = "Finde eine passende Sicherheitskarte, um die Tür zu öffnen.";
            }
            return interactionPrompt;
        }
        set
        {
            // intentionally left empty
        }
    }

    public void Interact(Player player)
    {
        GameObject usedCardPrefab;
        if (player.HasOneOf(ConnectedItems))
        {
            usedCardPrefab = ConnectedItems[0].prefab;
            GameTelemetryLogger.LogTelemetryEvent(new ItemUsedData(ConnectedItems[0].itemName));
        }
        else if (player.HasItem(MasterCard))
        {
            usedCardPrefab = MasterCard.prefab;
            GameTelemetryLogger.LogTelemetryEvent(new ItemUsedData(MasterCard.itemName));
        }
        else
        {
            Debug.Log("Access denied: You need a valid access card.");
            return; // no valid card
        }

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
        SusPoint[] allSusPoints = GetComponentsInChildren<SusPoint>();
        NPCEventManager.NotifyNPCsAboutSuspiciousAction(allSusPoints[Random.Range(0, allSusPoints.Length)].transform.position);
        GameTelemetryLogger.LogTelemetryEvent(new SuspiciousEventTriggeredData("AccessScanner"));

        // open the door
        door.Open();
    }
    
    private void FixedUpdate()
    {
        var player = PlayerRegistry.Player;
        // check if the player is near to the object, then set the layer of the object and all of its children to "Interactable"
        if (Vector3.Distance(transform.position, player.transform.position) < 5.5f)
        {
            SetLayerRecursively(gameObject, LayerMask.NameToLayer("Interactable"));
            if (!gameData.playWithInteractableShader)
            {
                // todo: implement logic for making lights on when shader is disabled
            }
        }
        else
        {
            SetLayerRecursively(gameObject, LayerMask.NameToLayer("Default"));
        }
    }
        
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}