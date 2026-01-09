using Objects.Interactables;
using Objects.Interactables.Items;
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
        hudCanvas.Hide();

        currentInteractable = null;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteractable();
    }


    void CheckForInteractable()
    {
        Ray checkRay = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));

        Debug.DrawRay(checkRay.origin, checkRay.direction, Color.red);

        if (Physics.SphereCast(checkRay, 0.05f, out var hit, interactionDistance))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable") || hit.collider.gameObject.layer == LayerMask.NameToLayer("InteractableNoOutline"))
            {
                Collider targetCollider = hit.collider;
                GameObject targetObject = targetCollider.gameObject;

                if (targetCollider.TryGetComponent(out IInteractable interactable))
                {
                    currentInteractable = interactable;

                    hudCanvas.Show(currentInteractable.InteractionPrompt);

                    return;
                }
            }
            else
            {
                hudCanvas.Hide();

                currentInteractable = null;
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
