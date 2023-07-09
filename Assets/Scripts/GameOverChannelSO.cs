using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameOverChannel", menuName = "ScriptableObjects/GameOverChannel", order = 1)]
public class GameOverChannelSO : ScriptableObject
{
    public delegate void GameOverDelegate();
    public event GameOverDelegate OnGameOver;

    public void RaiseEvent()
    {
        if (OnGameOver != null)
        {
            OnGameOver();
        }
    }
}
