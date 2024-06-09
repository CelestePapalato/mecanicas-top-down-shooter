using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp Data", menuName = "ScriptableObjects/PowerUp Data", order = 2)]

public class PowerUp : ScriptableObject, IBuff
{
    [SerializeField] int _healPoints;
    [SerializeField] float _speedMultiplier;
    [SerializeField] float _damageMultiplier;
    [SerializeField] float _buffTime;

    public void Buff(object o)
    {
        Health healthComponent = o as Health;
        if (healthComponent)
        {
            healthComponent.Heal(_healPoints);
        }
        Player player = o as Player;
        if (player)
        {
            //player.SpeedPowerUp(_speedMultiplier, _buffTime);
            //player.DamagePowerUp(_damageMultiplier, _buffTime);
        }
    }
}
