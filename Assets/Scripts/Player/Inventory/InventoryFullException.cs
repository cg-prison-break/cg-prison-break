using System;

public class InventoryFullException : Exception
{
    public InventoryFullException(int maxSize)
        : base($"Inventory is full. Max size: {maxSize}")
    {
    }
}