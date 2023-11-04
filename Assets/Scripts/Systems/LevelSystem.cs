using System;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    const int SCORE_BASE = 100;
    const int REWARD_DEATH = -10;
    const int REWARD_STAR = 50;

    [Header("References")]
    public GameObject[] levelPrefabs;

    public bool isLevelLoaded { get => currentLevelObject != null; }

    private GameObject currentLevelObject = null;
    private int currentLevel = 0;
    private int[] scores;

    public void Start()
    {
        // Subscribe to events
        EventSystem.current.OnPlayerDie += OnPlayerDie;
        EventSystem.current.OnLevelComplete += OnLevelComplete;
        EventSystem.current.OnStarCollected += OnStarCollected;

        // Initialize ghost tracker
        scores = new int[levelPrefabs.Length];

        // Load level 1
        LoadLevel(currentLevel);
    }

    // Give the player a punishment for every ghost they use to encrouage using the least amount of ghosts possible
    public void OnPlayerDie(Vector2 spawnPoint, Vector2[] positionHistory)
    {
        scores[currentLevel] += REWARD_DEATH;
    }

    // Destroy the current level and transition to the next level
    public void OnLevelComplete()
    {
        Debug.Log($"Score for level {currentLevel + 1}: {scores[currentLevel]}");

        UnloadCurrentLevel();
        currentLevel++;
        LoadLevel(currentLevel);
    }

    // Give the player a reward when a star is collected
    public void OnStarCollected()
    {
        scores[currentLevel] += REWARD_STAR;
    }

    // Load a level by the given index
    public void LoadLevel(int levelIndex)
    {
        if (!isLevelLoaded)
        {
            currentLevelObject = Instantiate(levelPrefabs[levelIndex], Vector2.zero, Quaternion.identity);
            scores[levelIndex] = SCORE_BASE;
        }
        else
        {
            throw new InvalidOperationException("Cannot load a level while one is already active");
        }
    }

    // Unload whatever level is active right now
    public void UnloadCurrentLevel()
    {
        if (isLevelLoaded)
        {
            Destroy(currentLevelObject);
            currentLevelObject = null;
        }
        else
        {
            throw new InvalidOperationException("Cannot unload a level while nothing is loaded");
        }
    }
}
