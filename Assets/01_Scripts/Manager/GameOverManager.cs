using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] AudioClip click;
    [SerializeField] AudioClip enter;
    [SerializeField] Button startButton;
    [SerializeField] Button settingButton;
    [SerializeField] Button quitButton;
    [SerializeField] Text scoreText;
    [SerializeField] Text bestScoreText;

    private void Awake()
    {
        startButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ClickPlay(click);
            OnPlay();
        });
        settingButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ClickPlay(click);
        });
        quitButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ClickPlay(click);
            ExitGame();
        });
    }


    public void OnButtonEnter(Button isOver)
    {
            SoundManager.Instance.ClickPlay(enter);
        isOver.image.color = new Color(255, 255, 255);
    }
    public void OnButtonExit(Button isOver)
    {
        isOver.image.color = Color.white;
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = string.Format($"SCORE\n{PlayerPrefs.GetInt("SCORE")}");
        bestScoreText.text = string.Format($"BEST SCORE {PlayerPrefs.GetInt("BEST_SCORE")}");
    }

    private void OnPlay()
    {
        UI.Instance.ChangeScene(SceneState.BASIC_GAME);
    }
}
