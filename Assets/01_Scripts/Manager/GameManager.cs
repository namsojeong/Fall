using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WaterRippleForScreens;

public class GameManager : MonoSingleton<GameManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private bool isBoss = false;
    [SerializeField] public int curScore = 0;

    public int Score
    {
        get { return curScore; }
        set { curScore = value; }
    }
    public bool IsBoss
    {
        get { return isBoss; }
        set { isBoss = value; }
    }


}
