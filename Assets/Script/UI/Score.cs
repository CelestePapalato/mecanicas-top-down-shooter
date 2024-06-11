using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void UpdateScoreText(int score)
    {
        text.text = score.ToString();
    }

    private void OnEnable()
    {
        GameManager.ScoreUpdate += UpdateScoreText;
    }

    private void OnDisable()
    {
        GameManager.ScoreUpdate -= UpdateScoreText;
    }
}
