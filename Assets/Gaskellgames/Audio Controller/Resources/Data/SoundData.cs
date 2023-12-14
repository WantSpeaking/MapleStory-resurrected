using UnityEngine;
using UnityEngine.Audio;

using Gaskellgames;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.AudioController
{
    [System.Serializable]
    public class SoundData
    {
        public string ID;
        public string name;
        public string artist;
        public AudioClip clip;

        public AudioMixerGroup outputAudioMixerGroup;

        public bool mute;
        public bool bypassEffects;
        public bool bypassListenerEffects;
        public bool bypassReverbZones;
        public bool playOnAwake;
        public bool loop;

        [Range(0f, 256f)] public int priority=128;
        [Range(0f, 1f)] public float volume=1;
        [Range(-3f, 3f)] public float pitch=1;
        [Range(-1f, 1f)] public float panStereo = 0;
        [Range(0f, 1f)] public float spatialBlend = 0;
        [Range(0f, 1.1f)] public float reverbZoneMix = 1;

        [HideInInspector] public AudioSource source;

		public SoundData()
		{

		}

		public SoundData(string soundID, AudioClip audioClip, AudioMixerGroup group)
        {
            ID = soundID;
			name = soundID;
			clip = audioClip;
            outputAudioMixerGroup = group;

		}
    }
}