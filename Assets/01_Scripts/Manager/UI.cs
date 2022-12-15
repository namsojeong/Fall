using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoSingleton<UI>
{
    public Button resumeBtn;
    public GameObject pauseObj;
    public Button settingBtn;
    public TMP_Text curScoreText;

    private bool isPause = true;

    private void Awake()
    {
        //resumeBtn.onClick.AddListener(ESCout);
        Debug.Log(resumeBtn);
    }

    public void ESCin()
    {
        pauseObj.SetActive(true);
        if (!isPause)
        {
            Time.timeScale = 0;
        }
    }

    public void ESCout()
    {
        pauseObj.SetActive(false);
        if (isPause)
        {
            isPause = false;
            Time.timeScale = 1;
        }
    }
    public void OffUI(GameObject ui)
    {
        ui.SetActive(false);
    }

    public void OnUI(GameObject ui)
    {
        ui.SetActive(true);
    }

    public void SetCursor(bool isOn)
    {
        Cursor.visible = !isOn;
        if (isOn)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

    }

    public void ChangeScene(SceneState scene)
    {
        GameManager.Instance.SetSceneState(scene);
        switch (scene)
        {
            case SceneState.START:
                SceneManager.LoadScene("Start");
                break;
            case SceneState.BASIC_GAME:
                SceneManager.LoadScene("DefaultGameScene");
                break;
            case SceneState.VS:
                SceneManager.LoadScene("vs");
                break;
            case SceneState.BOSS_GAME:
                SceneManager.LoadScene("BossScene");
                break;
            case SceneState.GAMEOVER:
                SceneManager.LoadScene("GameOver");
                break;
        }
    }

    public void ChangeScene(string scene)
    {
        switch (scene)
        {
            case "Start":
                GameManager.Instance.SetSceneState(SceneState.START);
                break;
            case "DefaultGameScene":
                GameManager.Instance.SetSceneState(SceneState.BASIC_GAME);
                break;
            case "vs":
                GameManager.Instance.SetSceneState(SceneState.VS);
                break;
            case "BossScene":
                GameManager.Instance.SetSceneState(SceneState.BOSS_GAME);
                break;
            case "GameOver":
                GameManager.Instance.SetSceneState(SceneState.GAMEOVER);
                break;
        }

        SceneManager.LoadScene(scene);
    }

}
