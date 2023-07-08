using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    private Transform _targetPosition;

    [SerializeField] private float walkingSpeed = 1f;
    [SerializeField] private float climbingSpeed = 1f;


    void Start()
    {
        _targetPosition = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Fixedupdate()
    {
        
    }

	private void OnTriggerStay2D(Collider2D collision)
	{
		Debug.Log(collision.gameObject.name);
	}
}
