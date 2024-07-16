using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace Controllers
{
    public class MusicPlayerController : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<AudioClip> audioClips;
        
        public void PlayRandomSongOnLoop()
        {
            LoadRandomSong();
            
            audioSource.Play();
            
            audioSource.loop = true;
        }

        public void PlaySong(AudioClip song)
        {
            audioSource.clip = song;
            
            audioSource.Play();
        }

        public void PauseMusic()
        {
            if(audioSource.isPlaying)
                audioSource.Pause();
            else
                audioSource.UnPause();
        }
        
        private void LoadRandomSong()
        {
            if (audioClips.Any())
                audioSource.clip = audioClips[Random.Range(0, audioClips.Count)];
        }
    }
}
