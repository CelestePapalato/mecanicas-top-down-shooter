using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float timeAlive;

    private void Start()
    {
        Destroy(gameObject, timeAlive);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}