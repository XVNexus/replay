using System;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem current;
    private void Awake() => current = this;

    // This event carries the replay data of a dead player which is used to construct a ghost replay
    public void TriggerPlayerDie(Vector2 spawnPoint, List<Vector2> positionHistory) => OnPlayerDie?.Invoke(spawnPoint, positionHistory);
    public event Action<Vector2, List<Vector2>> OnPlayerDie;
}
