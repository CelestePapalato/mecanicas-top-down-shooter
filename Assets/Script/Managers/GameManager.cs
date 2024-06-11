using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<int> ScoreUpdate;
    public static event Action<Round> NextRound;

    [SerializeField] float restTime;
    [SerializeField] Round[] rounds;

    private int score;

    private void Awake()
    {
        score = 0;
    }

    private void OnEnable()
    {
        Enemy.EnemyDead += ScoreUp;
    }

    private void OnDisable()
    {
        Enemy.EnemyDead -= ScoreUp;
    }

    private void Start()
    {
        ScoreUpdate.Invoke(score);
    }

    private void ScoreUp(int points)
    {
        points = Mathf.Max(points, 0);
        score += points;
        ScoreUpdate.Invoke(score);
    }
}
