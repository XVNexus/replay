using UnityEngine;

public class SensorController : MonoBehaviour
{
    int overlaps;

    public bool isTriggered {
        get {
            return overlaps > 0;
        }
    }

    public void Start()
    {
        // Subscribe to events
        EventSystem.current.OnPlayerDie += OnPlayerDie;
    }

    // Keep track of how many objects are hitting this trigger to see if the player is on the ground
    public void OnTriggerEnter2D() => overlaps++;
    public void OnTriggerExit2D() => overlaps--;

    // When the player dies, reset overlapping object counter to avoid issues with sensor always being triggered
    public void OnPlayerDie(Vector2 spawnpoint, Vector2[] positionHistory)
    {
        overlaps = 0;
    }
}
