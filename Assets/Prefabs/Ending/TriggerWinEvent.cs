using UnityEngine;
using UnityEngine.SceneManagement;

namespace Prefabs.Ending
{
    public class TriggerWinEvent : MonoBehaviour
    {
        private bool _triggered;

        private void OnTriggerEnter(Collider other)
        {
            if (_triggered || !other.CompareTag("Player"))
            {
                Debug.Log("Not the player!");
            }
            _triggered = true;
            Debug.Log("Player triggered end of Game and won!");
            SceneManager.LoadScene("MainMenu");
        }
    }
}
