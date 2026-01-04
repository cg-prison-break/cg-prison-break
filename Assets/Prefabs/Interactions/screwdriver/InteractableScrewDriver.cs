using Objects.Interactables.Items;
using UnityEngine;

public class InteractableScrewDriver : MonoBehaviour, IInteractableItem
{
    [Header("Data")]
    [SerializeField] private ItemData _itemData;

    public ItemData itemData
    {
        get { return _itemData; }
        set { _itemData = value; }
    }

    public string InteractionPrompt
    {
        get => $"Drücke F, um \"{_itemData.itemName}\" aufzunehmen.";
        set => InteractionPrompt = value;   
    }

    public void Interact(Player interactor)
    {
        interactor.AddItem(itemData);
        NPCEventManager.NotifyNPCsAboutSuspiciousAction(interactor.transform.position);
        GameTelemetryLogger.LogTelemetryEvent(new SuspiciousEventTriggeredData("ScrewDriverUsed"));
        Destroy(gameObject);
    }
}
