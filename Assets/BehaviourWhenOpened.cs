using Prefabs.Ending;
using UnityEngine;

public class BehaviourWhenOpened : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<OnCarOpenNotifier>().NotifyWhenOpened();
    }
}
