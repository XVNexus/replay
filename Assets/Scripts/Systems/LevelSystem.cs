using System;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    [Header("References")]
    public GameObject[] levelPrefabs;

    public bool isLevelLoaded { get => currentLevelObject != null; }

    private GameObject currentLevelObject = null;
    private int currentLevel = 0;
    private int[] ghostCounts;

    public void Start()
    {
        // Subscribe to events
        EventSystem.current.OnPlayerDie += OnPlayerDie;
        EventSystem.current.OnLevelComplete += OnLevelComplete;

        // Initialize ghost tracker
        ghostCounts = new int[levelPrefabs.Length];

        // Load level 1
        LoadLevel(currentLevel);
    }

    // Incremement level ghost counter
    public void OnPlayerDie(Vector2 spawnPoint, Vector2[] positionHistory)
    {
        ghostCounts[currentLevel]++;
    }

    // Destroy the current level and transition to the next level
    public void OnLevelComplete()
    {
        Debug.Log($"Ghost count for level {currentLevel + 1}: {ghostCounts[currentLevel]}");
        UnloadCurrentLevel();
        currentLevel++;
        LoadLevel(currentLevel);
    }

    // Load a level by the given index
    public void LoadLevel(int levelIndex)
    {
        if (!isLevelLoaded)
        {
            currentLevelObject = Instantiate(levelPrefabs[levelIndex], Vector2.zero, Quaternion.identity);
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
