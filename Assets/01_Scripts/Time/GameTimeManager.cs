using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameTimeManager : MonoBehaviour
{

    public Text timeText;
    public string endScene = "vs";

    private float curTime = Define.GAME_TIME;

    public void ResetGameTime()
    {
        curTime = Define.GAME_TIME;
    }

    public void StartTime(int setTime)
    {
        ResetGameTime();
        curTime = setTime;
    }

    private void Update()
    {
         if(curTime <= 0)
         {
             EndTime();
         }
        TimeUpdate();
    }
    private void TimeUpdate()
    {
         curTime -= Time.deltaTime;
        timeText.text = string.Format($"{(int)curTime / 60}:{(int)curTime % 60}");
    }

    private void EndTime()
    {
        PlayerPrefs.SetInt("SCORE", 0);
        UI.Instance.ChangeScene(SceneState.GAMEOVER);
    }

}
