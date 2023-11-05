using System;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    const int SCORE_BASE = 100;
    const int REWARD_DEATH = -10;
    const int REWARD_STAR = 50;

    [Header("References")]
    public GameObject[] levelPrefabs;
    public GameObject endLevelPrefab;

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
        EventSystem.current.TriggerUpdateUi(null, scores[currentLevel]);
    }

    // Destroy the current level and transition to the next level, or go to end screen if no more levels
    public void OnLevelComplete()
    {
        UnloadCurrentLevel();
        if (currentLevel < levelPrefabs.Length - 1)
        {
            LoadLevel(++currentLevel);
        }
        else
        {
            Instantiate(endLevelPrefab, Vector2.zero, Quaternion.identity);
            var highscore = 0;
            for (var i = 0; i < levelPrefabs.Length; i++)
            {
                highscore += scores[i];
            }
            EventSystem.current.TriggerUpdateUi(-1, highscore);
        }
    }

    // Give the player a reward when a star is collected
    public void OnStarCollected()
    {
        scores[currentLevel] += REWARD_STAR;
        EventSystem.current.TriggerUpdateUi(null, scores[currentLevel]);
    }

    // Load a level by the given index
    public void LoadLevel(int levelIndex)
    {
        var isEndLevel = levelIndex == levelPrefabs.Length;
        var levelPrefab = isEndLevel ? endLevelPrefab : levelPrefabs[levelIndex];
        currentLevelObject = Instantiate(levelPrefab, Vector2.zero, Quaternion.identity);
        currentLevelObject.transform.position = new Vector3(0f, -10f);
        LeanTween.moveLocalY(currentLevelObject, 0f, .5f)
            .setDelay(.5f)
            .setEaseOutExpo();
        scores[levelIndex] = SCORE_BASE;
        EventSystem.current.TriggerUpdateUi(isEndLevel ? -1 : levelIndex, SCORE_BASE);
    }

    // Unload whatever level is active right now
    public void UnloadCurrentLevel()
    {
        if (currentLevelObject == null)
        {
            throw new InvalidOperationException("Cannot unload a level while nothing is loaded");
        }

        var levelToUnload = currentLevelObject;
        LeanTween.moveY(levelToUnload, 10f, .5f)
            .setEaseInExpo()
            .setOnComplete(() => Destroy(levelToUnload));
    }
}
