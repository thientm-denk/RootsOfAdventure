using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource musicTheme;
    private bool isPlay = true;

    public void MuteResMusic()
    {
        if (isPlay)
        {
            musicTheme.Pause();
            isPlay = false;
            return;
        }
        musicTheme.UnPause();
        isPlay = true;
    }
   
}
