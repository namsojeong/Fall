using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoSingleton<UI>
{

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
