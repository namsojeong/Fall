using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioSource laser;
    [SerializeField] private AudioSource click;

    public void SFXPlay(AudioClip audio)
    {
        sfx.Stop();
        sfx.clip = audio;
        sfx.Play();
    }

    public void LaserPlay(AudioClip audio)
    {
        sfx.clip = audio;
        sfx.Play();
    }

    public void BGMPlay(AudioClip audio)
    {
        bgm.Stop();
        bgm.clip = audio;
        bgm.Play();
    }
    
    public void ClickPlay(AudioClip audio)
    {
        click.Stop();
        click.clip = audio;
        click.Play();
    }

    public void SetVolume(int val)
    {
        bgm.volume = val;
        sfx.volume = val;
        click.volume = val;
    }

    //public void SFXPlay(string sfxName, AudioClip clip)
    //{
    //    GameObject go = new GameObject(sfxName + "Sound");
    //    AudioSource audiosource = go.AddComponent<AudioSource>();
    //    if (audiosource.name == "laserSound")
    //        audiosource.spatialBlend = 1f;
    //    audiosource.clip = clip;
    //    Destroy(go, clip.length);
    //    audiosource.Play();
    //}


    //public IEnumerator SetMusicTime(float length)
    //{
    //    //isPlayingMusic = true;
    //    yield return new WaitForSeconds(length);
    //    //isPlayingMusic = false;
    //}
}