using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefaultGameManager : MonoBehaviour
{
    private void Start()
    {
        GameTimeManager.Instance.StartTime(10);
    }

}
