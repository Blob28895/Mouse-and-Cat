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


    private int _score = 0;
    private float _scoringTime = 0f;

	private void Start()
	{
        //scoreText.text = "testing this bullshit";
	}
	// Update is called once per frame
	void FixedUpdate()
    {
        if(_scoringTime <= Time.time)
        {
            Debug.Log("Updating Score");
            _score += scorePerTime;
            _score += scorePerMouseAmount * (getNumberOfMice() / mouseMultiplyCount);
            //Debug.Log(_score);
            scoreText.text = " ";
            scoreText.text = "Score: " + _score.ToString();

            _scoringTime = secondsPerScore + Time.time;
        }
    }

    private void updateText(string s)
    {
		scoreText.text = s;
	}
    private int getNumberOfMice()
    {
        return GameObject.FindGameObjectsWithTag("Mouse").Length;
    }
}