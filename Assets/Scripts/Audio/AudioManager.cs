using UnityEngine;
using UnityEngine.Audio;
namespace AudioSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField]
        private SFXHandler sfxHandler;
        [Tooltip("A reference to which Audio Mixer the source is outputting towards. ")]
        [SerializeField]
        private AudioMixer audioMixer;
        [Tooltip("The name of the exposed parameter representing volume in the Master Tracker of the Mixer")]
        [SerializeField]
        private string globalTrackVolumeParameterName = "masterVolume";

        [Tooltip("The name of the exposed parameter representing volume in the Music Tracker of the Mixer")]
        [SerializeField]
        private string musicTrackVolumeParameterName = "musicVolume";
        [Tooltip("The name of the exposed parameter representing volume in the Music Tracker of the Mixer")]
        [SerializeField]
        private string musicTrackPitchParameterName = "musicPitch";
        [Header("Fading Properties")]
        [SerializeField]
        private float fadeOutDuration;
        [SerializeField]
        private float fadeInDuration;
        private AudioSource audioSource;
        [Header("Current Music Track")]
        [SerializeField]
        private MusicTrack currMusicTrack;
        [Header("SFX Properties")]
        [SerializeField]
        private Vector2 sfxDefaultPitchRandomRange = new(0.9f, 1.1f);

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
        void Start()
        {
            sfxHandler.AudioMixer = this.audioMixer;
        }
        void Update()
        {
            audioMixer.GetFloat(musicTrackPitchParameterName, out float pitch);
            Debug.Log("Curr Music Pitch: " + pitch);
        }
        public void SetGlobalVolume(float _preferedVolume)
        {
            UtilsAudioMixer.SetVolume(audioMixer, globalTrackVolumeParameterName, _preferedVolume);
        }
        /// <summary>
        /// Fades out or completely stops current playing audio clip. 
        /// </summary>
        /// <param name="fadeMusicTrack"> Will it fade out the MusicTrack?</param>
        public void StopCurrentMusicTrack(bool fadeMusicTrack)
        {
            if (!fadeMusicTrack)
            {
                audioSource.Stop();
                return;
            }
            StartCoroutine(UtilsAudioMixer.StartFade(audioMixer, musicTrackVolumeParameterName, fadeOutDuration, 0.0001f));
        }
        /// <summary>
        /// Plays a new audio clip with the option of fading it in. If a clip is currently playing, it will fade out or stop
        /// according to preference.
        /// </summary>
        /// <param name="MusicTrack"> Which clip it will play. </param>
        /// <param name="fadeMusicTrack"> Will it fade in the audio? </param>
        public void PlayMusicTrack(MusicTrack MusicTrack, bool fadeMusicTrack)
        {
            float vol = PlayerSettings.MusicVolume;
            if (!fadeMusicTrack)
            {
                UtilsAudioMixer.SetVolume(audioMixer, musicTrackVolumeParameterName,
                    PlayerSettings.MusicVolume);
                PlayMusicTrack(MusicTrack);
                return;
            }
            if (!audioSource.isPlaying)
            {
                //Sets volume to the minumum, then fades in to the prefered volume.
                UtilsAudioMixer.SetVolume(audioMixer, musicTrackVolumeParameterName, 0);
                PlayMusicTrack(MusicTrack);
                StartCoroutine(UtilsAudioMixer.StartFade(audioMixer, musicTrackVolumeParameterName,
                    fadeInDuration, vol));
            }
            else
            {
                //Fades out previous track by setting its volume to 0, then fades in the new track.
                StartCoroutine(UtilsAudioMixer.StartFade(audioMixer, musicTrackVolumeParameterName,
                    fadeOutDuration, 0, MusicTrack, (paramClip) =>
                    {
                        PlayMusicTrack(paramClip);
                        StartCoroutine(UtilsAudioMixer.StartFade(audioMixer, musicTrackVolumeParameterName
                            , fadeInDuration, vol));
                    }));
            }
            this.currMusicTrack = MusicTrack;
        }
        /// <summary>
        /// Play a SFX Audio Clip with its original pitch.
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="pan"></param>
        public void PlaySFX(AudioClip clip, float pan = 0)
        {
            if (clip != null)
            {
                sfxHandler.PlaySFX(clip, PlayerSettings.SfxVolume, pan);
            }
        }
        /// <summary>
        /// Play a SFX Audio Clip with a custom pitch range. 
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="pan"></param>
        public void PlaySFX(AudioClip clip, Vector2 customPitchRange, float pan = 0)
        {
            if (clip != null)
            {
                sfxHandler.PlaySFX(clip, PlayerSettings.SfxVolume, customPitchRange, pan);
            }
        }
        /// <summary>
        /// Play a SFX Audio Clip with an exact pitch.
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="pan"></param>
        public void PlaySFX(AudioClip clip, float exactPitch, float pan = 0)
        {
            if (clip != null)
            {
                sfxHandler.PlaySFX(clip, PlayerSettings.SfxVolume, exactPitch, pan);
            }
        }
        /// <summary>
        /// Play a SFX Audio Clip with the default
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="pan"></param>
        public void PlaySFX(AudioClip clip, bool useDefaultSFXPitchRange, float pan = 0)
        {
            if (clip != null)
            {
                if (useDefaultSFXPitchRange)
                    sfxHandler.PlaySFX(clip, PlayerSettings.SfxVolume, this.sfxDefaultPitchRandomRange, pan);
                else
                    sfxHandler.PlaySFX(clip, PlayerSettings.SfxVolume, pan);
            }

        }

        private void PlayMusicTrack(MusicTrack MusicTrack)
        {
            audioSource.Stop();
            audioSource.clip = MusicTrack.MusicClip;
            audioSource.loop = MusicTrack.IsLoopable;
            audioSource.Play();
            audioMixer.SetFloat(musicTrackPitchParameterName, MusicTrack.Pitch);
            audioMixer.GetFloat(musicTrackPitchParameterName, out float pitch);
            Debug.Log("Curr Music Pitch: " + pitch);
        }
    }
}
