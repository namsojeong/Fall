using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameTimeManager : MonoSingleton<GameTimeManager>
{
    public Text timeText;

    private float curTime = Define.GAME_TIME;
    private bool isStoppedTime = false;

    public string endScene = "VS";
    public void ResetGameTime()
    {
        curTime = Define.GAME_TIME;
    }

    public void StartTime(int setTime)
    {
        curTime = setTime;
        isStoppedTime = false;
    }

    private void Awake()
    {
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
            if(curTime <= 0)
            {
                EndTime();
            }
            TimeUI();
        }
    }
    private void TimeUI()
    {
        timeText.text = string.Format($"{(int)curTime / 60}:{(int)curTime % 60}");
    }

    private void EndTime()
    {
        if(GameManager.Instance.GetSceneState() == SceneState.BASIC_GAME)
        {
            UI.Instance.SetCursor(true);
        UI.Instance.ChangeScene("vs");
        }
        else
        {
            UI.Instance.SetCursor(false);
        UI.Instance.ChangeScene("GameOver");
        }

    }

}
