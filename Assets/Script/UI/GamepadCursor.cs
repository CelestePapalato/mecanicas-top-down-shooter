using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadCursor : MonoBehaviour
{
    [SerializeField] float _padding = 35f;
    [Header("Dependencies")]
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] float _cursorSpeed;
    [SerializeField] RectTransform _cursorTransform;
    [SerializeField] Canvas _canvas;

    private RectTransform _canvasRectTransform;
    private Camera _camera;
    private Vector2 _currentPosition;
    private Vector2 _deltaVector = Vector2.zero;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _camera = Camera.main;

        Cursor.visible = false;

        if (_canvas)
        {
            _canvasRectTransform = _canvas.GetComponent<RectTransform>();
        }

        _currentPosition = _cursorTransform.anchoredPosition;
    }

    private void OnPoint(InputValue inputValue)
    {
        Vector2 deltaValue = inputValue.Get<Vector2>();
        deltaValue *= Time.deltaTime;
        _deltaVector = deltaValue;
    }

    private void AnchorCursor(Vector2 position)
    {
        Vector2 anchorPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, position,
        _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _camera, out anchorPosition);
        _cursorTransform.anchoredPosition = anchorPosition;
    }

    private void Update()
    {
        Vector2 newPosition = _currentPosition + _deltaVector;

        newPosition.x = Mathf.Clamp(newPosition.x, _padding, Screen.width - _padding);
        newPosition.y = Mathf.Clamp(newPosition.y, _padding, Screen.height - _padding);

        _currentPosition = newPosition;

        AnchorCursor(newPosition);
    }
}
