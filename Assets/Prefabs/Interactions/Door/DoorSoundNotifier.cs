using UnityEngine;

public class DoorSoundNotifier : MonoBehaviour
{
    [SerializeField] private AudioSource cardSwipeSound;
    [SerializeField] private AudioSource doorTurnSound;
    [SerializeField] private AudioSource doorLockSound;
    [SerializeField] private AudioSource doorUnlockSound;


    public void OnCardSwiped()
    {
        cardSwipeSound?.Play();
    }

    public void OnDoorTurned()
    {
        doorTurnSound?.Play();
    }

    public void OnDoorLocked()
    {
        doorLockSound?.Play();
    }

    public void OnDoorUnlocked()
    {
        doorUnlockSound?.Play();
    }
}