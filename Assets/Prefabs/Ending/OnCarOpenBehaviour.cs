using System.Collections;
using UnityEngine;

namespace Prefabs.Ending
{
    public class OnCarOpenBehaviour : MonoBehaviour
    {
        private static readonly int Close = Animator.StringToHash("Close");
        public Player player;
        public Transform playerTarget;
        public Animator animator;

        public void OnCarOpened()
        {
            StartCoroutine(MovePlayerToTarget());
        }

        private IEnumerator MovePlayerToTarget()
        {
            var cc = player.GetComponent<CharacterController>();
            cc.enabled = false;

            var pt = playerTarget;

            var startPos = player.transform.position;
            var startRot = player.transform.rotation;

            var targetPos = pt.position;
            var targetRot = pt.rotation;

            const float duration = 1.0f;
            var elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                var t = elapsed / duration;

                player.transform.position = Vector3.Lerp(startPos, targetPos, t);
                player.transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

                yield return null;
            }

            player.transform.SetPositionAndRotation(targetPos, targetRot);
            // add Player as a Child of the Car
            player.transform.parent = pt;
            animator.SetTrigger(Close);
        }
    }
}
