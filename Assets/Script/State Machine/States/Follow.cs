using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follow : State
{
    [SerializeField] float pathUpdateRate;
    NavMeshAgent agent;
    Rigidbody rb;
     
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    public override void Entrar(StateMachine personajeActual)
    {
        base.Entrar(personajeActual);
        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
        }
        if(!agent)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        rb.isKinematic = true;
        agent.enabled = true;
        StartCoroutine(UpdateNavMeshTarget());
    }

    public override void Salir()
    {
        base.Salir();
        rb.isKinematic = false;
        agent.enabled = false;
        StopAllCoroutines();
    }

    public override void Actualizar()
    {
        base.Actualizar();
    }

    IEnumerator UpdateNavMeshTarget()
    {
        while(isActive)
        {
            yield return new WaitForSeconds(pathUpdateRate);
            agent.destination = Player.Instance.transform.position;
        }
    }
}
