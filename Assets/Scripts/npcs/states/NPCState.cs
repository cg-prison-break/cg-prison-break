using UnityEngine;

public abstract class NPCState
{
    // optional state hint color
    protected virtual Color? StateHintColor => null;
    public abstract void EnterState(NPC npc);
    public abstract void UpdateState(NPC npc);
    public abstract void ExitState(NPC npc);

    protected void UpdateStateHint(NPC npc)
    {
        if (StateHintColor is null)
        {
            return;
        }

        foreach (Transform child in npc.transform)
        {
            if (child.name == "state_hint")
            {
                var meshRenderer = child.GetComponent<MeshRenderer>();
                Material mat = meshRenderer.material;
                mat.SetColor("_BaseColor", (Color)StateHintColor!);
            }
        }
    }
}
