using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceGame : MonoBehaviour
{
    private void Awake()
    {
        if (!AudioManager.instance.Initialized)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            AudioManager.instance.Initialze(audioSource);
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
}
