using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSManager : MonoBehaviour
{
    [SerializeField] AudioClip vsClip;

    private void Start()
    {
        SoundManager.Instance.SFXPlay(vsClip);
    }
    public void LoadScene()
    {
        UI.Instance.ChangeScene(SceneState.BOSS_GAME);
    }
}
