using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Unity.Services.Leaderboards.Models;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine.SceneManagement;
using UnityEngine;

// BRO THERE IS LIKE NO DOCS FOR Unity.Services.Leaderboards
// https://cloud-code-sdk-documentation.cloud.unity3d.com/leaderboards/v1.1/leaderboardsapi#Import (Why is there javscript docs but not C#?) 
// https://forum.unity.com/threads/how-can-i-extract-values-to-use-from-the-data-the-methods-return.1423920/
public class LeaderboardController : MonoBehaviour
{
    [SerializeField] private ScoreSO _scoreSO;
    [SerializeField] private GameOverChannelSO _gameOverChannelSO;
    [SerializeField] private LeaderboardChannelSO _leaderboardChannelSO;

    private ProfanityFilter _filter = new ProfanityFilter();

    private async void Awake()
    {
        await UnityServices.InitializeAsync();

        if(!AuthenticationService.Instance.IsSignedIn)
            await SignInAnonymously();

        // check if default name
        // THIS IS KINDA HARDCODED, OUR NAMES ARE LIMITED TO 8 CHARS IN THE INPUT BOX
        var playerName = await AuthenticationService.Instance.GetPlayerNameAsync();
        if(playerName.ToString().Length <= 8)
        {
            _leaderboardChannelSO.isDefaultName = false;
        }
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

    private void OnEnable() 
    { 
        _gameOverChannelSO.GameOverEvent += IsLeaderboardScore; 
        _leaderboardChannelSO.GetLeaderboardEntriesEvent += GetLeaderboardEntries;
        _leaderboardChannelSO.ChangePlayerNameEvent += ChangePlayerName;
    }

    private void OnDisable() 
    { 
        _gameOverChannelSO.GameOverEvent -= IsLeaderboardScore;
        _leaderboardChannelSO.GetLeaderboardEntriesEvent -= GetLeaderboardEntries;
        _leaderboardChannelSO.ChangePlayerNameEvent -= ChangePlayerName;
    }

    public void IsLeaderboardScore()
    {
        // store current score just in case it gets changed
        int potentialLeaderboardScore = _scoreSO.getScore();

        if (potentialLeaderboardScore >= _scoreSO.highScores[gameObject.scene.name])
            AddScoreEntry(potentialLeaderboardScore, GetLeaderboardId());
    }

    private async void AddScoreEntry(int score, String leaderboardId)
    {
        Debug.Log("Adding score: " + score + " to leaderboard: " + leaderboardId);
        await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score);
        _leaderboardChannelSO.RaiseScoreSuccessfullyUploadedEvent();
    }

    private async void ChangePlayerName(String name)
    {
        // String filteredName = await _filter.FilterWord(name);
        await AuthenticationService.Instance.UpdatePlayerNameAsync(name.ToString());
        _leaderboardChannelSO.RaiseNameSuccessfullyChangedEvent();
    }

    private async Task<List<LeaderboardEntry>> GetLeaderboardEntries()
    {
        var getScoreOptions = new GetScoresOptions { Offset = 0, Limit = 12};
        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(GetLeaderboardId(), getScoreOptions);
        return scoresResponse.Results;
    }

    // gets id from scene name (scene name with underscores instead of spaces)
    private String GetLeaderboardId()
    {
        String sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(sceneName.Replace(" ", "_"));
        return sceneName.Replace(" ", "_");
    }
}