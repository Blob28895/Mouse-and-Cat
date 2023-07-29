using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Leaderboards.Models;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "LeaderboardChannel", menuName = "ScriptableObjects/LeaderboardChannel", order = 1)]
public class LeaderboardChannelSO : ScriptableObject
{
    public event GetLeaderboardEntriesDelegate GetLeaderboardEntriesEvent;
    public event ChangePlayerNameDelegate ChangePlayerNameEvent;

    public bool isDefaultName { get; private set;} = true;

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
}

public delegate Task<List<LeaderboardEntry>> GetLeaderboardEntriesDelegate();
public delegate void ChangePlayerNameDelegate(string name);