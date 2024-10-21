using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour{
    private static SoundManager instance;
    public static SoundManager Inst
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
            }
            return instance;
        }
    }
    public List<AudioClip> audioClips;
    public AudioSource audioSource;
    public Dictionary<string, AudioClip> audioClipDic = new Dictionary<string, AudioClip>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        audioSource = GetComponent<AudioSource>();
    }
    void Start(){
        for (int i = 0; i < audioClips.Count; i++)
        {
            audioClipDic.Add(audioClips[i].name, audioClips[i]);
        }
    }
    public void PlaySound(int index)
    {
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }
    public void PlaySound(string name)
    {
        audioSource.clip = audioClipDic[name];
        audioSource.Play();
    }
    public void StopSound()
    {
        audioSource.Stop();
    }
    public void PauseSound()
    {
        audioSource.Pause();
    }
    public void UnPauseSound()
    {
        audioSource.UnPause();
    }
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }
    public void SetLoop(bool isLoop)
    {
        audioSource.loop = isLoop;
    }
    public void SetPitch(float pitch)
    {
        audioSource.pitch = pitch;
    }
    public void SetAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
    }
    public void PlayOneShot(string clipName)
    {
        AudioClip clip= audioClipDic[clipName];
        if(clip != null)
            audioSource.PlayOneShot(clip);
    }
}