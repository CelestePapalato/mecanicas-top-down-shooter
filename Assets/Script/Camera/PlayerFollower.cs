using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFollower : MonoBehaviour
{
    [Header("Seguimiento al jugador")]
    [SerializeField]
    [Range(0f, 0.3f)] float _movementSmoothing;

    GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _player.transform.position, (1f / _movementSmoothing) * Time.deltaTime);      
    }
}