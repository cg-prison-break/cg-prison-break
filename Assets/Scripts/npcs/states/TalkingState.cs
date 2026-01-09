using System.Collections;
using UnityEngine;

public class TalkingState : NPCState
{
    private AudioSource audioSource;
    private AudioClip audioClip;
    private NPCState previousState;

    public TalkingState(AudioSource audioSource, AudioClip audioClip, NPCState previousState)
    {
        this.audioSource = audioSource;
        this.audioClip = audioClip;
        this.previousState = previousState;
    }

    public override void EnterState(NPC npc)
    {
        if (audioSource != null && audioClip != null)
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
        npc.Movement.StopWalking();

        var originalRotation = npc.transform.rotation;
        var target = Quaternion.LookRotation((PlayerRegistry.Player.transform.position - npc.transform.position).normalized);

        float t = 0f;
        while (t < 0.15f)
        {
            npc.transform.rotation = Quaternion.Slerp(originalRotation, target, t / 0.15f);
            t += Time.deltaTime;
            yield return null;
        }
        npc.transform.rotation = target;
        yield return new WaitForSeconds(0.6f);

        // play sound
        audioSource.PlayOneShot(audioClip);
        yield return new WaitWhile(() => audioSource.isPlaying);

        // walking
        npc.Movement.StartWalking();
        npc.ChangeState(previousState);
    }
}
