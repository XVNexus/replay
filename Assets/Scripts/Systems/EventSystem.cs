using System;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem current;
    private void Awake() => current = this;

    // Carries the replay data of a dead player which is used to construct a ghost replay
    public void TriggerPlayerDie(Vector2 spawnPoint, Vector2[] positionHistory) => OnPlayerDie?.Invoke(spawnPoint, positionHistory);
    public event Action<Vector2, Vector2[]> OnPlayerDie;

    // Initiates a level transition
    public void TriggerLevelComplete(int currentLevel, int ghostCount) => OnLevelComplete?.Invoke(currentLevel, ghostCount);
    public event Action<int, int> OnLevelComplete;
}
