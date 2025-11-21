using UnityEngine;

using Objects.Interactables.Items;


public class AccessCard : MonoBehaviour, IInteractableItem
{
    [Header("Data")]
    [SerializeField] private ItemData _itemData;

    public ItemData itemData
    {
        get { return _itemData; }
        set { _itemData = value; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public string InteractionPrompt
    {
        get => "Click F to pick up!";
        set => InteractionPrompt = value;   
    }

    public void Interact(Player player)
    {
        bool pickedUp = player.AddItem(itemData);
        if (pickedUp) Destroy(gameObject);
    }
    
}
