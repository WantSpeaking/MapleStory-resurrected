using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Gaskellgames;

/// <summary>
/// Code created by Gaskellgames
/// </summary>

namespace Gaskellgames.AudioController
{
    public class SoundController : SingletonMono<SoundController>
    {
        #region Variables

        private enum musicOnLoadOptions
        {
            None,
            SingleTrack,
            Playlist,
            PlaylistFollowingSingleTrack
        }
        private SoundManager soundManager;

        //[Header("UI Audio Info")] [Space]

        [SerializeField] private TMP_Text songNameHUD;
        [SerializeField] private TMP_Text artistNameHUD;
        [SerializeField] private Image albumArtworkHUD;

        //[Header("Stop Sounds on Load Scene")] [Space]

        [SerializeField] private bool stopMusic = true;
        [SerializeField] private bool stopEnvironment = true;
        [SerializeField] private bool stopSoundFX = true;
        [SerializeField] private bool stopMenuUI = true;

        //[Header("Play Music on Load Scene")] [Space]

        [SerializeField] private musicOnLoadOptions playOnLoad;
        [SerializeField] private string trackID = "";
        [SerializeField] private Sprite albumArtwork;
        [SerializeField] private bool loop = false;
        private musicOnLoadOptions playOnLoadCheck;
        private bool singleTrackPlayed = false;
        private bool singleTrackHUD = false;

        [SerializeField] private string playlistID = "";
        [SerializeField] private bool shuffle = false;
        [SerializeField] private bool repeat = false;
        private int playlistIndex = 0;

        [ReadOnly, SerializeField] private List<string> tracksPlayed;
        [ReadOnly, SerializeField] private List<string> tracksUnplayed;
        [ReadOnly, SerializeField] private PlaylistData currentPlaylist;
        private int currentPlaylistIndex;
        private int tracksPlayedCheck;
        private float trackTimer;

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Game Loop

        private void Start()
        {
            InitialiseStartValues();
        }

