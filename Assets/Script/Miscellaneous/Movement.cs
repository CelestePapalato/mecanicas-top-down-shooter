using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Direction related")]
    [SerializeField] private bool _directionRelatedToCamera = false;
    [Header("Speed related")]
    [SerializeField] float maxSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float decceleration;
    [Header("Smoothing")]
    [SerializeField]
    [Range(0f, .5f)] float rotationSmoothing;

    private float speedMultiplier = 1f;
    private float currentMaxSpeed;
    private Vector2 input_vector = Vector2.zero;
    private Vector3 currentDirection = Vector3.zero;
    private Rigidbody rb;
    private float _rotationCurrentVelocity;

    private Camera _camera;

    public float SpeedMultiplier { get => speedMultiplier; set => speedMultiplier = value; }

    public float MaxSpeed
    {
        get => maxSpeed * speedMultiplier;
        set
        {
            if (value > 0)
            {
                float diff = maxSpeed / value;
                maxSpeed = value;
                acceleration *= diff;
                decceleration *= diff;
            }
            if (value == 0)
            {
                maxSpeed = 0;
            }
        }
    }
    public float Acceleration
    {
        get => acceleration * speedMultiplier; private set { acceleration = (value > 0) ? value : acceleration; }
    }
    public float Decceleration
    {
        get => decceleration * speedMultiplier; private set { decceleration = (value > 0) ? value : decceleration; }
    }

    public Vector2 Direction
    {
        get => input_vector;

        set
        {
            if (value != Vector2.zero)
            {
                currentDirection.x = value.x;
                currentDirection.z = value.y;
                currentDirection.Normalize();
            }
            input_vector = Vector2.ClampMagnitude(value, 1);
        }

    }

    private void Awake()
    {
        currentMaxSpeed = 0;
        rb = GetComponent<Rigidbody>();
        _camera = Camera.main;
    }

    private void FixedUpdate()
    {
        UpdateCurrentMaxSpeed();
        Move();
    }

    private void UpdateCurrentMaxSpeed()
    {
        float desiredSpeed = input_vector.magnitude * MaxSpeed;
        if (desiredSpeed < currentMaxSpeed)
        {
            currentMaxSpeed -= decceleration * Time.fixedDeltaTime;
        }
        else
        {
            currentMaxSpeed += acceleration * Time.fixedDeltaTime;
        }
        currentMaxSpeed = Mathf.Clamp(currentMaxSpeed, 0f, MaxSpeed);
        Debug.Log(currentMaxSpeed);
    }

    private void Move()
    {
        Vector3 targetSpeed = currentDirection * currentMaxSpeed;
        if (_directionRelatedToCamera)
        {
            targetSpeed = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0) * targetSpeed;
        }
        Vector3 currentSpeed = rb.velocity;
        currentSpeed.y = 0;
        Vector3 dv = targetSpeed - currentSpeed;
        float dt = Time.fixedDeltaTime;

        // a = v/t
        rb.AddForce(dv / dt, ForceMode.Acceleration);

        /*
        if (rb.velocity.magnitude > MaxSpeed)
        {
            Vector3 currentVelocity = rb.velocity;
            currentVelocity.y = 0;
            Vector3 vel_objetivo = currentVelocity.normalized * MaxSpeed;
            Vector3 vel_excedida = currentVelocity - vel_objetivo;
            rb.AddForce(vel_excedida * -1, ForceMode.Acceleration);
        }
        */
    }

    /*
    void ObjectRotation()
    {
        if (input_vector != Vector2.zero)
        {

            float rotacionActual = transform.eulerAngles.y;
            float rotacionObjetivo = Mathf.Atan2(input_vector.x, input_vector.y) * Mathf.Rad2Deg;
            if (_directionRelatedToCamera)
            {
                rotacionObjetivo += _camera.transform.eulerAngles.y;
            }

            float rotacion = Mathf.SmoothDampAngle(rotacionActual, rotacionObjetivo, ref _rotationCurrentVelocity, rotationSmoothing);

            rb.MoveRotation(Quaternion.Euler(0f, rotacion, 0f));
        }
    }
    */
}