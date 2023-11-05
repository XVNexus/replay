using UnityEngine;

public class SpawnpointController : MonoBehaviour
{
    [Header("General")]
    public Vector2 spawnPosition;

    // Send the spawnpoint to the event system
    void Start()
    {
        EventSystem.current.TriggerSpawn(spawnPosition);
    }
}
