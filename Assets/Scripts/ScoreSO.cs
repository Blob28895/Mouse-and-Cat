using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Score", menuName = "ScriptableObjects/Score", order = 1)]
public class ScoreSO : ScriptableObject
{
    public int score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;

            if(_score >= highScore)
            {
                highScore = _score;
            }
        }
    }

    public int highScore {get; private set; } = 0;

    private int _score = 0;
    
    private void OnEnable()
    {
        _score = 0;
    }

    public void ResetScore()
    {
        _score = 0;
        highScore = 0;
    }
}
