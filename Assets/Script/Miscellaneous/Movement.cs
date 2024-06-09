using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Direction related")]
    [SerializeField] private bool _directionRelatedToCamera = false;
    [Header("Speed related")]
    [SerializeField] float _maxSpeed;
    [SerializeField] float _acceleration;
    [SerializeField] float _decceleration;
    [Header("Smoothing")]
    [SerializeField]
    [Range(0f, .5f)] float rotationSmoothing;

    private float _currentMaxSpeed;

    private Vector2 _inputVector = Vector3.zero;
    public Vector2 InputVector
    {
        get { return _inputVector; }
        private set { _inputVector = value; }
    }

    private Rigidbody rb;
    private float _rotationCurrentVelocity;

    private Camera _camera;



    private void Awake()
    {
        _currentMaxSpeed = 0;
        rb = GetComponent<Rigidbody>();
        if(_directionRelatedToCamera)
        {
            _camera = Camera.main;
        }
    }

    private void FixedUpdate()
    {
        UpdateCurrentMaxSpeed();
        ObjectRotation();
        Move();
    }

    private void UpdateCurrentMaxSpeed()
    {
        float desiredSpeed = _inputVector.magnitude * _maxSpeed;
        if(desiredSpeed < _currentMaxSpeed)
        {
            _currentMaxSpeed -= _decceleration * Time.fixedDeltaTime;
        }
        else
        {
            _currentMaxSpeed += _acceleration * Time.fixedDeltaTime;
        }
        _currentMaxSpeed = Mathf.Clamp(_currentMaxSpeed, 0f, _maxSpeed);
    }

    private void Move()
    {
        Vector3 targetSpeed = transform.forward * _currentMaxSpeed;
        Vector3 currentSpeed = rb.velocity;
        currentSpeed.y = 0;
        Vector3 dv = targetSpeed - currentSpeed;
        float dt = Time.fixedDeltaTime;

        // a = v/t
        rb.AddForce(dv / dt, ForceMode.Acceleration);

        if (rb.velocity.magnitude > _maxSpeed)
        {
            Vector3 currentVelocity = rb.velocity;
            currentVelocity.y = 0;
            Vector3 vel_objetivo = currentVelocity.normalized * _maxSpeed;
            Vector3 vel_excedida = currentVelocity - vel_objetivo;
            rb.AddForce(vel_excedida * -1, ForceMode.Acceleration);
        }
    }

    void ObjectRotation()
    {
        if (_inputVector != Vector2.zero)
        {

            float rotacionActual = transform.eulerAngles.y;
            float rotacionObjetivo = Mathf.Atan2(_inputVector.x, _inputVector.y) * Mathf.Rad2Deg;
            if (_directionRelatedToCamera)
            {
                rotacionObjetivo += _camera.transform.eulerAngles.y;
            }

            float rotacion = Mathf.SmoothDampAngle(rotacionActual, rotacionObjetivo, ref _rotationCurrentVelocity, rotationSmoothing);

            rb.MoveRotation(Quaternion.Euler(0f, rotacion, 0f));
        }
    }

    private bool MathApproximately(float n1, float n2, float tolerance)
    {
        return (Mathf.Abs(n2 - n1) < tolerance);
    }

    private void OnMove(InputValue inputValue)
    {
        Vector2 input_vector = inputValue.Get<Vector2>();
        _inputVector = Vector2.ClampMagnitude(input_vector, 1);
    }
}