using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ChasePlayer : State
{
    [SerializeField] float pathUpdateRate;
    NavMeshAgent agent;
    Rigidbody rb;

    float originalSpeed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        originalSpeed = agent.speed;
    }

    public override void Entrar(StateMachine personajeActual)
    {
        base.Entrar(personajeActual);
        if (rb)
        {
            rb.isKinematic = true;
        }
        if (!agent) { agent = GetComponent<NavMeshAgent>(); }
        agent.enabled = true;
        agent.speed = originalSpeed;
        StartCoroutine(UpdateNavMeshTarget());
    }

    public override void Salir()
    {
        base.Salir();
        if (rb)
        {
            rb.isKinematic = false;
        }
        StopAllCoroutines();
    }

    IEnumerator UpdateNavMeshTarget()
    {
        Player player = Player.Instance;
        while(isActive && player)
        {
            yield return new WaitForSeconds(pathUpdateRate);
            agent.destination = player.transform.position;
        }
    }
}
