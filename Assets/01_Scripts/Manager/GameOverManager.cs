using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    private void Awake()
    {
        UI.Instance.SetCursor(false);
    }
}
