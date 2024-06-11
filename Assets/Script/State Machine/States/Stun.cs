using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stun : State
{
    [SerializeField] float stunTime;
    [SerializeField] State nextState;
    [SerializeField]
    [Range(0f, 1f)] float speedModifier;

    NavMeshAgent agent;

    float originalSpeed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        personaje = GetComponent<StateMachine>();
        if(personaje)
        {
            personaje.OnDamaged += StunStart;
        }
    }

    public override void Entrar(StateMachine personajeActual)
    {
        if (!personaje)
        {
            personaje = personajeActual;
        }
        Invoke(nameof(StunEnd), stunTime);
    }

    private void StunStart()
    {
        if (agent != null)
        {
            originalSpeed = agent.speed;
            agent.speed *= speedModifier;
        }
        personaje.CambiarEstado(this);
    }

    private void StunEnd()
    {
        agent.speed = originalSpeed;
        personaje.CambiarEstado(nextState);
    }
}
