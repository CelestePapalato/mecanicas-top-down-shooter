using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    public static event Action RoundEnd;

    [SerializeField] float spawnRate;

    private int totalEnemyCount;
    private int killsToSpawnSpecial;
    private int spawnedEnemies = 0;
    private int killedEnemies = 0;

    private bool spawnSpecials = false;
    private bool specialsAlreadySpawned = false;

    private Round currentRound;

    private List<Enemy> specialEnemyBuffer = new List<Enemy>();

    private void OnEnable()
    {
        GameManager.NextRound += NextRound;
        Enemy.EnemyDead += EnemyKilled;
    }

    private void OnDisable()
    {
        GameManager.NextRound -= NextRound;
        Enemy.EnemyDead -= EnemyKilled;
    }

    private void NextRound(Round round)
    {
        currentRound = round;
        killedEnemies = 0;
        spawnedEnemies = 0;
        totalEnemyCount = round.TotalEnemyCount;
        killsToSpawnSpecial = round.KillsToSpawnSpecial;
        spawnSpecials = false;
        specialsAlreadySpawned = (killsToSpawnSpecial == 0 || currentRound.SpecialEnemies.Length == 0) ? true : false;
        StartCoroutine(SpawnEnemies());
    }

    private void EnemyKilled(int points)
    {
        if(currentRound == null) { return; }
        killedEnemies++;
        if(killedEnemies >= totalEnemyCount)
        {
            currentRound = null;
            RoundEnd?.Invoke();
        }
        if (spawnSpecials || specialsAlreadySpawned) { return; }
        spawnSpecials = killedEnemies >= killsToSpawnSpecial;
        if (spawnSpecials)
        {
            specialEnemyBuffer = currentRound.SpecialEnemies.ToList();
        }
    }

    IEnumerator SpawnEnemies()
    {
        while(spawnedEnemies < totalEnemyCount)
        {
            yield return new WaitForSeconds(spawnRate);

            SpawnSpecials();

            bool spawningSpecials = spawnedEnemies >= killsToSpawnSpecial && !specialsAlreadySpawned;

            if (spawnedEnemies >= totalEnemyCount)
            {
                break;
            }

            if(!spawningSpecials)
            {
                if (EnemySpawnPoint.SpawnEnemy(PickRandomEnemy()))
                {
                    spawnedEnemies++;
                }
            }
        }
    }

    private void SpawnSpecials()
    {
        if (specialEnemyBuffer.Count == 0) { return; }

        for (int i = 0; i < specialEnemyBuffer.Count; i++)
        {
            if (spawnedEnemies >= totalEnemyCount)
            {
                break;
            }
            if (EnemySpawnPoint.SpawnEnemy(specialEnemyBuffer[i]))
            {
                specialEnemyBuffer.RemoveAt(i);
                spawnedEnemies++;
            }
        }

        specialsAlreadySpawned = specialEnemyBuffer.Count == 0;
    }

    private Enemy PickRandomEnemy()
    {
        if (currentRound.NormalEnemies.Length == 0) { return null; }
        int index = UnityEngine.Random.Range(0, currentRound.NormalEnemies.Length);
        Enemy enemy = currentRound.NormalEnemies[index];
        return enemy;
    }
}
