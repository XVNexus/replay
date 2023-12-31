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
    public void TriggerLevelComplete() => OnLevelComplete?.Invoke();
    public event Action OnLevelComplete;

    // Signals where to move the player on the newly loaded level
    public void TriggerSpawn(Vector2 spawnpoint) => OnSpawn?.Invoke(spawnpoint);
    public event Action<Vector2> OnSpawn;

    // Triggered when the player collects any star
    public void TriggerStarCollected() => OnStarCollected?.Invoke();
    public event Action OnStarCollected;

    // Send new data to the ui to display
    public void TriggerUpdateUi(int? levelIndex, int? score) => OnUpdateUi?.Invoke(levelIndex, score);
    public event Action<int?, int?> OnUpdateUi;
}
