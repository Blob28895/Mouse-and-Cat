using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Leaderboards.Models;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class LeaderboardUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject _scoresPanel;
    [SerializeField] private GameObject _nameEntryPanel;

    [Header("Asset References")]
    [SerializeField] private LeaderboardChannelSO _leaderboardChannel = default;
    [SerializeField] private ScoreSO _scoreSO = default;

    private List<ScoreEntry> _scoreEntries = new List<ScoreEntry>();

    private void Start()
    {
        // get score entry objects
        GameObject[] childObjects = _scoresPanel.transform.Cast<Transform>().Select(child => child.gameObject).ToArray();

        foreach(GameObject obj in childObjects)
        {
            TextMeshProUGUI[] textObjects = obj.GetComponentsInChildren<TextMeshProUGUI>();
            
            _scoreEntries.Add(new ScoreEntry (textObjects[0], textObjects[2], textObjects[1]));
        }
    }

    private void OnEnable()
    {
        UpdateScoresUI();
        _leaderboardChannel.ScoreSuccessfullyUploadedEvent += UpdateScoresUI;

        if (_leaderboardChannel.isDefaultName && _scoreSO.getScore() >= _scoreSO.highScores[gameObject.scene.name])
        {
            _nameEntryPanel.SetActive(true);
            _leaderboardChannel.NameSuccessfullyChangedEvent += UpdateScoresUI;
        }
    }

    private void OnDisable()
    {
        _leaderboardChannel.ScoreSuccessfullyUploadedEvent -= UpdateScoresUI;
    }

    private async void UpdateScoresUI()
    {   
        LeaderboardEntry[] leaderboard = await _leaderboardChannel.GetLeaderboardEntries();
        Debug.Log("Updating UI with Entries from Server");
        
        for(int i = 0; i < leaderboard.Length; i ++)
        {
            _scoreEntries[i].nameText.text = leaderboard[i].PlayerName.ToString();
            _scoreEntries[i].scoreText.text = leaderboard[i].Score.ToString();
            _scoreEntries[i].rankText.text = (leaderboard[i].Rank + 1).ToString();
        }
    }

    public void SubmitName(TMP_InputField inputField)
    {
        _leaderboardChannel.ChangePlayerName(inputField.text);
        _nameEntryPanel.SetActive(false);
    }
}

public class ScoreEntry
{
    public TextMeshProUGUI nameText { get; set; }
    public TextMeshProUGUI scoreText { get; set; }
    public TextMeshProUGUI rankText { get; set; }

    public ScoreEntry(TextMeshProUGUI nameText, TextMeshProUGUI scoreText, TextMeshProUGUI rankText)
    {
        this.nameText = nameText;
        this.scoreText = scoreText;
        this.rankText = rankText;
    }
}
