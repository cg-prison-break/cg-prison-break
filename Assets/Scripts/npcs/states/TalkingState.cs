
using Sounds.NPCs;
using UnityEngine;

public class TalkingState : NPCState
{
    private AudioSource audioSource;
    private NPCInteractionSoundSet soundSet;
    private NPCState previousState;
    private Player player;

    public TalkingState(AudioSource audioSource, NPCInteractionSoundSet soundSet, NPCState previousState, Player player)
    {
        this.audioSource = audioSource;
        this.soundSet = soundSet;
        this.previousState = previousState;
        this.player = player;
    }

    public override void EnterState(NPC npc)
    {
        // look into direction of player
        npc.transform.LookAt(player.transform);

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
        
    }

    public override void UpdateState(NPC npc)
    {
        if (!audioSource.isPlaying)
        {
            npc.ChangeState(previousState);
        }
    }
}
