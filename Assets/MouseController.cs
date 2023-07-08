using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseController : MonoBehaviour
{
    private Transform _targetPosition;
    private bool _canClimb = true;
    private Animator _animator;
    private Rigidbody2D _rb;

    [SerializeField] private float walkingSpeed = 1f;
    [SerializeField] private float climbingSpeed = 1f;
    [Tooltip("This is all layers that the mouse will define as the ground")]
    [SerializeField] private LayerMask ground;


    void Start()
    {
        _targetPosition = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = (_targetPosition.position - transform.position).normalized;
        determineClimability();
        walk(direction);
    }
    
    private void walk(Vector3 dir)
    {
        //Debug.Log("Walk");
        //Debug.Log(dir);
		_animator.SetTrigger("walk");
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
        if( !_canClimb ) { return; }
        _animator.SetTrigger("climb");

        //Debug.Log("climb");

    }

    private void determineClimability()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 15f, ground);
        //Debug.DrawRay(transform.position, transform.up * 15f, Color.cyan, 1.5f);
        if ( hit.collider == null ) { _canClimb=false; return; }
        else { _canClimb = true; return;}
    }

	private void OnTriggerStay2D(Collider2D collision)
	{
		Debug.Log(collision.gameObject.name);
	}
}
