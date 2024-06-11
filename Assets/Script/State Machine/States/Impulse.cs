using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Impulse : MonoBehaviour
{
    [SerializeField]
    [Range(1f, 15f)] float speedModifier;
    [SerializeField] float speedModifierRate;
    [SerializeField] float speedModifierLength;

    NavMeshAgent agent;

    float originalSpeed;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalSpeed = agent.speed;
        Invoke(nameof(AddImpulse), speedModifierRate);
    }

    private void AddImpulse()
    {
        agent.speed *= speedModifier;
        Invoke(nameof(StopImpulse), speedModifierLength);
    }

    private void StopImpulse()
    {
        agent.speed = originalSpeed;
        Invoke(nameof(AddImpulse), speedModifierRate);
    }
}
