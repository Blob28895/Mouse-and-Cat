using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _maxRunSpeed = 5f;
    [SerializeField] private float _runSpeed = 1f;

    [Tooltip("Max jump force that can be applied from holding down the jump button")]
    [SerializeField] private float _maxJumpForce = 10f;

    [SerializeField] private float _jumpForce = 1f;

    [Tooltip("How fast the jump force increases while holding the jump button")]
    [SerializeField] private float _jumpForceAccrualRate = 1f;

    [SerializeField] private float _ascendingGravityScale = 10f;
    [SerializeField] private float _descendingGravityScale = 40f;

    [Header("Asset References")]
    [SerializeField] private InputReaderSO _inputReader = default;

    private Vector2 _inputVector;
    private Rigidbody2D _rb;

    // jumping related variables
    private bool _isJumping;
    private bool _isTouchingGround;
    private float _timeWhenJumpStart;

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
        _rb.AddForce(_inputVector * _runSpeed, ForceMode2D.Impulse);

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

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!_isJumping && _isTouchingGround)
        {
            if(context.phase == InputActionPhase.Started)
            {
                _timeWhenJumpStart = Time.fixedTime;
            }

            // if jump button is released
            if(context.phase == InputActionPhase.Canceled)
            {
                float accruedJumpForce = (Time.fixedTime - _timeWhenJumpStart) * _jumpForceAccrualRate;
                Debug.Log("Accrued Jump Force: " + accruedJumpForce);

                if(accruedJumpForce > _maxJumpForce)
                {
                    accruedJumpForce = _maxJumpForce;
                }

                _isJumping = true;
                _rb.AddForce(Vector2.up * _jumpForce * accruedJumpForce, ForceMode2D.Impulse);
            }
        }
    }
}
