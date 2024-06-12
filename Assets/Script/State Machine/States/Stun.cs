using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stun : MonoBehaviour
{
    [SerializeField] float stunTime;
    [SerializeField] State nextState;
    [SerializeField]
    [Range(0f, 1f)] float speedModifier;

    NavMeshAgent agent;
    Health health;

    float originalSpeed;

    bool stunApplied = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        originalSpeed = agent.speed;
        health = GetComponent<Health>();
    }

    private void StunStart(int health, int maxHealth)
    {
        if (stunApplied)
        {
            CancelInvoke(nameof(StunEnd));
            StunEnd();
        }
        agent.speed *= speedModifier;
        stunApplied = true;
        Invoke(nameof(StunEnd), stunTime);
    }

    private void StunEnd()
    {
        agent.speed /= speedModifier;
        stunApplied = false;
    }

    private void OnEnable()
    {
        if (health)
        {
            health.Damaged += StunStart;
        }
    }

    private void OnDisable()
    {
        if (health)
        {
            health.Damaged -= StunStart;
        }
    }
}
