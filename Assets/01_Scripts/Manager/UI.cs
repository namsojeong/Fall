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
    }

    public void SetPause(bool isPause)
    {
        pauseObj.SetActive(isPause);
        if(isPause) Time.timeScale = 0;
        else Time.timeScale = 1;
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
        SetCursor(true);
        SceneManager.LoadScene((int)scene);
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

}
