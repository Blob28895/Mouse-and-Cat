using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject _player;

    private Vector3 _velocity = Vector3.zero;
    [Tooltip("Time for the camera to move from one position to the next, lower numbers results in the camera following the player more closely")]
    [SerializeField] private float smoothTime = 5f;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Vector3 targetPosition = _player.transform.position + new Vector3(0, 0, -10);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
    }
}
