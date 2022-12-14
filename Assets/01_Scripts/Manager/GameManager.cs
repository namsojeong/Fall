using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaterRippleForScreens;

public class GameManager : MonoSingleton<GameManager>
{

    [SerializeField] private bool isBoss = false;
    [SerializeField] private bool isPause = false;

    public bool IsBoss
    {
        get { return isBoss; }
        set { isBoss = value; }
    }

    public bool IsPause
    {
        get { return isPause; }
        set { isPause = value; }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            UI.Instance.ESCin();
    }
}
