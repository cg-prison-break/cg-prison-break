using UnityEngine;
using System.Collections;
using UnityEditor.Animations;

using Objects.Interactables;

public class OpenDoor : MonoBehaviour, IInteractableConnected
{
    [SerializeField] private ItemData _connectedItem;
    public ItemData ConnectedItem
    {
        get { return _connectedItem; }
        set { _connectedItem = value; }
    }

    [SerializeField] private Animator animator;

    // Configurable parameters
    [Header("Rotation")]
    [SerializeField] private Vector3 rotationAngle = new Vector3(0f, -90f, 0f); // default (0, -90, 0)
    [SerializeField] private float duration = 1.0f;

    [Header("Auto close")]
    [Tooltip("If < 0: no auto close")]
    [SerializeField] private float autoCloseDelay = 5.0f; // seconds before auto-close

    // internal state
    private Quaternion initialRotation;
    private bool m_IsRotating = false;
    private bool isOpen = false;
    private bool locked = true;
    private Coroutine autoCloseCoroutine;

    // Interaction prompt (read-only external)
    public string InteractionPrompt
    {
        get
        {
            if (locked) return "Locked";
            return isOpen ? "Press F to Close" : "Press F to Open";
        }
        set
        {
            // intentionally left empty
        }
    }

    void Start()
    {
        initialRotation = transform.rotation;
        locked = ConnectedItem != null; // start locked if door requires an item
    }

    public void Interact(Player player)
    {
        if (m_IsRotating) return;

        if (isOpen)
        {
            // if open, allow closing by anyone (or require item if you prefer to enforce)
            Close();
            return;
        }

        // closed -> try to open
        if (ConnectedItem != null && !player.HasItem(ConnectedItem))
        {
            Debug.Log("Door is locked, you need the required item.");
            return;
        }

        // open the door
        Open();
    }

    public void Open()
    {
        if (m_IsRotating || isOpen) return;

        locked = false; // unlocking when opened
        animator?.SetBool("locked", locked);
        Quaternion target = initialRotation * Quaternion.Euler(rotationAngle);
        StartCoroutine(RotateTo(target, () =>
        {
            isOpen = true;
            // start auto close timer if enabled
            if (autoCloseDelay > 0)
            {
                if (autoCloseCoroutine != null) StopCoroutine(autoCloseCoroutine);
                autoCloseCoroutine = StartCoroutine(AutoCloseAfterDelay());
            }
        }));
    }

    public void Close()
    {
        if (m_IsRotating || !isOpen) return;

        // stop pending auto-close if any
        if (autoCloseCoroutine != null)
        {
            StopCoroutine(autoCloseCoroutine);
            autoCloseCoroutine = null;
        }

        StartCoroutine(RotateTo(initialRotation, () =>
        {
            isOpen = false;
            locked = true;
            animator?.SetBool("locked", locked);
        }));
    }

    private IEnumerator AutoCloseAfterDelay()
    {
        yield return new WaitForSeconds(autoCloseDelay);
        // Only close if still open and not rotating
        if (isOpen && !m_IsRotating)
        {
            Close();
        }
    }

    private IEnumerator RotateTo(Quaternion targetRotation, System.Action onComplete = null)
    {
        m_IsRotating = true;

        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
        m_IsRotating = false;
        onComplete?.Invoke();
    }
}
