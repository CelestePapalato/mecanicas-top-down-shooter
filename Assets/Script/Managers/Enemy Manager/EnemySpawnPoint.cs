using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    private static List<EnemySpawnPoint> ActiveSpawnPoints = new List<EnemySpawnPoint>();

    [SerializeField] float spawnTimer;

    private bool canSpawn = true;

    private void OnTriggerEnter(Collider other)
    {
        ActiveSpawnPoints.Remove(this);
        canSpawn = false;
    }

    private void OnTriggerExit(Collider other)
    {
        ActiveSpawnPoints.Add(this);
        canSpawn = true;
    }

    private void OnEnable()
    {
        ActiveSpawnPoints.Add(this);
    }

    private void OnDisable()
    {
        ActiveSpawnPoints.Remove(this);
    }

    public static bool SpawnEnemy(Enemy enemy)
    {
        if (!enemy) { return false; }

        EnemySpawnPoint spawnPoint = PickRandomSpawnPoint();

        if(!spawnPoint) { return false; }

        return spawnPoint.Spawn(enemy);
    }

    protected bool Spawn(Enemy enemy)
    {
        if (!enemy || !canSpawn) { return false; }
        Instantiate(enemy, transform.position, Quaternion.identity);
        StartCoroutine(Rest());
        return true;
    }

    private IEnumerator Rest()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnTimer);
        canSpawn = true;
    }

    private static EnemySpawnPoint PickRandomSpawnPoint()
    {
        if(ActiveSpawnPoints.Count == 0) { return null; }
        int index = Random.Range(0, ActiveSpawnPoints.Count);
        EnemySpawnPoint spawnPoint = ActiveSpawnPoints[index];
        return spawnPoint;
    }

    [ContextMenu("Print active spawn points")]
    public void PrintSpawnPoints()
    {
        Debug.Log("Current Spawn Points: ");
        foreach(EnemySpawnPoint spawnPoint in ActiveSpawnPoints)
        {
            Debug.Log(spawnPoint.name + " | Position: " + spawnPoint.transform.position);
        }
    }
}
