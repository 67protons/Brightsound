using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioSource musicSource;

    //public static AudioManager instance = null;

	//void Start ()
 //   {
 //       //Check for AudioManager
 //       if (instance == null)
 //           instance = this;
 //       else if (instance != this)
 //           Destroy(this.gameObject);

 //       //Use if you don't want to destroy between scenes.
 //       DontDestroyOnLoad(this.gameObject);	
	//}
	
    public void PlaySFXClip(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
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

    //Sets the master music volume where volume is a float in the range (0, 1.0)
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    //Sets the master SFX volume where volume is a float in the range (0, 1.0)
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    //Selects and plays a random clip from a given selection
    public void randomSFX(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        sfxSource.PlayOneShot(clips[randomIndex]);
    }

    //Note, this fade assumes the audio has already been playing, but at volume 0
    public IEnumerator fadeIn(AudioSource clip, float fadeRate) {
        float audioVolume = 0;
        clip.volume = audioVolume;
        while (audioVolume <= 1) {
            audioVolume += fadeRate * Time.deltaTime;
            //Cheap fix until I learn how to use WWise dynamic music
            AkSoundEngine.SetRTPCValue("TrackFade", audioVolume);
            clip.volume = audioVolume;
            yield return null;
        }

        //Note, uncomment if you want to try and fade clips in and out multiple time
        //Might be super buggy
        /*
        AudioFadeOut previousClip = clip.gameObject.GetComponent<AudioFadeOut>();
            if (previousClip != null)
                previousClip.activated = false;
        */
    }

    public IEnumerator fadeOut(AudioSource clip, float fadeRate) 
    {
        float audioVolume = clip.volume;
        while(audioVolume >= 0) 
        {
            audioVolume -= fadeRate * Time.deltaTime;
            clip.volume = audioVolume;
            yield return null;
        }

         //Note, uncomment if you want to try and fade clips in and out multiple time
         //Might be super buggy
         /*
         AudioFadeIn previousClip = clip.gameObject.GetComponent<AudioFadeIn>();
         if (previousClip != null)
             previousClip.activated = false;
         */
     }
 }
