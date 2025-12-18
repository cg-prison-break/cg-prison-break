using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prefabs.StartAnimation
{
    public class WalkPivotSceneManager : MonoBehaviour
    {
        [SerializeField] private GameObject loadingSceneIndicator;
        
        public void OnWalkingFinished()
        {
            loadingSceneIndicator.SetActive(true);
            SceneManager.LoadScene("MainScene");
        }
    }
}
