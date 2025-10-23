using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Proyect.Inputs;
using System;

public class EndGameController : MonoBehaviour
{
    private BoxCollider _endGameBox;
    private PlayerControls _playerControls;
    private bool _controlsDisposed;

    private void Awake()
    {
        _endGameBox = GetComponent<BoxCollider>();
        _endGameBox.isTrigger = true;
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        if (_playerControls != null)
            _playerControls.Enable();
    }

    private void OnDisable()
    {
        if (_playerControls != null)
            _playerControls.Disable(); // asegura que los action maps (incl. Gameplay) queden desactivados
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("End Game Triggered");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            CleanupControls();
            SceneManager.LoadScene("EndGameScene");
        }
    }

    private void OnDestroy()
    {
        CleanupControls();
    }

    private void CleanupControls()
    {
        if (_playerControls == null || _controlsDisposed) return;

        // Asegurar explícitamente que el mapa Gameplay está deshabilitado (evita la aserción)
        try { _playerControls.Gameplay.Disable(); } catch { /* generated API may vary */ }

        _playerControls.Disable();
        _playerControls.Dispose();
        _controlsDisposed = true;
        _playerControls = null;
    }
}
