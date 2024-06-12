using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField]
    TMP_Text scoreText;
    [SerializeField]
    TMP_Text bestScoreText;

    private void UpdateScore(int score)
    {
        if (!scoreText)
        {
            Debug.LogWarning("No se ha referenciado un componente Text para el puntaje.");
            return;
        }
        scoreText.text = score.ToString();
    }

    private void UpdateBestScore(int bestScore)
    {
        if (!bestScoreText)
        {
            Debug.LogWarning("No se ha referenciado un componente Text para el puntaje más alto.");
            return;
        }
        bestScoreText.text = bestScore.ToString();
    }

    private void OnEnable()
    {
        GameManager.OnScoreUpdate += UpdateScore;
        GameManager.OnBestScoreUpdate += UpdateBestScore;
    }

    private void OnDisable()
    {
        GameManager.OnScoreUpdate -= UpdateScore;
        GameManager.OnBestScoreUpdate -= UpdateBestScore;
    }
}
