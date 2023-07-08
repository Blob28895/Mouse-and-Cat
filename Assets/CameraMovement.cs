using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraMovement : MonoBehaviour
{
	#region "Private Variable Declarations
	private GameObject _player;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _targetPosition = Vector3.zero;
	private Bounds _backgroundBounds;
    private Bounds _cameraBounds;
    private Bounds _newCameraBounds;
    private Camera _camera;
    private float _cameraHeight;
    private float _cameraWidth;
    private float _minX;
    private float _maxX;
    private float _minY;
    private float _maxY;
	#endregion

	[Tooltip("Time for the camera to move from one position to the next, lower numbers results in the camera following the player more closely")]
    [SerializeField] private float smoothTime = 5f;

    [Tooltip("Sprite in that the camera will never go outside of the bounds of")]
    [SerializeField] private SpriteRenderer _background;
    
    void Awake()
    {
        _camera = GetComponent<Camera>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _backgroundBounds = _background.bounds;
        
        _cameraHeight = _camera.orthographicSize;
        _cameraWidth = _cameraHeight * _camera.aspect;

        _minX = _backgroundBounds.min.x + _cameraWidth;
        _maxX = _backgroundBounds.max.x - _cameraWidth;

		_minY = _backgroundBounds.min.y + _cameraHeight;
		_maxY = _backgroundBounds.extents.y - _cameraHeight;

        _newCameraBounds = new Bounds();
        _newCameraBounds.SetMinMax(
            new Vector3(_minX, _minY, 0f),
            new Vector3(_maxX, _maxY, 0f)
            );
	}

	void Update()
    {
        _targetPosition = _player.transform.position + new Vector3(0, 0, -10);
        _targetPosition = GetCameraBounds();
        transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _velocity, smoothTime);
    }

    private Vector3 GetCameraBounds()
    {
        return new Vector3(
            Mathf.Clamp(_targetPosition.x, _newCameraBounds.min.x, _newCameraBounds.max.x),
			Mathf.Clamp(_targetPosition.y, _newCameraBounds.min.y, _newCameraBounds.max.y),
            transform.position.z
			);
    }
}
