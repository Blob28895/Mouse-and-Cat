using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject gameOverPanel = default;
    [SerializeField] private TextMeshProUGUI highScoreUI = default;
    [SerializeField] private TextMeshProUGUI scoreUI = default;
    [SerializeField] private TextMeshProUGUI newHighScoreUI = default;


    [Header("Asset References")]
    [SerializeField] private GameOverChannelSO gameOverChannel = default;
    [SerializeField] private ScoreSO scoreSO = default;

    private bool _gameOver = false;

    private void OnEnable()
    {
        gameOverChannel.GameOverEvent += OnGameOver;
    }

    private void OnDisable()
    {
        gameOverChannel.GameOverEvent -= OnGameOver;
        newHighScoreUI.gameObject.SetActive(false);
    }

    private void OnGameOver()
    {
        if (_gameOver) { return; }

        gameOverPanel.SetActive(true);
        Debug.Log("Game Over");
        if(scoreSO.score >= scoreSO.highScore)
        {
            newHighScoreUI.gameObject.SetActive(true);
        }

        highScoreUI.text = "Highscore: " + scoreSO.highScore.ToString();
        scoreUI.text = "Score: " + scoreSO.score.ToString();
        _gameOver = true;
    }
}
