using Sounds.NPCs;
using System.Collections;
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
        // play random sound from soundset
        if (soundSet.interactionSounds.Length > 0)
        {
            npc.StartCoroutine(TalkingCoroutine(npc));
        }
        else
        {
            npc.ChangeState(previousState);
        }
    }

    public override void ExitState(NPC npc) { }

    public override void UpdateState(NPC npc) { }

    private IEnumerator TalkingCoroutine(NPC npc)
    {
        // look into direction of player
        npc.transform.LookAt(npc.playerRef.transform);
        npc.animator.SetBool("isWalking", false);
        yield return new WaitForSeconds(0.5f);

        // play random sound
        var rnd = new System.Random();
        audioSource.PlayOneShot(soundSet.interactionSounds[rnd.Next(soundSet.interactionSounds.Length)]);
        yield return new WaitWhile(() => audioSource.isPlaying);

        // walking
        npc.animator.SetBool("isWalking", true);
        npc.ChangeState(previousState);
    }
}
