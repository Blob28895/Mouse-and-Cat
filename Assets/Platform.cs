using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Platform : MonoBehaviour
{

    private PlatformEffector2D _effector;
	[SerializeField] private InputReaderSO _inputReader = default;
	// Start is called before the first frame update
	void Start()
    {
        _effector = GetComponent<PlatformEffector2D>();
		_inputReader.EnableGameplayInput();
	}

	private void OnEnable()
	{
        _inputReader.JumpEvent += jumpSetOffset;
	}
	private void OnDisable()
	{
        _inputReader.JumpEvent -= jumpSetOffset;

	}

	// Update is called once per frame
	void Update()
    {
        if(Input.GetAxisRaw("Vertical") > 0)
        {
            _effector.rotationalOffset = 0;
        }
        else if(Input.GetAxisRaw("Vertical") < 0)
        {
            _effector.rotationalOffset = 180;
        }
    }

    private void jumpSetOffset(InputAction.CallbackContext context)
    {
        _effector.rotationalOffset = 0;
    }
}
