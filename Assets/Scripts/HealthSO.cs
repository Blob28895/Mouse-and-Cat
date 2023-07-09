using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "Health", menuName = "ScriptableObjects/Health", order = 1)]
public class HealthSO : ScriptableObject
{
    [SerializeField] private int _startingHealth = 100;

    [Tooltip("How long to wait before starting to regen health")]
    [SerializeField] private float _timeToWaitForRegen = 5f;

    [Tooltip("How much health to regen each increment")]
    [SerializeField] private int _regenIncrementAmount = 1;

    [Tooltip("How long to wait between each increment")]
    [SerializeField] private float _timeBetweenRegenIncrement = 1;

    [Header("Asset References")]
    [SerializeField] private GameOverChannelSO _gameOverChannel;

    private int _currentHealth;

    void OnEnable()
    {
        _currentHealth = _startingHealth;
    }

    public void resetHealth()
    {
		_currentHealth = _startingHealth;
	}

    public void Damage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _gameOverChannel.RaiseEvent();
        }
    }

    public IEnumerator AttemptToRegen()
    {
        int initialHealth = _currentHealth;

        yield return new WaitForSeconds(_timeToWaitForRegen);

        while (_currentHealth < _startingHealth)
        {
            // if we took damage during the wait, don't regen
            if(initialHealth != _currentHealth) 
            { 
                yield break; 
            }

            _currentHealth += _regenIncrementAmount;
            initialHealth = _currentHealth;
            yield return new WaitForSeconds(_timeBetweenRegenIncrement);
        }
    }

    public int GetStartingHealth() { return _startingHealth; }
    public int GetHealth() { return _currentHealth; }
}
