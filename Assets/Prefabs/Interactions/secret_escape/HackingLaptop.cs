using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prefabs.Interactions.secret_escape
{
    public class HackingLaptop : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(WaitForSoundAndEndGame());
        }

        private static IEnumerator WaitForSoundAndEndGame()
        {
            yield return new WaitForSeconds(10f);
            EndingContext.NextEnding = EndingType.SecretEscapeUsed;
            SceneManager.LoadScene(GameScene.Ending);
        }
    }
}
