using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GM gM;
    public AudioSource growingSource;
    public List<AudioClip> growSounds;

    public void PlayGrowingSFX(){
        growingSource.PlayOneShot(growSounds[Random.Range(0,growSounds.Count)]);
    }

    public AudioSource drinkSource;
    public List<AudioClip> drinkSounds;

    public void PlayDrinkSFX(){
        drinkSource.PlayOneShot(drinkSounds[Random.Range(0,drinkSounds.Count)]);
    }

    public AudioSource noteSource;
    public List<AudioClip> noteSounds;

    public void PlayNoteSFX(int i){
        i = i%noteSounds.Count;
        noteSource.PlayOneShot(noteSounds[i]);
    }

    public AudioSource musicSource;
    public AudioSource deathSource;
    public AudioClip die, rise, blossom, win, final;


    public void DieSFX(){
        musicSource.volume = 0;
        deathSource.Stop();
        deathSource.PlayOneShot(die);
    }

    public void EndSfx(){
        musicSource.volume = 0;
        deathSource.Stop();
        deathSource.PlayOneShot(win);
    }

    public void StartRise(){
        deathSource.Stop();

        deathSource.PlayOneShot(rise);
    }

    public void Blossom(){
        deathSource.Stop();

        deathSource.PlayOneShot(blossom);
    }

    public void BlossomEnd(){
        deathSource.Stop();

        deathSource.PlayOneShot(final);
    }

    

    public void Replay(){
        StartCoroutine(ShowDeathUIRoutine());
    }

    IEnumerator ShowDeathUIRoutine(){
        musicSource.volume = 0;
        float lerp = 0;
        while(lerp<0.35f){
            musicSource.volume = lerp;
            lerp +=Time.deltaTime;
            yield return null;
        }
        
    }

    public void MuteAudio(bool mute){
        growingSource.mute = mute;
        noteSource.mute = mute;
        drinkSource.mute = mute;
        deathSource.mute = mute;
        hurtSource.mute = mute;
    }

    public void MuteMusic(bool mute){
        musicSource.mute = mute;
    }
    public AudioSource hurtSource;

    public void HurtSfx(){
        hurtSource.volume = 1f;
        hurtSource.Play();
    }

    private void Update() {
        hurtSource.volume = Mathf.Min(1f,gM.rootControler.poisonned);
    }
}
