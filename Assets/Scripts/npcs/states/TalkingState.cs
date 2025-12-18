
using Sounds.NPCs;
using UnityEngine;

public class TalkingState : NPCState
{
    private AudioSource audioSource;
    private NPCInteractionSoundSet soundSet;
    private NPCState previousState;

    public TalkingState(AudioSource audioSource, NPCInteractionSoundSet soundSet, NPCState previousState)
    {
        this.audioSource = audioSource;
        this.soundSet = soundSet;
        this.previousState = previousState;
    }

    public override void EnterState(NPC npc)
    {
        // look into direction of player
        npc.transform.LookAt(npc.playerRef.transform);
        npc.animator.SetBool("isWalking", false);

        // play random sound from soundset
        if (soundSet.interactionSounds.Length > 0)
        {
            var rnd = new System.Random();
            audioSource.PlayOneShot(soundSet.interactionSounds[rnd.Next(soundSet.interactionSounds.Length)]);
        }
        else
        {
            npc.ChangeState(previousState);
        }
    }

    public override void ExitState(NPC npc)
    {
        npc.animator.SetBool("isWalking", true);
    }

    public override void UpdateState(NPC npc)
    {
        if (!audioSource.isPlaying)
        {
            npc.ChangeState(previousState);
        }
    }
}
