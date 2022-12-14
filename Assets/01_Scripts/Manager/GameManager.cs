using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{

    private bool isBoss = false;
    public bool IsBoss
    {
        get { return isBoss; }
        set { isBoss = value; }
    }

}
