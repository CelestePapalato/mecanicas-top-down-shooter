using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stun : MonoBehaviour
{
    [SerializeField] int damageNeeded;
    [SerializeField] float stunLength;
    [SerializeField]
    [Range(0f, 1f)] float speedModifier;

    NavMeshAgent agent;
    Health health;

    float originalSpeed;

    int lastAccumulatedDamage;

    bool stunApplied = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        originalSpeed = agent.speed;
        health = GetComponentInChildren<Health>();
    }

    private void StunStart(int health, int maxHealth)
    {
        int previousAccumulatedDamage = lastAccumulatedDamage;
        lastAccumulatedDamage = maxHealth - health;
        bool newTreshold = previousAccumulatedDamage < lastAccumulatedDamage;
        if ( lastAccumulatedDamage % damageNeeded != 0 || !newTreshold)
        {
            return;
        }
        if (stunApplied)
        {
            CancelInvoke(nameof(StunEnd));
            StunEnd();
        }
        agent.speed *= speedModifier;
        stunApplied = true;
        Invoke(nameof(StunEnd), stunLength);
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