        private void Update()
        {
            UpdateLogic();
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

#if UNITY_EDITOR

        #region Reset & OnDrawGizmos [Editor]

        private void OnDrawGizmosSelected()
        {
            if(!Application.isPlaying)
            {
                UpdateInspector();
            }
        }

        #endregion

#endif

        //----------------------------------------------------------------------------------------------------

        #region Initialisation

        private void InitialiseStartValues()
        {
            soundManager = FindObjectOfType<SoundManager>();
            trackTimer = 0;
            tracksPlayedCheck = 0;
            currentPlaylistIndex = 0;
            tracksPlayed = new List<string>();
            tracksUnplayed = new List<string>();
            playOnLoadCheck = musicOnLoadOptions.None;

            if (soundManager != null)
            {
                LoadPlaylist();

                // stop sounds
                if (stopMusic)
                {
                    soundManager.StopMusic();
                }
                if (stopEnvironment)
                {
                    soundManager.StopEnvironment();
                }
                if (stopSoundFX)
                {
                    soundManager.StopSoundFX();
                }
                if (stopMenuUI)
                {
                    soundManager.StopMenuUI();
                }
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Logic

        private void UpdateInspector()
        {
            if(playOnLoadCheck != playOnLoad)
            {
                if (playOnLoad == musicOnLoadOptions.SingleTrack)
                {
                    playlistID = "";
                    shuffle = false;
                    repeat = false;
                }
                else if (playOnLoad == musicOnLoadOptions.Playlist)
                {
                    trackID = "";
                    loop = false;
                }
                else if (playOnLoad == musicOnLoadOptions.PlaylistFollowingSingleTrack)
                {
                    loop = false;
                }
                else
                {
                    trackID = "";
                    loop = false;

                    playlistID = "";
                    shuffle = false;
                    repeat = false;
                }

                playOnLoadCheck = playOnLoad;
            }
        }

        private void UpdateLogic()
        {
            if (soundManager != null)
            {
                if (trackID != "" && (!singleTrackPlayed || loop))
                {
                    if((playOnLoad == musicOnLoadOptions.SingleTrack || playOnLoad == musicOnLoadOptions.PlaylistFollowingSingleTrack) && !singleTrackHUD)
                    {
                        UpdateHUD(albumArtwork, trackID);
                        singleTrackHUD = true;
                    }

                    if (!loop)
                    {
                        singleTrackPlayed = true;
                        soundManager.PlayMusic(trackID);
                    }
                    else if (!soundManager.AudioSourceMusic.isPlaying || trackTimer > soundManager.AudioSourceMusic.clip.length)
                    {
                        trackTimer = 0;
                        soundManager.PlayMusic(trackID);
                    }
                }
                else if (playlistID != "")
                {
                    if (currentPlaylist != null && (!soundManager.AudioSourceMusic.isPlaying || trackTimer > soundManager.AudioSourceMusic.clip.length) )
                    {
                        if (tracksPlayedCheck >= currentPlaylist.musicTracks.Count && repeat)
                        {
                            ResetPlayedTracks();
                        }

                        if (tracksPlayedCheck < currentPlaylist.musicTracks.Count)
                        {
                            PlayNextTrack(currentPlaylistIndex);
                            UpdateHUD(currentPlaylist.sprite, currentPlaylist.musicTracks[currentPlaylistIndex]);
                            SelectNextTrack();
                        }
                    }
                }

                UpdateTrackTimer();
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Private Functions

        private void UpdateHUD(Sprite sprite, string songID)
        {
            if(albumArtworkHUD != null)
            {
                albumArtworkHUD.sprite = sprite;
            }

            if (soundManager != null && songNameHUD != null)
            {
                songNameHUD.text = soundManager.GetSoundNameFromID(songID);
            }

            if (soundManager != null && artistNameHUD != null)
            {
                artistNameHUD.text = soundManager.GetSoundArtistFromID(songID);
            }
        }

        private void UpdateTrackTimer()
        {
            trackTimer += Time.deltaTime;
        }

        private void LoadPlaylist()
        {
            currentPlaylist = soundManager.GetPlaylistData(playlistID);
            currentPlaylistIndex = playlistIndex;

            if(currentPlaylist != null)
            {
                ResetPlayedTracks();
            }
        }

        private void ResetPlayedTracks()
        {
            tracksPlayedCheck = 0;
            tracksPlayed.Clear();
            tracksUnplayed.Clear();

            for (int i = 0; i < currentPlaylist.musicTracks.Count; i++)
            {
                tracksUnplayed.Add(currentPlaylist.musicTracks[i]);
            }
        }

        private void SelectNextTrack()
        {
            if(shuffle && tracksUnplayed.Count > 0)
            {
                int temp_selected = Random.Range(0, tracksUnplayed.Count);

                for(int i = 0; i < currentPlaylist.musicTracks.Count; i++)
                {
                    if (tracksUnplayed[temp_selected] == currentPlaylist.musicTracks[i])
                    {
                        currentPlaylistIndex = i;
                    }
                }
            }
            else
            {
                if (currentPlaylistIndex >= currentPlaylist.musicTracks.Count - 1)
                {
                    currentPlaylistIndex = 0;
                }
                else
                {
                    currentPlaylistIndex++;
                }
            }
        }

        private void PlayNextTrack(int index)
        {
            string track = currentPlaylist.musicTracks[index];

            trackTimer = 0;
            tracksPlayedCheck++;
            tracksPlayed.Add(track);
            tracksUnplayed.Remove(track);
            soundManager.PlayMusic(track);
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Public Functions

        public void PlayMusic(string soundID)
        {
            if (soundManager != null)
            {
                soundManager.PlayMusic(soundID);
            }
        }

        public void PlaySoundFX(string soundID)
        {
            if (soundManager != null)
            {
                soundManager.PlaySoundFX(soundID);
            }
        }
		public void PlayMusic(AudioClip clip)
		{
			if (soundManager != null)
			{
				soundManager.PlayMusic(clip);
			}
		}

		public void PlaySoundFX(AudioClip clip)
		{
			if (soundManager != null)
			{
				soundManager.PlaySoundFX(clip);
			}
		}
		public void PlayEnvironment(string soundID)
        {
            if (soundManager != null)
            {
                soundManager.PlayEnvironment(soundID);
            }
        }

        public void PlayMenuUI(string soundID)
        {
            if (soundManager != null)
            {
                soundManager.PlayMenuUI(soundID);
            }
        }
        public void PauseAllSounds()
        {
            PauseMusic();
            PauseSoundFX();
            PauseEnvironment();
            PauseMenuUI();
        }

        public void PauseMusic()
        {
            if (soundManager != null)
            {
                soundManager.PauseMusic();
            }
        }

        public void PauseSoundFX()
        {
            if (soundManager != null)
            {
                soundManager.PauseSoundFX();
            }
        }

        public void PauseEnvironment()
        {
            if (soundManager != null)
            {
                soundManager.PauseEnvironment();
            }
        }

        public void PauseMenuUI()
        {
            if (soundManager != null)
            {
                soundManager.PauseMenuUI();
            }
        }

        public void StopAllSounds()
        {
            StopMusic();
            StopSoundFX();
            StopEnvironment();
            StopMenuUI();
        }

        public void StopMusic()
        {
            if (soundManager != null)
            {
                soundManager.StopMusic();
            }
        }

        public void StopSoundFX()
        {
            if (soundManager != null)
            {
                soundManager.StopSoundFX();
            }
        }

        public void StopEnvironment()
        {
            if (soundManager != null)
            {
                soundManager.StopEnvironment();
            }
        }

        public void StopMenuUI()
        {
            if (soundManager != null)
            {
                soundManager.StopMenuUI();
            }
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Editor Functions

#if UNITY_EDITOR

        public string GetMusicOnLoad() { return playOnLoad.ToString(); }

#endif
        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Getters / Setters

        public SoundManager SoundManager
        {
            get { return soundManager; }
        }

        #endregion

    } //class end
}
