using Objects.Interactables;
using UnityEngine;

// delay the order of class execution
[DefaultExecutionOrder(100)]
public class InteractWithObject : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public LayerMask interactionLayer;
    
    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Camera playerCamera;
    
    
    
    private IInteractable currentInteractable;
    
    
    
    private void Awake()
    {
        if (playerController == null)
        {
            playerController = GetComponent<PlayerController>();
        }
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }
    
    private void OnEnable()
    {
        if (playerController != null)
        {
            playerController.OnInteractPerformed += PerformInteraction;
        }
    }
    
    private void OnDisable()
    {
        if (playerController != null)
        {
            playerController.OnInteractPerformed += PerformInteraction;
        }
    }
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteractable();
    }
    
    
    void CheckForInteractable()
    {
        Ray checkRay = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
        
    
        if (Physics.Raycast(checkRay, out RaycastHit hit, interactionDistance, interactionLayer))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                currentInteractable = interactable;
                
                // TODO  call UI framework to display the interactable prompt
            }
            else
            {
                // TODO remove prompt from UI if present
                
                
                currentInteractable = null;
            }
            
        }
        else
        {
            // TODO remove prompt from UI if present
            
            
            currentInteractable = null;
        }
    }
    
    
    void PerformInteraction()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }
}
