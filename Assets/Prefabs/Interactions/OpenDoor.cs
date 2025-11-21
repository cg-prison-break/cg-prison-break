using UnityEngine;
using System.Collections;

using Objects.Interactables;


public class OpenDoor : MonoBehaviour, IInteractableConnected
{
    [SerializeField]private ItemData _connectedItem;

    public ItemData ConnectedItem
    {
        get { return _connectedItem; }
        set { _connectedItem = value; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        if (!m_IsRotating && player.HasItem(ConnectedItem))
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
