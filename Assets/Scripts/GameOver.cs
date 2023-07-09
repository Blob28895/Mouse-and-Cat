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
    }

    private void OnGameOver()
    {
        if (_gameOver) { return; }

        gameOverPanel.SetActive(true);

        highScoreUI.text = scoreSO.highScore.ToString();
        scoreUI.text = scoreSO.score.ToString();
        _gameOver = true;
    }
}
