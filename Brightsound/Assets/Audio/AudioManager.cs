using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioSource musicSource;

    public static AudioManager instance = null;

	void Start ()
    {
        //Check for AudioManager
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        //Use if you don't want to destroy between scenes.
        DontDestroyOnLoad(this.gameObject);	
	}
	
    public void PlaySFXClip(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    //Note: AudioManager is set to only play one song at a time
    public void PlayMusic(AudioClip clip)
    {
        if(musicSource.isPlaying)
            musicSource.Stop();
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Stop();
    }

    //Warning: This will stop ALL currently playing SFX
    public void StopSFX()
    {
        if (sfxSource.isPlaying)
            sfxSource.Stop();
    }

    //Sets the music volume where volume is a float in the range (0, 1.0)
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    //Sets the SFX volume where volume is a float in the range (0, 1.0)
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
