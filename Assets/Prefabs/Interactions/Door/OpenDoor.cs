using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Objects.Interactables;

public class OpenDoor : MonoBehaviour, IInteractableConnected
{
    [SerializeField] private List<ItemData> _connectedItems;
    public List<ItemData> ConnectedItems => _connectedItems;

    [SerializeField] private List<ItemData> _masterItems; // crowbar or similar

    public List<ItemData> MasterItems => _masterItems;

    [SerializeField] private Animator animator;

    [Header("Auto close")]
    [Tooltip("If < 0: no auto close")]
    [SerializeField] private float autoCloseDelay = 5.0f;
    [SerializeField] private float closeDuration = 1.0f;

    [SerializeField] private bool isOpen = false;
    private Coroutine autoCloseCoroutine;

    public string InteractionPrompt
    {
        get {
            var prompt = "";
            var player = PlayerRegistry.Player;
            if (player == null)
            {
                Debug.LogError("Player was not found.");
            }
            if (CanInteract(player))
            {
                prompt = $"Drücke F, um zu {(isOpen ? "schließen" : "öffnen")}.";
            } else
            {
                prompt = "Die Tür ist verschlossen. Suche nach einem passenden Gegenstand, um sie zu öffnen.";
            }
            return prompt;
        }
        set => InteractionPrompt = value;
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
        if (!CanInteract(player))
        {
            Debug.Log("Door is locked, you need the required item.");
            return;
        }

        foreach (var item in ConnectedItems)
        {
            GameTelemetryLogger.LogTelemetryEvent(new ItemUsedData(item.itemName));
        }

        // open the door
        Open();
    }

    public bool CanInteract(Player player)
    {
        return !Lockable()
            || (ConnectedItems.Count > 0 && player.HasOneOf(ConnectedItems))
            || (MasterItems.Count > 0 && player.HasOneOf(MasterItems));
    }

    public bool Lockable()
    {
        return ConnectedItems.Count > 0 || MasterItems.Count > 0;
    }

    public void Open()
    {
        if (isOpen) return;

        animator.SetBool("locked", false);
        StartCoroutine(InvokeAfter(closeDuration, () =>
        {
            isOpen = true;
            // inform NPCs when door is an actual lockable door
            if (Lockable())
            {
                NPCEventManager.NotifyNPCsAboutSuspiciousAction(transform.position);
                GameTelemetryLogger.LogTelemetryEvent(new SuspiciousEventTriggeredData("SecuredDoorOpened"));
            }

            // start auto close timer if enabled
            if (autoCloseDelay > 0)
            {
                if (autoCloseCoroutine != null) StopCoroutine(autoCloseCoroutine);
                autoCloseCoroutine = StartCoroutine(AutoCloseAfterDelay());
            }
            animator.SetBool("open", true);
            animator.SetBool("start", false);
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
