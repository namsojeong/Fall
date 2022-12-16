using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoSingleton<ScoreManager>
{
    [SerializeField] Text scoreText;

    int score = 0;
    int bestScore = 0;

    private void Awake()
    {
        ResetScore();
    }

    private void ResetScore()
    {
        score = 0;
    }

    private void SaveScore()
    {

    }

    public void AddScore(int addScore)
    {
        score += addScore;
        Debug.Log(score);
        UpdateScoreUI();
    }
    
    public void UpdateScoreUI()
    {
        scoreText.text = $"SCORE\n{score}";
    }
    
}
