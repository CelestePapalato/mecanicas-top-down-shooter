using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : StateMachine
{
    public static event Action<int> EnemyDead;
    public UnityAction<Enemy> OnDead;

    [SerializeField] int points;

    Health healthComponent;
    ItemSpawner itemSpawner;
    Animator animator;
    NavMeshAgent agent;

    float originalSpeed;

    protected override void Awake()
    {
        base.Awake();
        healthComponent = GetComponentInChildren<Health>();
        if (healthComponent)
        {
            healthComponent.NoHealth += Dead;
            healthComponent.Damaged += DamageReceived;
        }
        itemSpawner = GetComponent<ItemSpawner>();
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        originalSpeed = agent.speed;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        float speedBlend = agent.speed / originalSpeed;
        animator?.SetFloat("Speed", speedBlend);
        base.Update();
    }

    private void Dead()
    {
        itemSpawner?.DropItem();
        Destroy(gameObject);
        EnemyDead.Invoke(points);
        //GameManager.instance.AddPoints(points);
    }

    private void DamageReceived(int health, int maxHealth)
    {
        DamageReceived();
    }

    private void OnDestroy()
    {
        OnDead?.Invoke(this);
    }
}