using Objects.Interactables;
using Sounds.NPCs;
using UnityEngine;

namespace Prefabs.npcs
{
    public class IdlePrisoner : MonoBehaviour, IInteractable
    {
        [SerializeField] private NPCInteractionSoundSet soundSet;
        [SerializeField] private AudioSource audioSource;
        
        public string InteractionPrompt
        {
            get => "Drücke F, um mit Gefangenen zu sprechen.";
            set => InteractionPrompt = value;   
        }
        
        public void Interact(Player player)
        {
            transform.LookAt(player.transform);
            var rnd = new System.Random();
            audioSource.PlayOneShot(soundSet.interactionSounds[rnd.Next(soundSet.interactionSounds.Length)]);
        }
    }
}