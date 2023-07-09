using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Health", menuName = "ScriptableObjects/Health", order = 1)]
public class HealthSO : ScriptableObject
{
    [SerializeField] private int _startingHealth = 100;

    [Header("Asset References")]
    [SerializeField] private GameOverChannelSO _gameOverChannel;

    private int _currentHealth;

    void Start()
    {
        _currentHealth = _startingHealth;
    }

    void Update()
    {
        if (_currentHealth <= 0)
        {
            _gameOverChannel.RaiseEvent();
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
    }
}
