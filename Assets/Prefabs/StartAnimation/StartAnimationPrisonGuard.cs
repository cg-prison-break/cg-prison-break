using System.Collections;
using UnityEngine;

namespace Prefabs.StartAnimation
{
    public class StartAnimationPrisonGuard : MonoBehaviour
    {
        private static readonly int WalkAgain = Animator.StringToHash("WalkAgain");
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");

        [Header("Audio Settings")]
        [SerializeField] private AudioClip firstGuardSpeech;
        [SerializeField] private AudioClip openKeySound;
        [SerializeField] private AudioClip lastGuardSpeech;
        [SerializeField] private AudioSource audioSource;
    
        [Header("Animation Settings")]
        [SerializeField] private Animator walkingAnimator;
        [SerializeField] private Animator doorAnimator;
        [SerializeField] private Animator guardAnimator;

        private readonly float _lengthOfFirstGuardSpeech = 6.5f;
        private readonly float _lengthOfOpenKeySound = 4.5f;
        private readonly float _lengthOfLastGuardSpeech = 7.5f;

        public StartAnimationPrisonGuard()
        {
            _lengthOfOpenKeySound = 0.5f;
        }

        private void Start()
        {
            StartCoroutine(GuardAnimationCoroutine());
        }

        private IEnumerator GuardAnimationCoroutine()
        {
            audioSource.PlayOneShot(firstGuardSpeech);
            yield return new WaitForSeconds(_lengthOfFirstGuardSpeech);
            audioSource.PlayOneShot(openKeySound);
            yield return new WaitForSeconds(_lengthOfOpenKeySound + 2);
            doorAnimator.SetBool(IsOpen, true);
            yield return new WaitForSeconds(1);
            audioSource.PlayOneShot(lastGuardSpeech);
            yield return new WaitForSeconds(_lengthOfLastGuardSpeech);
            guardAnimator.SetBool(IsWalking, true);
            walkingAnimator.SetBool(IsWalking, true);
        }
    }
}
