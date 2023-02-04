using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class AudioManager : FastSingleton<AudioManager>
    {
        static bool initialized = false;
        static Dictionary<AudioName, AudioClip> audioClips = new Dictionary<AudioName, AudioClip>();
        static AudioSource audioSource;

        public bool Initialized
        {
            get { return initialized; }
        }

        public void Initialze(AudioSource source)
        {
            initialized = true;
            audioSource = source;
            audioClips.Add(AudioName.ButtonClick, Resources.Load<AudioClip>("ButtonClick"));
            audioClips.Add(AudioName.Win, Resources.Load<AudioClip>("Win"));
            audioClips.Add(AudioName.Start, Resources.Load<AudioClip>("Start"));
            audioClips.Add(AudioName.MusicThemeSong, Resources.Load<AudioClip>("MusicThemeSong"));
            audioClips.Add(AudioName.Lose, Resources.Load<AudioClip>("Lose"));
        }

        public void Play(AudioName name)
        {
            var audioTmp = audioClips[name];
            if (audioTmp)
            {
                audioSource.PlayOneShot(audioClips[name]);
            }
            else
            {
                Debug.Log("audio Load that bai: " + name);
            }
        
        
        }

        public void StopPlayAll()
        {
            audioSource.Stop(); 
        }

    }

}

