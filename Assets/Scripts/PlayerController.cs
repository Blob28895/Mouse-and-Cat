using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _maxRunSpeed = 5f;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _jumpForce = 1f;
    [SerializeField] private float _ascendingGravityScale = 10f;
    [SerializeField] private float _descendingGravityScale = 40f;

    [Header("Asset References")]
    [SerializeField] private InputReaderSO _inputReader = default;

    private Vector2 _inputVector;
    private Rigidbody2D _rb;
    private bool _isJumping;
    private bool _isTouchingGround;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _inputReader.EnableGameplayInput();
    }

    private void OnEnable()
    {
        _inputReader.RunEvent += OnRun;
        _inputReader.JumpEvent += OnJump;
    }

    private void OnDisable()
    {
        _inputReader.RunEvent -= OnRun;
        _inputReader.JumpEvent -= OnJump;
    }

    private void FixedUpdate()
    {
        _rb.AddForce(_inputVector * _moveSpeed, ForceMode2D.Impulse);

        // keeps player from running faster than max run speed
        if(_rb.velocity.x > _maxRunSpeed)
        {
            _rb.velocity = new Vector2(_maxRunSpeed, _rb.velocity.y);
        }
        else if(_rb.velocity.x < -_maxRunSpeed)
        {
            _rb.velocity = new Vector2(-_maxRunSpeed, _rb.velocity.y);
        }

        // adjust gravity scale based on ascending or descending
        if(_rb.velocity.y < 0)
        {
            Debug.Log("Ascending");
            _rb.gravityScale = _ascendingGravityScale;
        }
        else if(_rb.velocity.y < 0)
        {
            Debug.Log("Descending");
            _rb.gravityScale = _descendingGravityScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Touching Ground");
            _isJumping = false;
            _isTouchingGround = true;
        }
    }

    // --- Event Listeners ---
    private void OnRun(Vector2 movementInput)
    {
        _inputVector = movementInput;
    }

    private void OnJump()
    {
        if (!_isJumping && _isTouchingGround)
        {
            Debug.Log("Jump");
            _isJumping = true;
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
        
    }
}
