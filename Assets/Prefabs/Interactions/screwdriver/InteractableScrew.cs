using Objects.Interactables;
using Prefabs.Interactions.screwdriver;
using UnityEngine;

public class InteractableScrew : MonoBehaviour, IInteractableConnected
{
    [SerializeField]private ItemData _connectedItem;
    public Animator animator;
    public GameObject parent;
    public GameObject animatedScrewDriver;
    public WindowScrewHandler windowScrewHandler;
    private bool _isScrewing;
    
    public string InteractionPrompt
    {
        get => "Press F to unscrew.";
        set => InteractionPrompt = value;   
    }

    public ItemData ConnectedItem
    {
        get { return _connectedItem; }
        set { _connectedItem = value; }
    }


    public void Interact(Player interactor)
    {
        if (!interactor.HasItem(_connectedItem)) return;
        if (_isScrewing) return;

        Debug.Log("Screwing...");
        Debug.Log("Screwing...");
        animatedScrewDriver.SetActive(true);
        _isScrewing = true;
        animator.Play("ScrewAnimation");
    }
    
    public void OnScrewAnimationFinished()
    {
        Debug.Log("Screwing Finished.");
        windowScrewHandler.notifyAboutUnscrewAction();
        Destroy(parent);
    }
}
