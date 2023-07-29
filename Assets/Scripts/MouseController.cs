using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class MouseController : MonoBehaviour
{
    private Transform _targetPosition;
    private bool _canClimb = true;
    private bool _isClimbing = false;
    private Animator _animator;
    private Rigidbody2D _rb;

    [Header("Damage Settings")]
    [SerializeField] private float _damageFrequency = 1f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private GameObject _damageEffect = default;

    [Header("Layer Settings")]

	[Tooltip("This is all layers that the mouse will define as the ground")]
	[SerializeField] private LayerMask ground;

	[Header("Different Colliders")]
	[SerializeField] private Collider2D _walkingCollider;
	[SerializeField] private Collider2D _ClimbingCollider;

    [Header("Speeds")]
    [SerializeField] private float walkingSpeed = 1f;
    [SerializeField] private float climbingSpeed = 1f;

    [Header("Audio")]
    [SerializeField] private AudioSource _squeak;
    [Tooltip("Object that will spawn to play the mouse death noise when a mouse dies. Since it cant be playing a sound while also destroying itself")]
    [SerializeField] private GameObject _deathSoundObject;


    void Start()
    {
        _targetPosition = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _squeak = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = (_targetPosition.position - transform.position);//.normalized;
        //Debug.Log(direction);
        determineClimability();
        if ((_canClimb && Math.Abs(direction.x) < Math.Abs(direction.y)) || _isClimbing)
        { //if the vertical distance is greater than the horizontal distance try and climb
            //Debug.Log("Climb Call");
            climb();
        }
        else
        { //otherwise walk
            if(_isClimbing) { /*Debug.Log("Im stopping climbing to start walking");*/ stopClimbing(); }
            walk(direction);
            //Debug.Log("walk call");
        }
    }
    
    private void walk(Vector3 dir)
    {
        //Debug.Log("Walk");
        //Debug.Log(dir);
		if (Math.Abs(dir.x) < 0.05f) { return; }
        if(dir.x < 0f)
        {
			dir.x = -1;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
			dir.x = 1;
			GetComponent<SpriteRenderer>().flipX = false;
		}
        
        _rb.MovePosition(_rb.position + new Vector2(dir.x, 0) * walkingSpeed * Time.deltaTime);
    }

    private void climb()
    {
        if( !_canClimb ) { /*Debug.Log("I cant Climb anymore so I will stop")*/; stopClimbing();  return; }
        if (!_isClimbing) { startClimbing(); }
        if(_rb.position.y > _targetPosition.position.y)
        {
            stopClimbing() ; return;
        }
        _rb.MovePosition( _rb.position + new Vector2(0, 1) * climbingSpeed * Time.deltaTime);
    }

    private void startClimbing()
    {
        //Debug.Log("Starting Climbing");
        if(_isClimbing ) { return; }
        _animator.SetTrigger("climb");
        _isClimbing = true;
        _rb.gravityScale = 0f;
        _ClimbingCollider.enabled = true;
        _walkingCollider.enabled = false;
    }
    private void stopClimbing()
    {
		//Debug.Log("Stopping Climbing");
		if (!_isClimbing ) { return; }
        _animator.SetTrigger("walk");
        _isClimbing = false;
        _rb.gravityScale = 10f;
		_ClimbingCollider.enabled = false;
		_walkingCollider.enabled = true;
	}

    private void determineClimability()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, GetComponent<SpriteRenderer>().bounds.min.y)/*transform.position*/, transform.up, 15f, ground);
        Debug.DrawRay(new Vector2(transform.position.x, GetComponent<SpriteRenderer>().bounds.min.y), transform.up * 15f, Color.cyan, 0.01f);
        if ( hit.collider == null ) { _canClimb=false; return; }
        else { _canClimb = true; return;}
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(AttackPlayer(other, _damage));
        }
    }

    private IEnumerator AttackPlayer(Collider2D playerCollider, int damage)
    {
        _damageEffect.SetActive(true);

        HealthSO playerHealth = playerCollider.GetComponent<PlayerController>().health;
        playerHealth.Damage(_damage);
        _squeak.Play();
        
        yield return new WaitForSeconds(_damageFrequency);

        _damageEffect.SetActive(false);
        StartCoroutine(playerHealth.AttemptToRegen());
    }

    public void Die()
    {
        Instantiate(_deathSoundObject);
        Destroy(gameObject);
    }
}
