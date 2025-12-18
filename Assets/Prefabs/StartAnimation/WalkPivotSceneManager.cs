using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prefabs.StartAnimation
{
    public class WalkPivotSceneManager : MonoBehaviour
    {
        public void OnWalkingFinished()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
