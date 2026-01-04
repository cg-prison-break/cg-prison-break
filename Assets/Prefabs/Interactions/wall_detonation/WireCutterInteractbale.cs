using Objects.Interactables.Items;
using UnityEngine;

public class WireCutterInteractbale : MonoBehaviour, IInteractableItem
{
    [Header("Data")]
    [SerializeField] private ItemData _itemData;
    [SerializeField] private GameData gameData;

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

    public void Interact(Player player)
    {
        var pickedUp = player.AddItem(itemData);
        if (pickedUp) Destroy(gameObject);
    }
    
    private void FixedUpdate()
    {
        var player = PlayerRegistry.Player;
        // check if the player is near to the object, then set the layer of the object and all of its children to "Interactable"
        if (Vector3.Distance(transform.position, player.transform.position) < 5.5f)
        {
            SetLayerRecursively(gameObject, LayerMask.NameToLayer(GetInteractableLayerName()));
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
        
    private string GetInteractableLayerName()
    {
        return gameData.playWithInteractableShader ? "Interactable" : "InteractableNoOutline";
    }
}