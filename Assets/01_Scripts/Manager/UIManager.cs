using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
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
        SceneManager.LoadScene(scene);
    }
}
