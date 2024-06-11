using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OnDamage : State
{
    [SerializeField] float stunTime;
    [SerializeField] State nextState;

    Rigidbody rb;
    NavMeshAgent agent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        if(rb != null)
        {
            rb.isKinematic = false;
        }
        if(agent != null)
        {
            agent.enabled = false;
        }
        Invoke(nameof(StunEnd), stunTime);
    }

    private void StunStart()
    {
        personaje.CambiarEstado(this);
    }

    private void StunEnd()
    {
        personaje.CambiarEstado(nextState);
    }
}
