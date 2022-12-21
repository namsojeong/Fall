using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance => instance;
    private static ScoreManager instance;

    [SerializeField] Text scoreText;

    int score = 0;
    int bestScore = 0;

    private void Awake()
    {
        instance = this;
        if(instance==null)
        {
            instance = GetComponent<ScoreManager>();
        }

        ResetScore();
    }

    private void ResetScore()
    {
        GetScore();
        score = 0;
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
