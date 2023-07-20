using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameOverChannel", menuName = "ScriptableObjects/GameOverChannel", order = 1)]
public class GameOverChannelSO : ScriptableObject
{
    public event UnityAction GameOverEvent = delegate { };
    public bool isGameOver { get; set; }

    public void RaiseEvent()
    {
        isGameOver = true;
        GameOverEvent?.Invoke();
    }
}
