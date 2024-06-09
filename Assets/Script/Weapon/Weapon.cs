using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Antlr3.Runtime.Tree.TreeWizard;

public class Weapon : MonoBehaviour, IBuffable
{
    [SerializeField] Projectile _projectilePrefab;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] private float _fireRate = 0.25f;
    private float _currentFireRateMultiplier = 1;
    private float _currentDamageMultiplier = 1;
    private float FireRate
    {
        get { return _fireRate / _currentFireRateMultiplier; }
    }

    [SerializeField]
    private float shootStrength;
    [SerializeField] ParticleSystem _particleSystem;

    private bool _canShoot = true;

    public void Accept(IBuff buff)
    {
        buff.Buff(this);
    }

    private void Awake()
    {
        if (_particleSystem)
        {
            _particleSystem.Stop();
        }
    }

    void OnShoot()
    {
        if (!_canShoot)
        {
            return;
        }
        StartCoroutine(ControlFireRate());
        Quaternion rotation = _spawnPoint.rotation;
        Projectile projectile = Instantiate(_projectilePrefab, _spawnPoint.position, rotation);
        Damage projectileDamage = projectile.GetComponent<Damage>();
        projectileDamage.DamageMultiplier = _currentDamageMultiplier;
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * shootStrength, ForceMode.Impulse);
    }

    private IEnumerator ControlFireRate()
    {
        _canShoot = false;
        yield return new WaitForSeconds(FireRate);
        _canShoot = true;
    }

    public void FireRateBonus(float fireRateBonus, float bonusTimeLength)
    {
        StopCoroutine(ControlFireRate());
        StopCoroutine(nameof(FireRateBonusTimer));
        StartCoroutine(FireRateBonusTimer(fireRateBonus, bonusTimeLength));
    }

    private IEnumerator FireRateBonusTimer(float fireRateBonus, float timeLength)
    {
        if (_particleSystem)
        {
            _particleSystem.Play();
        }
        _currentFireRateMultiplier = fireRateBonus;
        _canShoot = true;
        StopCoroutine(ControlFireRate());
        yield return new WaitForSeconds(timeLength);
        _currentFireRateMultiplier = 1;
        if (_particleSystem)
        {
            _particleSystem.Stop();
        }
    }

    public void DamageBonus(float damageBonus, float bonusTimeLength)
    {
        StopCoroutine(nameof(DamageBonusTimer));
        StartCoroutine(DamageBonusTimer(damageBonus, bonusTimeLength));
    }

    private IEnumerator DamageBonusTimer(float damageBonus, float bonusTimeLength)
    {
        if(_currentDamageMultiplier < damageBonus)
        {
            _currentDamageMultiplier = damageBonus;
        }
        yield return new WaitForSeconds(bonusTimeLength);
        _currentDamageMultiplier = 1;
    }
}