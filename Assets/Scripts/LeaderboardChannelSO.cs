using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Unity.Services.Leaderboards.Models;

[CreateAssetMenu(fileName = "LeaderboardChannel", menuName = "ScriptableObjects/LeaderboardChannel", order = 1)]
public class LeaderboardChannelSO : ScriptableObject
{
    public event GetLeaderboardEntriesDelegate GetLeaderboardEntriesEvent;
    public event ChangePlayerNameDelegate ChangePlayerNameEvent;
    public event UnityAction ScoreSuccessfullyUploadedEvent = delegate { };

    public bool isDefaultName { get; set;} = true;

    public async Task<LeaderboardEntry[]> GetLeaderboardEntries()
    {
        var result = await GetLeaderboardEntriesEvent?.Invoke();
        return result.ToArray();
    }

    public void ChangePlayerName(string name)
    {
        ChangePlayerNameEvent?.Invoke(name);
        isDefaultName = false;
    }

    public void RaiseScoreSuccessfullyUploadedEvent()
    {
        ScoreSuccessfullyUploadedEvent?.Invoke();
    }
}

public delegate Task<List<LeaderboardEntry>> GetLeaderboardEntriesDelegate();
public delegate void ChangePlayerNameDelegate(string name);