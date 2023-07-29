using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoring : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    [Tooltip("Score that will be awarded to the player every second that they stay alive")]
    [SerializeField] private int scorePerTime = 5;

    [Tooltip("Number of seconds that will pass before the player is awarded more points")]
    [SerializeField] private float secondsPerScore = 1f;

    [Tooltip("Bonus points awarded for increments of mice on the screen")]
    [SerializeField] private int scorePerMouseAmount = 5;

    [Tooltip("Number of mice required to multiply by mouse multiplier again")]
    [SerializeField] private int mouseMultiplyCount = 2;


    [Header("Asset References")]
    [SerializeField] private ScoreSO scoreSO = default;
    [SerializeField] private GameOverChannelSO _gameOver = default;

    private int _score = 0;
    private float _scoringTime = 0f;
    private int _currentMultiplier = 1;
    private bool _ableToScore = true;

    private void OnEnable()
    {
        _gameOver.GameOverEvent += turnOffScoring;
    }

    private void OnDisable()
    {
        _gameOver.GameOverEvent -= turnOffScoring;
    }

	// Update is called once per frame
	void FixedUpdate()
    {
        updateMultiplier();

        if(_scoringTime <= Time.time && _ableToScore)
        {
            _score += scorePerTime;
            _score += scorePerMouseAmount * _currentMultiplier;

            scoreSO.setScore(_score, gameObject.scene.name);

            scoreText.text = " ";
            scoreText.text = "Score: " + _score.ToString();

            _scoringTime = secondsPerScore + Time.time;
        }
    }

    private void updateText(string s)
    {
		scoreText.text = s;
	}

    private void updateMultiplier()
    {//10 - 2,  15
        if(getNumberOfMice() - _currentMultiplier * mouseMultiplyCount >= mouseMultiplyCount)
        {
            //Debug.Log("Increment multiplier");
            _currentMultiplier += 1;
        }
    }
    private int getNumberOfMice()
    {
        return GameObject.FindGameObjectsWithTag("Mouse").Length;
    }

    private void turnOffScoring()
    {
        _ableToScore = false;
    }
}
