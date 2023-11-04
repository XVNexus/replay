using UnityEngine;

public class GhostSystem : MonoBehaviour
{
    [Header("References")]
    public GameObject playerPrefab;

    // Start is called before the first frame update
    public void Start()
    {
        // Subcribe to events
        EventSystem.current.OnPlayerDie += OnPlayerDie;
    }

    // Create a new replay ghost when the player dies
    public void OnPlayerDie(Vector2 spawnPoint, Vector2[] positionHistory)
    {
        var ghostPlayer = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);
        var replayController = ghostPlayer.AddComponent(typeof(PlayerReplayController)) as PlayerReplayController;
        replayController.positionHistory = positionHistory;
    }
}
