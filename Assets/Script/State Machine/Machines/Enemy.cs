using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : StateMachine
{
    public static event Action<int> EnemyDead;

    [SerializeField] int points;
    public UnityAction<Enemy> OnDead;
    Health healthComponent;
    ItemSpawner itemSpawner;

    protected override void Awake()
    {
        base.Awake();
        healthComponent = GetComponentInChildren<Health>();
        if (healthComponent)
        {
            healthComponent.NoHealth += Dead;
        }
        itemSpawner = GetComponent<ItemSpawner>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void Dead()
    {
        itemSpawner?.DropItem();
        Destroy(gameObject);
        EnemyDead.Invoke(points);
        //GameManager.instance.AddPoints(points);
    }

    private void OnDestroy()
    {
        OnDead?.Invoke(this);
    }
}