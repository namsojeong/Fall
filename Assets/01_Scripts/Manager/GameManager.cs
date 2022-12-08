using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool isBoss = false;
    public bool IsBoss
    {
        get { return isBoss; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
