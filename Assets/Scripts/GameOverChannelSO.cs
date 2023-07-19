using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameOverChannel", menuName = "ScriptableObjects/GameOverChannel", order = 1)]
public class GameOverChannelSO : ScriptableObject
{
    public event UnityAction GameOverEvent = delegate { };
    public bool isGameOver { get; private set; }

    public void RaiseEvent()
    {
        isGameOver = true;
        GameOverEvent?.Invoke();
    }

    private void OnDisable()
    {
        isGameOver = false;
    }

    private void OnEnable()
    {
        isGameOver = false;
    }
}
