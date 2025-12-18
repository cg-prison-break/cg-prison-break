using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prefabs.StartAnimation
{
    public class WalkPivotSceneManager : MonoBehaviour
    {
        [SerializeField] private GameObject loadingSceneIndicator;
        [SerializeField] private GameData gameData;
        
        
        public void OnWalkingFinished()
        {
            gameData.animationPlayed = true;
            loadingSceneIndicator.SetActive(true);
            SceneManager.LoadScene("MainScene");
        }
    }
}
