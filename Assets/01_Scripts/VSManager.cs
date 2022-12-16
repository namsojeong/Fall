using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSManager : MonoBehaviour
{
    
    public void LoadScene()
    {
        UI.Instance.ChangeScene(SceneState.BOSS_GAME);
    }
}
