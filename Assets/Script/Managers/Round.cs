using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Round Data", menuName = "ScriptableObjects/Round Data", order = 1)]
public class Round : ScriptableObject
{
    public static event Action NoEnemiesLeft;

    [SerializeField] int totalEnemyCount;
    [SerializeField] int killsToSpawnSpecial;
    [SerializeField] Enemy[] normalEnemies;
    [SerializeField] Enemy[] specialEnemies;

    public int TotalEnemyCount { get { return totalEnemyCount; } }

    public int KillsToSpawnSpecial { get { return killsToSpawnSpecial; } }

    public Enemy[] NormalEnemies { get {  return ArrayCopy(normalEnemies); } }

    public Enemy[] SpecialEnemies { get { return ArrayCopy(specialEnemies); } }

    private Enemy[] ArrayCopy(Enemy[] array)
    {
        Enemy[] copy = new Enemy[array.Length];

        for(int i = 0; i < array.Length; i++)
        {
            copy[i] = array[i];
        }
        return copy;
    }
}
