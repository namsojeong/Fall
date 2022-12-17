using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text bestScoreText;

    private void Awake()
    {
        UI.Instance.SetCursor(false);
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
}
