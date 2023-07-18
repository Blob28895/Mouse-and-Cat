using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;

// TODO:
// - Currently new high scores aren't being reported
// - Need to get score and only add if higher
// - Prompt for player username when new entry is achieved
// - Currently add score just adds a new score, need to update existing score if higher
// - Exception is thrown when SignInAnonymously is called when already signed in

public class LeaderboardController : MonoBehaviour
{
    [SerializeField] private ScoreSO _scoreSO;
    [SerializeField] private GameOverChannelSO _gameOverChannelSO;

    private string _currLeaderboardId = "Living_Room_Highscores";

    private async void Awake()
    {
        await UnityServices.InitializeAsync();

        await SignInAnonymously();
    }

    private async Task SignInAnonymously()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log(s);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void OnEnable()
    {
        _gameOverChannelSO.GameOverEvent += IsLeaderboardScore;
    }

    private void OnDisable()
    {
        _gameOverChannelSO.GameOverEvent -= IsLeaderboardScore;
    }

    // Checks if new score is applicable and adds it to the leaderboard if necessary
    public void IsLeaderboardScore()
    {
        if (_scoreSO.scoreReported)
        {
            return;
        }

        _scoreSO.scoreReported = true;
        AddScore(_scoreSO.score);
        GetScores();
    }

    private async void AddScore(int score)
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(_currLeaderboardId, score);
        Debug.Log(JsonConvert.SerializeObject(scoreResponse));
    }

    private async void GetScores()
    {
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(_currLeaderboardId);
        Debug.Log(JsonConvert.SerializeObject(scoresResponse));
    }
}