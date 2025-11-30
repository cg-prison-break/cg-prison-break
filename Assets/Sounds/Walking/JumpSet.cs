using UnityEngine;

namespace Sounds.Walking
{
    [System.Serializable]
    public class JumpSet
    {
        public string surfaceTag;
        public AudioClip[] jumpSounds;
        public AudioClip[] landingSounds;
    }
}