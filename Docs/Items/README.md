# Item Framework

## How it's set up

To properly represent the items in game the logic is split into two:

- The game world representation
  
    It defines the shape and behaviour of the in world object itself (Model, Textures, Scripts)
- The Data representation

    Contains the Information about the Item (Name, Sprite, Prefab)


## Game World representation

### Items
To define an object as an Item in the game world, it needs a script that implements the `IInteractableItem` interface.
The interface extends the demands with a reference to a `ScriptableObject ItemData`.

In C# the implementation looks like this:
```csharp
[Header("Data")]
[SerializeField] private ItemData _itemData;

public ItemData itemData
{
    get { return _itemData; }
    set { _itemData = value; }
}
```
Due to the interaction between ScriptableObjects and interface contracts we have to use this split workaround.

With this in place we can Drag the ScriptableObject into the field in the Inspector.

### Objects requiring Items
To define an object to require an Item to be used with it needs a script that implements the `IInteractableConnected` interface.
It is implemented in the same fashion as the `IInteractableItem`:
```csharp
[SerializeField]private ItemData _connectedItem;

public ItemData ConnectedItem
{
    get { return _connectedItem; }
    set { _connectedItem = value; }
}
```

The Interact logic can now assert if the Item is actually held by the player:
```csharp
if (interactor.HasItem(_connectedItem))
{
    // Actual Interaction logic
}
```


## Data representation

The actual Data of the Item is stored in a specific instance of the Item ScriptableObject.
These ScriptableObjects are not bound to a scene and are valid throughout the runtime.

For each Item implementation we need to create a new instance of the ScriptableObject
To do this right click the location in the filetree where you want the instance to be, select Create, then Inventory, then Item.
![CreateItem.png](CreateItem.png)

Then you can assign all the needed info in the inspector.

```csharp
public class ItemData : ScriptableObject
{
    public string id; // Currently unused
    public string itemName; 
    public Sprite icon; // Sprite to display in UI, May be changed dpending on the inventory UI implementation
    public GameObject prefab; // The corresponding prefab NOT scene object
}
```
