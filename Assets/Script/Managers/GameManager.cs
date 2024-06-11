using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<int> ScoreUpdate;
    public static event Action<int> BestScoreUpdate;
    public static event Action<Round> NextRound;
    public static event Action OnWin;
    public static event Action OnLost;

    [SerializeField] float restTime;
    [SerializeField] List<Round> rounds = new List<Round>();

    private int score;
    private static int bestScore = 0;

    public static int BestScore { get => bestScore; }

    private Round currentRound;

    private Player player;

    private void Awake()
    {
        score = 0;
        Time.timeScale = 1;
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
        ScoreUpdate?.Invoke(score);
        BestScoreUpdate?.Invoke(bestScore);
        player = Player.Instance;
        player.OnDead += PlayerDead;
        Rest();
    }

    [ContextMenu("Lose Game")]
    private void PlayerDead()
    {
        player.OnDead -= PlayerDead;
        Time.timeScale = 0;
        OnLost?.Invoke();
    }

    [ContextMenu("Win Game")]
    private void WinGame()
    {
        Time.timeScale = 0;
        OnWin.Invoke();
    }

    private void ScoreUp(int points)
    {
        points = Mathf.Max(points, 0);
        score += points;
        if(score > bestScore)
        {
            bestScore = score;
            BestScoreUpdate?.Invoke(score);
        }
        ScoreUpdate?.Invoke(score);
    }

    [ContextMenu("Next Round")]
    private void StartRound()
    {        
        currentRound = rounds[0];
        rounds.RemoveAt(0);
        NextRound?.Invoke(currentRound);
        Debug.Log("Round started " + currentRound.name);
    }

    private void Rest()
    {
        if (rounds.Count == 0)
        {
            Time.timeScale = 0;
            OnWin.Invoke();
            return;
        }
        StartCoroutine(RestTime());
    }

    private IEnumerator RestTime()
    {
        Debug.Log("Next Round in " + restTime + "s.");
        yield return new WaitForSeconds(restTime);
        StartRound();
    }
}
