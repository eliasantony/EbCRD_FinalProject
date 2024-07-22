using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    public PlayerInput.OnFootActions _onFootActions;
    
    private PlayerMotor _playerMotor;
    private PlayerLook _playerLook;
    
    void Awake()
    {
        _playerInput = new PlayerInput();
        _onFootActions = _playerInput.OnFoot;
        _playerMotor = GetComponent<PlayerMotor>();
        _playerLook = GetComponent<PlayerLook>();
        
        _onFootActions.Jump.performed += ctx => _playerMotor.Jump();
        _onFootActions.Sprint.performed += ctx => _playerMotor.Sprint();
    }

    private void LateUpdate()
    {
        _playerLook.ProcessLook(_onFootActions.Look.ReadValue<Vector2>());
    }
    
    void FixedUpdate()
    {
        _playerMotor.ProcessMove(_onFootActions.Movement.ReadValue<Vector2>());
    }
    
    private void OnEnable()
    {
        _onFootActions.Enable();
    }
    
    private void OnDisable()
    {
        _onFootActions.Disable();
    }
}
