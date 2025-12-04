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

    [Header("Auto close")]
    [Tooltip("If < 0: no auto close")]
    [SerializeField] private float autoCloseDelay = 5.0f;
    [SerializeField] private float closeDuration = 1.0f;

    // internal state
    private bool isOpen = false;
    private Coroutine autoCloseCoroutine;

    // Interaction prompt (read-only external)
    public string InteractionPrompt
    {
        get
        {
            return "Press F to Open";
        }
        set
        {
            // intentionally left empty
        }
    }

    void Start()
    {
    }

    public void Interact(Player player)
    {
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
        if (isOpen) return;

        animator.SetBool("locked", false);
        StartCoroutine(InvokeAfter(closeDuration, () =>
        {
            isOpen = true;
            // start auto close timer if enabled
            if (autoCloseDelay > 0)
            {
                if (autoCloseCoroutine != null) StopCoroutine(autoCloseCoroutine);
                autoCloseCoroutine = StartCoroutine(AutoCloseAfterDelay());
            }
            animator.SetBool("open", true);
        }));
    }

    public void Close()
    {
        if (!isOpen) return;

        // stop pending auto-close if any
        if (autoCloseCoroutine != null)
        {
            StopCoroutine(autoCloseCoroutine);
            autoCloseCoroutine = null;
        }

        StartCoroutine(InvokeAfter(closeDuration, () =>
        {
            isOpen = false;
            animator.SetBool("locked", true);
        }));
        animator.SetBool("open", false);
    }

    private IEnumerator AutoCloseAfterDelay()
    {
        yield return new WaitForSeconds(autoCloseDelay);
        // Only close if still open and not rotating
        if (isOpen)
        {
            Close();
        }
    }

    private IEnumerator InvokeAfter(float duration, System.Action onComplete)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        onComplete.Invoke();
    }
}
