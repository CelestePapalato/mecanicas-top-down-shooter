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
    float originalAngularSpeed;

    bool addedSpeed = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalSpeed = agent.speed;
        originalAngularSpeed = agent.angularSpeed;
        Invoke(nameof(AddImpulse), speedModifierRate);
    }

    private void AddImpulse()
    {
        if(addedSpeed)
        {
            CancelInvoke(nameof(StopImpulse));
            StopImpulse();
        }
        agent.speed *= speedModifier;
        agent.angularSpeed *= speedModifierRate;
        addedSpeed = true;
        Invoke(nameof(StopImpulse), speedModifierLength);
    }

    private void StopImpulse()
    {
        if(addedSpeed)
        {
            agent.speed /= speedModifier;
            agent.angularSpeed /= speedModifierRate;
            addedSpeed = false;
        }
        Invoke(nameof(AddImpulse), speedModifierRate);
    }
}
