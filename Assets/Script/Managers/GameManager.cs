using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<int> ScoreUpdate;
    public static event Action<Round> NextRound;

    [SerializeField] float restTime;
    [SerializeField] List<Round> rounds = new List<Round>();

    private int score;

    private Round currentRound;

    private void Awake()
    {
        score = 0;
    }

    private void OnEnable()
    {
        Enemy.EnemyDead += ScoreUp;
        EnemyManager.RoundEnd += Rest;
    }

    private void OnDisable()
    {
        Enemy.EnemyDead -= ScoreUp;
        EnemyManager.RoundEnd -= Rest;
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

    [ContextMenu("Next Round")]
    private void StartRound()
    {
        if (rounds.Count == 0) { return; }
        currentRound = rounds[0];
        rounds.RemoveAt(0);
        NextRound.Invoke(currentRound);
    }

    private void Rest()
    {
        StartCoroutine(RestTime());
    }

    private IEnumerator RestTime()
    {
        yield return new WaitForSeconds(restTime);
        StartRound();
    }
}
