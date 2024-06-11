using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Impulse : State
{
    [SerializeField]
    [Range(1f, 15f)] float speedModifier;
    [SerializeField] float speedModifierRate;
    [SerializeField] float speedModifierLength;

    NavMeshAgent agent;

    float agentSpeed;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        personaje = GetComponent<StateMachine>();
        Invoke(nameof(AddImpulse), speedModifierRate);
    }

    public override void Salir()
    {
        base.Salir();
        agent.speed = agentSpeed;
    }

    private void AddImpulse()
    {
        personaje?.CambiarEstado(this);
        agentSpeed = agent.speed;
        agent.speed *= speedModifier;
        Invoke(nameof(StopImpulse), speedModifierLength);
    }

    private void StopImpulse()
    {
        personaje?.CambiarEstado(null);
        Invoke(nameof(AddImpulse), speedModifierRate);
    }
}
