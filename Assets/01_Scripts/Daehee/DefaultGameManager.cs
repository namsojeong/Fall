using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefaultGameManager : MonoBehaviour
{
    GameTimeManager gameTimeManager;
    private void Awake()
    {
        gameTimeManager = GetComponent<GameTimeManager>();
    }
    private void Start()
    {
        gameTimeManager.StartTime(60);
    }

}
