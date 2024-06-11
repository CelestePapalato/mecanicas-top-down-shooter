using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public event Action OnDamaged;

    [SerializeField] protected State primerEstado;

    protected State estadoActual;
    protected State primerEstadoBuffer;
    protected State ultimoEstado;

    protected virtual void Awake()
    {
        if (!primerEstado)
        {
            primerEstado = GetComponent<State>();
        }
        if (primerEstado)
        {
            primerEstadoBuffer = primerEstado;
        }
        else
        {
            Debug.LogWarning("El State Machine " + name + "no posee ni encuentra un Estado al que llamar");
        }
    }

    protected virtual void Start()
    {
        CambiarEstado(primerEstado);
    }

    protected virtual void Update()
    {
        if (estadoActual)
        {
            estadoActual.Actualizar();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (estadoActual)
        {
            estadoActual.ActualizarFixed();
        }
    }

    public virtual void CambiarEstado(State nuevoEstado)
    {
        estadoActual?.Salir();
        estadoActual = (nuevoEstado) ? nuevoEstado : primerEstado;
        estadoActual?.Entrar(this);
    }

    protected virtual void DamageReceived()
    {
        OnDamaged?.Invoke();
    }

    private void OnEnable()
    {
        if (estadoActual)
        {
            return;
        }
        primerEstado = primerEstadoBuffer;
        CambiarEstado(ultimoEstado);
    }

    private void OnDisable()
    {
        ultimoEstado = estadoActual;
        primerEstadoBuffer = primerEstado;
        primerEstado = null;
        estadoActual?.Salir();
    }
}

public abstract class State : MonoBehaviour
{
    protected StateMachine personaje;
    protected bool isActive = false;

    public virtual void Entrar(StateMachine personajeActual)
    {
        personaje = personajeActual;
        personaje.OnDamaged += DañoRecibido;
        isActive = true;
    }
    public virtual void Salir()
    {
        isActive = false;
    }
    public virtual void Actualizar() { }
    public virtual void ActualizarFixed() { }
    public virtual void DañoRecibido() { }

    private void OnEnable()
    {
        if (personaje)
        {
            personaje.OnDamaged += DañoRecibido;
        }
    }

    private void OnDisable()
    {
        if (!personaje) { return; }
        personaje.OnDamaged -= DañoRecibido;
        personaje.CambiarEstado(null);
        isActive = false;
    }
}