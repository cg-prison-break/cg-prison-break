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
    [SerializeField] private Player player;
    [SerializeField] private InteractPromptUI hudCanvas;

    
    [Header("Selection Visuals")]
    [SerializeField] private Material selectionMaterial;
    
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
        
        Debug.DrawRay(checkRay.origin, checkRay.direction.normalized * interactionDistance, Color.red);
        
        if (Physics.Raycast(checkRay, out RaycastHit hit, interactionDistance, interactionLayer))
        {
            Collider targetCollider = hit.collider;
            GameObject targetObject = targetCollider.gameObject;
            
            if (targetCollider.TryGetComponent(out IInteractable interactable))
            {
                currentInteractable = interactable;
                
                hudCanvas.Show(currentInteractable.InteractionPrompt);
                
                
                
                MeshFilter[] meshFilters = targetObject.GetComponentsInChildren<MeshFilter>();
                
                foreach (MeshFilter mesh in meshFilters)
                {
                    Graphics.DrawMesh(
                        mesh.sharedMesh,
                        mesh.transform.localToWorldMatrix,
                        selectionMaterial,
                        0
                        );
                }
                
                return;
            }
            
        }
        else
        {
            hudCanvas.Hide();
            
            currentInteractable = null;
        }
    }
    
    
    void PerformInteraction()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact(player);
            Debug.Log(currentInteractable.InteractionPrompt);
        }
    }
}
