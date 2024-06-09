using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PointerFollower : MonoBehaviour
{
    [SerializeField] float distanceFromCamera = 50f;
    [SerializeField] float deadZone;
    [SerializeField] float maxDistance;
    [SerializeField]
    [Range(0f, 0.3f)] float _movementSmoothing;

    RectTransform pointer;
    Camera cam;

    private void Awake()
    {
        pointer = GameObject.FindGameObjectWithTag("Player Cursor").GetComponent<RectTransform>();
        transform.position = Vector3.zero;
        cam = Camera.main;
        transform.localPosition = Vector3.zero;
    }

    void Update()
    {
        Vector3 target = Vector3.zero;
        Vector3 direction = target;
        direction.x = pointer.anchoredPosition.x;
        direction.z = pointer.anchoredPosition.y;
        if(direction.magnitude > deadZone)
        {
            target = Vector3.ClampMagnitude(direction, maxDistance);
            target = Quaternion.Euler(0f, cam.transform.eulerAngles.y, 0f) * target;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, (1f / _movementSmoothing) * Time.deltaTime);
    }
}
