using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _maxRunSpeed = 5f;
    [SerializeField] private float _runSpeed = 1f;

    [Tooltip("Minimum jump force that will be applied regardless of how long the spacebar is held")]
    [SerializeField] private float _minJumpForce = 2.5f;

    [Tooltip("Max jump force that can be applied from holding down the jump button")]
    [SerializeField] private float _maxJumpForce = 5f;

    [Tooltip("Multiplier applied to all jumps")]
    [SerializeField] private float _jumpForce = 6f;

    [Tooltip("Higher value = you need to hold the button down for less time to reach max jump force")]
    [SerializeField] private float _jumpForceAccrualRate = 10f;

    [Tooltip("Higher value = slower vertical ascent")]
    [SerializeField] private float _ascendingGravityScale = 7f;

    [Tooltip("Higher value = faster vertical descent")]
    [SerializeField] private float _descendingGravityScale = 15f;
    [SerializeField] private float _maxDescendingVelocity = 10f;

    [Header("Collider Settings")]
    [Tooltip("Higher value = larger jump buffer")]
    [SerializeField] private float _boxCastDistance = 3f;

    [Header("Monobehavior References")]
    [SerializeField] private Animator _animator = default;
    [SerializeField] private SpriteRenderer _spriteRenderer = default;

    [Header("Asset References")]
    [SerializeField] private InputReaderSO _inputReader = default;

    private Vector2 _inputVector;

    private Rigidbody2D _rb;
    private Collider2D _playerCollider;

    // jumping related variables
    private bool _isJumping = false;
    private float _timeWhenJumpStart;
    private bool _rejectJumpStartedMidair = false;

    private void Start()
    {
        _playerCollider = GetComponent<Collider2D>();
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
        SetAnimatorParameters();

        _rb.AddForce(_inputVector * _runSpeed, ForceMode2D.Impulse);

        CheckForGroundCollision();

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
        if(_rb.velocity.y > 0 && _isJumping)
        {
           // Debug.Log("ascending");
            _rb.gravityScale = _ascendingGravityScale;
        }
        else if(_rb.velocity.y < 0 && _isJumping)
        {
           // Debug.Log("descending");
            if(_rb.velocity.y < -_maxDescendingVelocity) { _rb.velocity = new Vector2(_rb.velocity.x, -_maxDescendingVelocity); }
            _rb.gravityScale = _descendingGravityScale;
        }
        else
        {
            //Debug.Log("not jumping");
            _rb.gravityScale = 1f;
        }
    }

    private void SetAnimatorParameters()
    {
        bool isRunning = _rb.velocity.x > .01f || _rb.velocity.x < -.01f;
        _animator.SetBool("isRunning", isRunning);

        _animator.SetBool("isJumping", _isJumping);

        _animator.SetFloat("runSpeed", _rb.velocity.x / _maxRunSpeed);
    }

    private void CheckForGroundCollision()
    {
        // must be falling towards ground for _isJumping to be false
        if(_rb.velocity.y > 0) { return; }

        // positions box cast at bottom center of player collider
        Vector3 colliderCenter = _playerCollider.bounds.center;
        Vector2 colliderCenter2D = new Vector2(colliderCenter.x, colliderCenter.y);

        Vector3 playerColliderSize = _playerCollider.bounds.size;
        Vector2 playerColliderSize2D = new Vector2(playerColliderSize.x, playerColliderSize.y);

        RaycastHit2D raycastHit = Physics2D.BoxCast(colliderCenter2D, playerColliderSize2D, 0f, Vector2.down, _boxCastDistance, LayerMask.GetMask("Ground"));

        if(raycastHit.collider != null)
        {
            _isJumping = false;
        }
        else
        {
            _isJumping = true;
        }
    }

    // --- Event Listeners ---
    private void OnRun(Vector2 movementInput)
    {
        if(movementInput.x > 0) { _spriteRenderer.flipX = false; }
        else if(movementInput.x < 0) { _spriteRenderer.flipX = true; }

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

            if(accruedJumpForce > _maxJumpForce) { accruedJumpForce = _maxJumpForce; }
            Debug.Log("Accrued Jump Force: " + accruedJumpForce);

            _isJumping = true;
            _rb.AddForce(Vector2.up * _jumpForce * Math.Max(accruedJumpForce, _minJumpForce), ForceMode2D.Impulse) ;
        }
    }
}
