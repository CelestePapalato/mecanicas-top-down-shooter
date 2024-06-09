using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable, IHittable
{
    [SerializeField] int maxHealth;
    [SerializeField] float invincibilityTime;
    public UnityAction<int, int> HealthUpdate;
    public UnityAction NoHealth;
    public UnityAction<int, int> Damaged;
    public UnityAction<int, int> Healed;

    int health;
    bool invincibility = false;
    Collider col;
    Rigidbody rb;

    private void Awake()
    {
        health = maxHealth;
        col = GetComponent<Collider>();
        rb = GetComponentInParent<Rigidbody>();
    }

    private void Start()
    {
        if (HealthUpdate != null)
        {
            HealthUpdate(health, maxHealth);
        }
    }

    public void Heal(int healPoints)
    {
        health = Mathf.Clamp(health + healPoints, 0, maxHealth);
        if(HealthUpdate != null)
        {
            HealthUpdate(health, maxHealth);
        }
        if(Healed != null)
        {
            Healed(health, maxHealth);
        }
    }

    public void Damage(IDamageDealer damageDealer)
    {
        if(invincibility)
        {
            return;
        }
        health = Mathf.Clamp(health - damageDealer.DamagePoints, 0, maxHealth);
        if (HealthUpdate != null)
        {
            HealthUpdate(health, maxHealth);
        }
        if(Damaged != null)
        {
            Damaged.Invoke(health, maxHealth);
        }
        if (health <= 0 && NoHealth != null)
        {
            col.enabled = false;
            NoHealth();
            return;
        }
        StartCoroutine(invincibilityEnabler());
    }

    public void Hit(IDamageDealer damageDealer)
    {
        Vector3 position = transform.position;
        Vector3 impulseVector = position - damageDealer.Position;
        impulseVector.Normalize();
        rb.AddForce(impulseVector * damageDealer.Impulse, ForceMode.Impulse);
    }

    IEnumerator invincibilityEnabler()
    {
        invincibility = true;
        col.enabled = false;
        yield return new WaitForSeconds(invincibilityTime);
        invincibility = false;
        col.enabled = true;
    }

    public void UpdateInvincibility(bool value)
    {
        StopCoroutine(invincibilityEnabler());
        invincibility = value;
        col.enabled = !value;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
