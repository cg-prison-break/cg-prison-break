using System;

public class KeyNotMappedToItemException : Exception
{
    public KeyNotMappedToItemException(int keyPressed)
        : base($"Key {keyPressed} is not mapped to any item.")
    {
    }
}