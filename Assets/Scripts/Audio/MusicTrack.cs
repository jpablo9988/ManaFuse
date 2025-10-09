using UnityEngine;
namespace AudioSystem
{
    [CreateAssetMenu(menuName = "Audio/Music Track", fileName = "New Music Track")]
    public class MusicTrack : ScriptableObject
    {
        [Tooltip("Name of the track. ")]
        [SerializeField]
        private string musicName;
        [Tooltip("Reference to the audio clip. ")]
        [SerializeField]
        private AudioClip musicClip;
        [Tooltip("Will the track loop? ")]
        [SerializeField]
        private bool isLoopable;
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float audioPitch = 1.0f;


        public AudioClip MusicClip { get { return musicClip; } private set { musicClip = value; } }
        public bool IsLoopable { get { return isLoopable; } private set { isLoopable = value; } }
        public float Pitch => audioPitch;
    }
}
