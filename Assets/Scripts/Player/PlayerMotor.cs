using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController _characterController;
    private Vector3 _playerVelocity;
    private bool _isGrounded;
    
    public float speed = 5.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    
    private bool _sprinting = false;
    
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        _isGrounded = _characterController.isGrounded;
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
    
    public void Jump()
    {
        if(_isGrounded)
        {
            _playerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }
    
    public void Sprint()
    {
        _sprinting = !_sprinting;
        if (_sprinting)
        {
            speed = 10.0f;
        }
        else
        {
            speed = 5.0f;
        }
    }
}
