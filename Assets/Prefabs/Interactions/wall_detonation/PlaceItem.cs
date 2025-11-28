using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceItem : MonoBehaviour
{
    public Player player;

    private void PlaceItemAtFeet(ItemData item)
    {
        var placedItem = Instantiate(item.prefab, player.transform.position, Quaternion.identity);
        player.RemoveItem(item);
    }

    public void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            var keyNumber = int.Parse(Keyboard.current.digit1Key.displayName);
            Debug.Log(keyNumber);
            var items = player.GetItems();
            var item = items[keyNumber - 1];

            PlaceItemAtFeet(item);
        }
    }
}
