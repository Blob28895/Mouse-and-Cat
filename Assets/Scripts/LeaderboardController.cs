using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Unity.Services.Leaderboards.Models;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine.SceneManagement;
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
    [SerializeField] private LeaderboardChannelSO _leaderboardChannelSO;

    private async void Awake()
    {
        await UnityServices.InitializeAsync();

        if(!AuthenticationService.Instance.IsSignedIn)
            await SignInAnonymously();

        Debug.Log(await AuthenticationService.Instance.GetPlayerNameAsync());
    }

    private async Task SignInAnonymously()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            Debug.Log(s);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Start()
    {
        _leaderboardChannelSO.GetLeaderboardEntriesEvent += GetLeaderboardEntries;
        _leaderboardChannelSO.ChangePlayerNameEvent += ChangePlayerName;
    }

    private void OnEnable() { _gameOverChannelSO.GameOverEvent += IsLeaderboardScore; }

    private void OnDisable() { _gameOverChannelSO.GameOverEvent -= IsLeaderboardScore; }

    // Adds new highscores to leaderboard, replaces existing score
    public void IsLeaderboardScore()
    {
        // store current score just in case it gets changed
        int potentialLeaderboardScore = _scoreSO.score;

        if (potentialLeaderboardScore >= _scoreSO.highScore)
            AddScoreEntry(potentialLeaderboardScore, GetLeaderboardId());
    }

    private void ChangePlayerName(String name)
    {
        AuthenticationService.Instance.UpdatePlayerNameAsync(name);
    }

    private async Task<List<LeaderboardEntry>> GetLeaderboardEntries()
    {
        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(GetLeaderboardId());
        return scoresResponse.Results;
    }

    private async void AddScoreEntry(int score, String leaderboardId)
    {
        Debug.Log("Adding score: " + score + " to leaderboard: " + leaderboardId);
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score);
    }

    // gets id from scene name (scene name with underscores instead of spaces)
    private String GetLeaderboardId()
    {
        String sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(sceneName.Replace(" ", "_"));
        return sceneName.Replace(" ", "_");
    }
}