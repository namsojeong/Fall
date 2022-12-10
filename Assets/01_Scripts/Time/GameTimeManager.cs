using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeManager : MonoSingleton<GameTimeManager>
{
    public Text timeText;

    private float curTime = Define.GAME_TIME;
    private bool isStoppedTime = false;

    public void ResetGameTime()
    {
        curTime = Define.GAME_TIME;
    }

    public void StartTime(int setTime)
    {
        curTime = setTime;
        isStoppedTime = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isStoppedTime = !isStoppedTime;
        }
        if (!isStoppedTime)
        {
            curTime -= Time.deltaTime;
            TimeUI();
        }
    }
    private void TimeUI()
    {
        timeText.text = string.Format($"{(int)curTime / 60}:{(int)curTime % 60}");
    }

}
