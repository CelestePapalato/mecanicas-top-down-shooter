using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PointerFollower : MonoBehaviour
{
    [SerializeField] float maxDistance;
    [SerializeField]
    [Range(0f, 0.3f)] float _movementSmoothing;

    PlayerCursor pointer;
    Camera cam;
    Transform player;

    private void Awake()
    {
        pointer = GameObject.FindGameObjectWithTag("Player Cursor").GetComponent<PlayerCursor>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = Vector3.zero;
        cam = Camera.main;
        transform.localPosition = Vector3.zero;
    }

    void Update()
    {
        Vector3 direction = pointer.GetWorldPoint();
        direction.y = player.position.y;
        direction -= player.position;
        Vector3 target = Vector3.ClampMagnitude(direction, maxDistance);
        
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, (1f / _movementSmoothing) * Time.deltaTime);
    }

}
