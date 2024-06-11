using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsScreen : MonoBehaviour
{
    [SerializeField] Canvas winCanvas;
    [SerializeField] Canvas loseCanvas;

    private void Awake()
    {
        if (winCanvas) { winCanvas.gameObject.SetActive(false); }
        if (loseCanvas) { loseCanvas.gameObject.SetActive(false); }
    }

    private void OnEnable()
    {
        GameManager.OnWin += OnWin;
        GameManager.OnLost += OnLost;
    }

    private void OnDisable()
    {
        GameManager.OnWin -= OnWin;
        GameManager.OnLost -= OnLost;
    }

    private void OnWin()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        winCanvas.gameObject.SetActive(true);
    }

    private void OnLost()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        loseCanvas.gameObject.SetActive(true);
    }
}
