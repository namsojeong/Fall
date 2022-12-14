using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoSingleton<UI>
{
    public Button resumeBtn;
    public GameObject pauseObj;
    public Button settingBtn;

    private void Awake()
    {
        resumeBtn.onClick.AddListener(ESCout);
        Debug.Log(resumeBtn);
    }
    
    public void ESCin()
    {
        pauseObj.SetActive(true);
        bool isPause = GameManager.Instance.IsPause;
        if (!isPause)
        {
            GameManager.Instance.IsPause = true;
            Time.timeScale = 0;
        }
    }
    public void ESCout()
    {
        pauseObj.SetActive(false);
        bool isPause = GameManager.Instance.IsPause;
        if(isPause)
        {
            GameManager.Instance.IsPause = false;
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

    public void ChangeScene(string scene)
    {
        if(scene=="DefaultGameScene")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        SceneManager.LoadScene(scene);
    }

}
