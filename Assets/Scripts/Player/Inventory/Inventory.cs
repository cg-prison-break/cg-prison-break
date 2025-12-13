using System;
using System.Collections.Generic;

public class Inventory
{
    private readonly List<ItemData> _list;
    private readonly int _maxSize;

    public Inventory(int maxSize)
    {
        if (maxSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxSize));

        _maxSize = maxSize;
        _list = new List<ItemData>(maxSize);
    }

    public int Count => _list.Count;
    public int MaxSize => _maxSize;

    public List<ItemData> Items => _list;

    public bool Contains(ItemData item) => _list.Contains(item);

    public void Add(ItemData item)
    {
        if (_list.Count >= _maxSize)
            throw new InventoryFullException(_maxSize);

        _list.Add(item);
    }

    public void AddRange(List<ItemData> items)
    {
        if (_list.Count + items.Count > _maxSize)
            throw new InventoryFullException(_maxSize);

        _list.AddRange(items);
    }

    public bool Remove(ItemData item) => _list.Remove(item);

    public ItemData FindByKeyPressed(int keyPressed) =>
        (keyPressed < 1 || keyPressed > _list.Count)
            ? throw new KeyNotMappedToItemException(keyPressed)
            : _list[keyPressed - 1];
}
