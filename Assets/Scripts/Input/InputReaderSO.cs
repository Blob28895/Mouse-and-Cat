using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReaderSO", menuName = "Input/InputReaderSO")]

public class InputReaderSO : ScriptableObject, GameInput.IGameplayActions, GameInput.IMenusActions
{
    public event UnityAction<Vector2> RunEvent = delegate { };
    public event UnityAction JumpEvent = delegate { };
    public event UnityAction PauseEvent = delegate { };
    public event UnityAction UnpauseEvent = delegate { };

    private GameInput _gameInput;

    private void OnEnable()
	{
		if (_gameInput == null)
		{
			_gameInput = new GameInput();

			_gameInput.Gameplay.SetCallbacks(this);
            _gameInput.Menus.SetCallbacks(this);
		}
    }

    public void EnableGameplayInput()
    {
        _gameInput.Gameplay.Enable();
        _gameInput.Menus.Disable();
    }

    public void EnableMenuInput()
    {
        _gameInput.Gameplay.Disable();
        _gameInput.Menus.Enable();
    }

    // --- Event Listeners ---
    public void OnRun(InputAction.CallbackContext context) { RunEvent.Invoke(context.ReadValue<Vector2>());}
    public void OnJump(InputAction.CallbackContext context) { JumpEvent.Invoke();}
    public void OnPause(InputAction.CallbackContext context) { PauseEvent.Invoke();}
    public void OnUnpause(InputAction.CallbackContext context) { UnpauseEvent.Invoke();}
}
