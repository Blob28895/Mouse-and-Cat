using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(fileName = "Score", menuName = "ScriptableObjects/Score", order = 1)]
public class ScoreSO : ScriptableObject
{
    public Dictionary<string, int> highScores = new Dictionary<string, int>();
    private int _score = 0;

    public void setScore(int s, string sceneName)
    {
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
            highScores.Add(sceneName, _score);
        }
    }

    public int getScore()
    {
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
