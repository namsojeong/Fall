using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WaterRippleForScreens;

public class GameManager : MonoSingleton<GameManager>
{

    [SerializeField] private bool isBoss = false;
    [SerializeField] public int curScore = 0;

    SceneState curScene = SceneState.START;

    public int CurScore
    {
        get { return curScore; }
        set { curScore = value; }
    }
    public bool IsBoss
    {
        get { return isBoss; }
        set { isBoss = value; }
    }

    public SceneState GetSceneState()
    {
        return curScene;
    }
    
    public void SetSceneState(SceneState change)
    {
        curScene = change;
    }

    private void Update()
    {
        //if (!isPlayingMusic)
        //{
        //    if (SceneManager.GetActiveScene().name == "Game")
        //    {
        //        SoundManager.instance.SFXPlay("GameSceneBgm",gameSceneBgm);
        //        isPlayingMusic = true;
        //        StartCoroutine(SetMusicTime(gameSceneBgm.length));
        //    }
        //    else if (SceneManager.GetActiveScene().name == "DefualtGameScene")
        //    {
        //        SoundManager.instance.SFXPlay("DefaltSceneBgm",defaultSceneBgm);
        //        isPlayingMusic = true;
        //        StartCoroutine(SetMusicTime(defaultSceneBgm.length));
        //    }
        //}
        //if(Input.GetKeyDown(KeyCode.Escape))
        //    UI.Instance.ESCin();
    }

}
