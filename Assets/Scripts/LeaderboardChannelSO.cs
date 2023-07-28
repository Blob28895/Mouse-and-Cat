using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Leaderboards.Models;
using System.Threading.Tasks;

[CreateAssetMenu(fileName = "LeaderboardChannel", menuName = "ScriptableObjects/LeaderboardChannel", order = 1)]
public class LeaderboardChannelSO : ScriptableObject
{
    public event GetLeaderboardEntriesDelegate GetLeaderboardEntriesEvent;

    public LeaderboardEntry[] GetLeaderboardEntries()
    {
        return GetLeaderboardEntriesEvent?.Invoke().Result.ToArray();
    }
}

public delegate Task<List<LeaderboardEntry>> GetLeaderboardEntriesDelegate();