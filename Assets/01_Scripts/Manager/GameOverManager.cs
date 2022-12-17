using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text bestScoreText;
    [SerializeField] Button playButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() => OnPlay());
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = string.Format($"SCORE\v{PlayerPrefs.GetInt("SCORE")}");
        bestScoreText.text = string.Format($"BEST SCORE {PlayerPrefs.GetInt("BEST_SCORE")}");
    }

    private void OnPlay()
    {
        UI.Instance.ChangeScene(SceneState.BASIC_GAME);
    }
}
