using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] musicList;
    public AudioClip[] sfxList;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(string _sfxName)
    {
        //Play the SFX with the specified name
        foreach(AudioClip sfx in sfxList)
        {
            if (sfx.name == _sfxName)
            {
                audioSource.PlayOneShot(sfx);
            }
            else Debug.LogWarning("[AudioController]: Can not find audioclip " + _sfxName + "in sfxList.");
        }
    }
    public void PlaySFX(int _sfxID)
    {
        //Play the sfx with the specified id
        if(_sfxID >= 0 && _sfxID < sfxList.Length)
        {
            audioSource.PlayOneShot(sfxList[_sfxID]);
        }
        else Debug.LogWarning("[AudioController]: Can not find audioclip in position n." + _sfxID + "in sfxList.");
    }
    public void PlaySFX(AudioClip _sfx)
    {
        //Play the sfx with the audioclip object
        if(_sfx != null)
        {
            audioSource.PlayOneShot(_sfx);
        }
    }
    //---------------------------------------------
    public void PlayMusic(string _musicName)
    {
        //Play the SFX with the specified name
        foreach (AudioClip music in musicList)
        {
            if (music.name == _musicName)
            {
                audioSource.clip = music;
                audioSource.Play();
            }
            else Debug.LogWarning("[AudioController]: Can not find audioclip " +   _musicName + "in musicList.");
        }
    }
    public void PlayMusic(int _musicID)
    {
        //Play the sfx with the specified id
        if (_musicID >= 0 && _musicID < sfxList.Length)
        {
            audioSource.clip = sfxList[_musicID];
            audioSource.Play();
        }
        else Debug.LogWarning("[AudioController]: Can not find audioclip in position n." + _musicID + "in musicList.");
    }
    public void PlayMusic(AudioClip _music)
    {
        //Play the sfx with the audioclip object
        if (_music != null)
        {
            audioSource.clip = _music;
            audioSource.Play();
        }
    }


}
