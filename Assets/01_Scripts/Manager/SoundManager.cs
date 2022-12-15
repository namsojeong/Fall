using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        if (audiosource.name == "laserSound")
            audiosource.spatialBlend = 1f;
        audiosource.clip = clip;
        Destroy(go, clip.length);
        audiosource.Play();
    }
}