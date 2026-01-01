using System.Collections;
using UnityEngine;

namespace Prefabs.Ending
{
    public class OnCarCloseBehaviour : MonoBehaviour
    {
        public AudioClip closeClip;
        public AudioClip carStartDriveClip;
        public Player player;

        public float driveSpeedKmh = 50f;
        public float accelerationDuration = 7f;

        private float speedMs;


        public void OnCarClose()
        {
            var audioSource = player.gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(closeClip);

            StartCoroutine(WaitAndStartCar());
        }


        private IEnumerator WaitAndStartCar()
        {
            yield return new WaitForSeconds(5f);

            player.GetComponent<AudioSource>().PlayOneShot(carStartDriveClip);
            yield return new WaitForSeconds(3f);
            
            speedMs = driveSpeedKmh / 3.6f;

            StartCoroutine(StartAndDriveCar());
        }


        private IEnumerator StartAndDriveCar()
        {
            float currentSpeed = 0f;  
            float elapsed = 0f;

            while (elapsed < accelerationDuration)
            {
                elapsed += Time.deltaTime;

                float t = elapsed / accelerationDuration;
                float smoothT = Mathf.SmoothStep(0f, 1f, t);

                currentSpeed = Mathf.Lerp(0f, speedMs, smoothT);

                transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

                yield return null;
            }

            while (true)
            {
                transform.Translate(Vector3.forward * speedMs * Time.deltaTime);
                yield return null;
            }
        }
    }
}
