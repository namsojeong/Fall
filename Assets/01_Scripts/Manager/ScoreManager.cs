using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Text scoreText;

    int score = 0;
    int bestScore = 0;

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
        UpdateScoreUI();
    }
    
    public void UpdateScoreUI()
    {
        scoreText.text = $"Score :{GameManager.Instance.CurScore}";
    }
    
}
