using UnityEngine;

public class SpawnpointController : MonoBehaviour
{
    // Send the spawnpoint to the event system
    void Start()
    {
        EventSystem.current.TriggerSpawn(transform.position);
    }
}
