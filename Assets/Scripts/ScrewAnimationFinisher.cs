using Prefabs.Interactions.screwdriver;
using UnityEngine;

public class ScrewAnimationFinisher : MonoBehaviour
{
    [SerializeField] private WindowScrewHandler windowScrewHandler;
    [SerializeField] private GameObject parent;
    
    public void OnScrewAnimationFinished()
    {
        Debug.Log("Notify about Screwing... Screwing Finished.");
        windowScrewHandler.notifyAboutUnscrewAction();
        Destroy(parent);
    }
}
