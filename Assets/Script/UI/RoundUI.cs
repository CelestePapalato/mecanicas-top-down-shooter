using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundUI : MonoBehaviour
{
    [SerializeField] TMP_Text textComponent;
    [SerializeField] string restText;

    int totalEnemyCount;
    int enemiesKilled;

    float restTimer;

    bool resting = false;

    private void OnEnable()
    {
        GameManager.OnRest += RestTime;
        GameManager.OnNextRound += RoundStarted;
        Enemy.EnemyDead += EnemyEliminated;
    }

    private void OnDisable()
    {
        GameManager.OnRest -= RestTime;
        GameManager.OnNextRound -= RoundStarted;
        Enemy.EnemyDead -= EnemyEliminated;
    }

    private void RestTime(float restTime)
    {
        resting = true;
        restTimer = restTime;
        StartCoroutine(UpdateRestTimer());
    }

    private void RoundStarted(Round round)
    {
        resting = false;
        enemiesKilled = 0;
        totalEnemyCount = round.TotalEnemyCount;
        UpdateText();
    }

    private void EnemyEliminated(int points)
    {
        enemiesKilled++;
        UpdateText();
    }

    private void UpdateText()
    {
        if (resting)
        {
            textComponent.text = restText + "\n" + (int) restTimer + "s";
            return;
        }
        textComponent.text = enemiesKilled + " / " + totalEnemyCount;
    }

    IEnumerator UpdateRestTimer()
    {
        while(restTimer > 0)
        {
            UpdateText();
            yield return null;
            restTimer -= Time.deltaTime;
        }
    }
}
