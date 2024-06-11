using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] float spawnRate;

    private int totalEnemyCount;
    private int killsToSpawnSpecial;
    private int spawnedEnemies = 0;
    private int killedEnemies = 0;

    private bool spawnSpecials = false;

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
        spawnedEnemies = 0;
        totalEnemyCount = round.TotalEnemyCount;
        killsToSpawnSpecial = round.KillsToSpawnSpecial;
        spawnSpecials = false;
        StartCoroutine(SpawnEnemies());
    }

    private void EnemyKilled(int points)
    {
        killedEnemies++;
        if (spawnSpecials) { return; }
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

            if (spawnedEnemies >= totalEnemyCount)
            {
                break;
            }

            if (EnemySpawnPoint.SpawnEnemy(PickRandomEnemy()))
            {
                spawnedEnemies++;
            }

            Debug.Log(spawnedEnemies);
        }
    }

    private void SpawnSpecials()
    {
        if (specialEnemyBuffer.Count == 0) { return; }

        for (int i = 0; i < specialEnemyBuffer.Count; i++)
        {
            if (EnemySpawnPoint.SpawnEnemy(specialEnemyBuffer[i]))
            {
                specialEnemyBuffer.RemoveAt(i);
                spawnedEnemies++;
            }
        }
    }

    private Enemy PickRandomEnemy()
    {
        if (currentRound.NormalEnemies.Length == 0) { return null; }
        int index = Random.Range(0, currentRound.NormalEnemies.Length);
        Enemy enemy = currentRound.NormalEnemies[index];
        return enemy;
    }
}
