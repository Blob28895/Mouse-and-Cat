using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Unity.Services.Leaderboards.Models;
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


// BRO THERE IS LIKE NO DOCS FOR Unity.Services.Leaderboards
// https://cloud-code-sdk-documentation.cloud.unity3d.com/leaderboards/v1.1/leaderboardsapi#Import (Why is there javscript docs but not C#?) 
// https://forum.unity.com/threads/how-can-i-extract-values-to-use-from-the-data-the-methods-return.1423920/
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

    private void OnEnable() { _gameOverChannelSO.GameOverEvent += IsLeaderboardScore; }

    private void OnDisable() { _gameOverChannelSO.GameOverEvent -= IsLeaderboardScore; }

    // Checks if new score is applicable and adds it to the leaderboard if necessary
    public async void IsLeaderboardScore()
    {
        // store current score just in case it gets changed
        int potentialLeaderboardScore = _scoreSO.score;

        if (potentialLeaderboardScore < _scoreSO.highScore)
            return;

        // prevents same score being reported twice
        _scoreSO.scoreReported = true;

        // if player already has a high score entry, replace it
        LeaderboardEntry playerEntry = await GetCurrentPlayerEntry();
        if (playerEntry.Score < potentialLeaderboardScore)
        {
            AddScoreEntry(potentialLeaderboardScore);
            return;
        }

        // if player doesn't have a high score entry and qualifies, add one
        LeaderboardEntry minEntry = await GetMinimumLeaderboardScoreEntry();
        if (minEntry.Score < potentialLeaderboardScore)
        {
            AddScoreEntry(potentialLeaderboardScore);
            return;
        }
    }

    private async void AddScoreEntry(int score)
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(_currLeaderboardId, score);
    }

    private async Task<LeaderboardEntry> GetMinimumLeaderboardScoreEntry()
    {
        var scoresResponse =
            await LeaderboardsService.Instance.GetScoresAsync(_currLeaderboardId);
        
        var minEntry = scoresResponse.Results.Min();
        return minEntry;
    }

    private async Task<LeaderboardEntry> GetCurrentPlayerEntry()
    {
        try
        {
            return await LeaderboardsService.Instance.GetPlayerScoreAsync(_currLeaderboardId);
        }
        catch(Exception e)
        {
            return default(LeaderboardEntry);
        }
    }
}