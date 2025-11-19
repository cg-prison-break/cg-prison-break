# Interacting with the Game World

## How it works

The player will produce a Raycast from the center of the player camera to the center of the Frustum (View Area) with a fixed length on every frame.
The Script then checks for collisions with the raycast. This check will only occur with objects on the Interactable Layer.

The Interaction distance / length of the raycast can be set on the player prefab in editor.
![InteractionDistance.png](InteractionDistance.png)

## Interactables

### Script and Interface

This concerns the immobile objects of the game world, which perform functions in place. Think doors.

To enable an object to be interactable you have to assign a script that implements the `IInteractable` interface.

```csharp
using Objects.Interactables;

public class InteractableObject : Monobehaviour, IInteractable 
{
    
}
```

The interface currently demands two implementations:

- A string that can be displayed by the (currently not impelemented) UI system
- A `Interact()` function that defines the behaviour of the object

Both are to be implemented on a per object basis.

# Example

This script will rotate the Object it is atteched to by 90° around the Y axis

```csharp
using System.Collections;
using Objects.Interactables;
using UnityEngine;


public class InitTestInteractableScript : MonoBehaviour, IInteractable
{
    private float duration = 1.0f;
    private Vector3 rotationAngle = new Vector3(0.0f, 90.0f, 0.0f);
    
    private bool m_IsRotating = false;
    
    public string InteractionPrompt
    {
        get => "Click F to spin!";
        set => InteractionPrompt = value;   
    }

    public void Interact(Player player)
    {
        Debug.Log("Interaction reached the Object!");
        if (!m_IsRotating)
        {
            StartCoroutine(Rotate());
        }
    }
    
    private IEnumerator Rotate()
    {
        m_IsRotating = true;
        
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = transform.rotation * Quaternion.Euler(rotationAngle);
        
        float elapsedTime = 0.0f;
        
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }
        
        transform.rotation = endRotation;  
        m_IsRotating = false;
    }
}

```


### Object

The Object itself must have a Collider of some sort (Collisionbox, MeshCollider, etc.)

It must be set to the Interactable Layer
![Layer.png](Layer.png)

Objects with a MeshRenderer have to be set to NON-static
![Static.png](Static.png)