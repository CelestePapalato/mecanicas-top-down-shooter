using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IBuffable
{
    private Health health;
    private Weapon weapon;
    private Movement movement;

    private void Awake()
    {
        weapon = GetComponentInChildren<Weapon>();
        health = GetComponentInChildren<Health>();
        movement = GetComponent<Movement>();
    }

    public void Accept(IBuff buff)
    {
        buff.Buff(this);
        buff.Buff(weapon);
        buff.Buff(health);
    }

    private void OnMove(InputValue inputValue)
    {
        Vector2 input_vector = inputValue.Get<Vector2>();
        movement.Direction = input_vector;
    }
}
