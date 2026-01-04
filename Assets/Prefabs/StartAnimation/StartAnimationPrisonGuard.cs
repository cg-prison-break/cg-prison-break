using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        [Header("TutorialPanel")]
        [SerializeField] private GameObject tutorialPanel;
        [SerializeField] private GameObject backButton;
        [SerializeField] private GameObject forwardButton;
        [SerializeField] private GameObject exitButton;
        private List<GameObject> _tutorialPages = new List<GameObject>();
        private int _currentTutorialIndex = 0;

        private readonly float _lengthOfFirstGuardSpeech = 6.5f;
        private readonly float _lengthOfOpenKeySound = 4.5f;
        private readonly float _lengthOfLastGuardSpeech = 7.5f;

        public StartAnimationPrisonGuard()
        {
            _lengthOfOpenKeySound = 0.5f;
        }

        private void Start()
        {
            CacheTutorialPages();
            showTutorialPanel();
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

        private void showTutorialPanel()
        {
            tutorialPanel.SetActive(true);
            SetCursorVisible(true);
            backButton.SetActive(false);
            forwardButton.SetActive(true);
            exitButton.SetActive(false);
            _currentTutorialIndex = 0;
            SetActiveTutorialPage(_currentTutorialIndex);
        }

        private void CacheTutorialPages()
        {
            _tutorialPages.Clear();
            if (tutorialPanel == null)
            {
                return;
            }

            foreach (Transform child in tutorialPanel.transform)
            {
                if (child != null)
                {
                    _tutorialPages.Add(child.gameObject);
                }
            }

            foreach (var page in _tutorialPages)
            {
                page.SetActive(false);
            }
        }

        private void SetActiveTutorialPage(int index)
        {
            if (_tutorialPages.Count == 0 || index < 0 || index >= _tutorialPages.Count)
            {
                return;
            }

            for (int i = 0; i < _tutorialPages.Count; i++)
            {
                _tutorialPages[i].SetActive(i == index);
            }
        }

        public void ShowNextTutorialPage()
        {
            if (_tutorialPages.Count == 0)
            {
                return;
            }

            int nextIndex = _currentTutorialIndex + 1;
            if (nextIndex >= _tutorialPages.Count)
            {
                return;
            }

            _currentTutorialIndex = nextIndex;
            UpdateButtonLayout();
            SetActiveTutorialPage(_currentTutorialIndex);
        }

        public void ShowPreviousTutorialPage()
        {
            if (_tutorialPages.Count == 0)
            {
                return;
            }

            int nextIndex = _currentTutorialIndex - 1;
            if (nextIndex < 0)
            {
                return;
            }

            _currentTutorialIndex = nextIndex;
            UpdateButtonLayout();
            SetActiveTutorialPage(_currentTutorialIndex);
        }

        private void UpdateButtonLayout()
        {
            backButton.SetActive(_currentTutorialIndex > 0);
            forwardButton.SetActive(_currentTutorialIndex < _tutorialPages.Count - 1);
            exitButton.SetActive(_currentTutorialIndex == _tutorialPages.Count - 1);
        }

        public void ExitTutorial()
        {
            tutorialPanel.SetActive(false);
            backButton.SetActive(false);
            forwardButton.SetActive(false);
            exitButton.SetActive(false);
            SetCursorVisible(false);
            StartCoroutine(GuardAnimationCoroutine());
        }

        private void SetCursorVisible(bool visible)
        {
            Cursor.visible = visible;
            Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
