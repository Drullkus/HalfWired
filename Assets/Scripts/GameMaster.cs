using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

[AutoStaticsCleanup]
public class GameMaster
{
    public static GameMaster Instance { get; } = new();

    public int TowerPadding = 2;
    public float RocketDifficulty = 1.0f;
    
    public bool MenuOpen = true;
    
    private List<(string name, int score)> _playerScores = new()
    {
        ("Paperboy", 67),
        ("Marvin", 42),
        ("Greg", 40),
        ("Harry", 35),
        ("Fives", 32),
        ("Gordon", 30),
        ("Sparky", 25),
        ("Nyan", 20),
        ("Lemon", 15),
        ("Goldfish", 10)
    };

    private GameMaster()
    {
        Debug.Log("GameMaster initialized");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public ImmutableList<(string name, int score)> GetPlayerScores() => _playerScores.ToImmutableList();

    public string GetScoresString() => string.Join("\n", _playerScores.Select(x => $"{x.name}: {x.score}"));
    
    private void SortList() => _playerScores = _playerScores.OrderByDescending(x => x.score).Take(10).ToList(); // Ties broken by insertion order

    public void AddScore(string name, int score)
    {
        _playerScores.Add((name, score));
        SortList();
    }

    public bool IsNewHighScore(int score)
    {
        return _playerScores.Count < 10 || _playerScores.Any(x => score > x.score);
    }

}
