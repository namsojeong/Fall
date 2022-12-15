using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSound : MonoBehaviour
{
    [SerializeField] private AudioClip laserSound;
    [SerializeField] private bool isPlayingSound = false;
    // Update is called once per frame
    void Update()
    {
        if(!isPlayingSound)
        {
            SoundManager.instance.SFXPlay("laser", laserSound);
            isPlayingSound = true;
            StartCoroutine(SetMusicTime(laserSound.length));
        }
    }

    public IEnumerator SetMusicTime(float time)
    {
        isPlayingSound = true;
        yield return new WaitForSeconds(time);
        isPlayingSound = false;
    }
}
