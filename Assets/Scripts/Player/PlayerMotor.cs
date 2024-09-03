using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController _characterController;
    private Vector3 _playerVelocity;
    private bool _isGrounded;
    private GameObject _gunHolder;
    private bool _weaponEquipped;
    
    public float aimSpeed = 2.5f;
    public float speed = 5.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    
    public PickUpController _pickUpController;
    
    
    private bool _canSprint = true;
    private bool _sprinting = false;
    
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _gunHolder = GameObject.FindGameObjectWithTag("GunHolder");
    }

    void Update()
    {
        _isGrounded = _characterController.isGrounded;
        _weaponEquipped = _gunHolder.transform.childCount > 0;
        HandleAiming();
        ProcessStamina();
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        _characterController.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        if(_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -2f;
        }
        _playerVelocity.y += gravity * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);
    }
    private void HandleAiming()
    {
        if (_weaponEquipped && Input.GetKey(KeyCode.Mouse1))
        {
            speed = aimSpeed;
        }
        else
        {
            speed = _sprinting ? 10.0f : 5.0f;
        }
    }
    public void Jump()
    {
        if(_isGrounded)
        {
            _playerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }
    

    private void ProcessStamina()
    {
        if (_canSprint && Input.GetKey(KeyCode.LeftShift))
        {
            Sprint();
        }
        else if (_sprinting)
        {
            StopSprinting();
        }
    }

    public bool IsSprinting()
    {
        return _sprinting && _canSprint;
    }

    public bool IsMoving()
    {
        return _characterController.velocity.magnitude > 0.1f; // Check if the player is moving
    }

    public void SetCanSprint(bool value)
    {
        _canSprint = value;
        if (!_canSprint)
        {
            StopSprinting(); // Stop sprinting if we can no longer sprint
        }
    }

    public void Sprint()
    {
        if (!_sprinting && _canSprint)
        {
            _sprinting = true;
            speed = 10.0f;
        }
    }

    public void StopSprinting()
    {
        _sprinting = false;
        speed = 5.0f;
    }
}
