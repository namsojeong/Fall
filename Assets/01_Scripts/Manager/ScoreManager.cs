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
        GetScore();
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("SCORE", score);
        PlayerPrefs.SetInt("BEST_SCORE", bestScore);
    }
    private void GetScore()
    {
        score = PlayerPrefs.GetInt("SCORE", 0);
        bestScore = PlayerPrefs.GetInt("BEST_SCORE", 0);
    }

    public void AddScore(int addScore)
    {
        score += addScore;
        if(score > bestScore)
        {
            bestScore = score;
            SaveScore();
        }
        UpdateScoreUI();
    }
    
    public void UpdateScoreUI()
    {
        scoreText.text = $"SCORE\n{score}";
    }
    
}
