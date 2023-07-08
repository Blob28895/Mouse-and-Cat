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

    [Tooltip("Multiplier applied to all jumps")]
    [SerializeField] private float _jumpForce = 1f;

    [Tooltip("Higher value = you need to hold the button down for less time to reach max jump force")]
    [SerializeField] private float _jumpForceAccrualRate = 1f;

    [Tooltip("Higher value = slower vertical ascent")]
    [SerializeField] private float _ascendingGravityScale = 10f;

    [Tooltip("Higher value = faster vertical descent")]
    [SerializeField] private float _descendingGravityScale = 40f;

    [Header("Asset References")]
    [SerializeField] private InputReaderSO _inputReader = default;

    private Vector2 _inputVector;
    private Rigidbody2D _rb;

    // jumping related variables
    private bool _isJumping;
    private float _timeWhenJumpStart;
    private bool _rejectJumpStartedMidair = false;

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
            _rb.gravityScale = _ascendingGravityScale;
        }
        else if(_rb.velocity.y < 0)
        {
            _rb.gravityScale = _descendingGravityScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isJumping = false;
        }
    }

    // --- Event Listeners ---
    private void OnRun(Vector2 movementInput)
    {
        _inputVector = movementInput;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if(_isJumping) {
            // don't let player start charging jump force while in midair
            _rejectJumpStartedMidair = true;
            return; 
        }

        // if jump button is pressed
        if(context.phase == InputActionPhase.Started)
        {
            _rejectJumpStartedMidair = false;
            _timeWhenJumpStart = Time.fixedTime;
        }

        // if jump button is released
        if(context.phase == InputActionPhase.Canceled)
        {
            if(_rejectJumpStartedMidair) { return; }

            // jump force is based on how long the jump button was held down
            float accruedJumpForce = (Time.fixedTime - _timeWhenJumpStart) * _jumpForceAccrualRate;
            Debug.Log("Accrued Jump Force: " + accruedJumpForce);

            if(accruedJumpForce > _maxJumpForce) { accruedJumpForce = _maxJumpForce; }

            _isJumping = true;
            _rb.AddForce(Vector2.up * _jumpForce * accruedJumpForce, ForceMode2D.Impulse);
        }
    }
}
