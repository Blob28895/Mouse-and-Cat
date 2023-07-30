using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "Score", menuName = "ScriptableObjects/Score", order = 1)]
public class ScoreSO : ScriptableObject
{
    public Dictionary<string, int> highScores;
    private int _score = 0;

    public void setScore(int s, string sceneName)
    {
        if(highScores == null)
        {
            highScores = new Dictionary<string, int>();
        }
        
        _score = s;

        if(highScores.ContainsKey(sceneName))
        {
            if(_score > highScores[sceneName])
            {
                highScores[sceneName] = _score;
            }
        }
        else
        {
            highScores[sceneName] = _score;
        }
    }

    public int getScore()
    {
        Debug.Log("Getting score: " + _score);
        return _score;
    }
    
    private void OnEnable()
    {
        _score = 0;
    }

    private void OnDisable()
    {
        _score = 0;
    }
}
