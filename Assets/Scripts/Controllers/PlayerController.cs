using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    const float PLAYER_RADIUS = 1f;
    readonly Color COLOR_MAIN = new(1f, .75f, .25f);
    readonly Color COLOR_GHOST = new(.25f, .75f, 1f);

    [Header("General")]
    public Vector2 spawnpoint = new(0f, 0f);
    public bool ghostMode = false;

    [Header("References")]
    public SpriteRenderer spriteRendererComponent;
    public Light2D light2dComponent;
    public SensorController sensorControllerComponent;

    private List<Vector2> positionHistory = new();
    private bool recordingStarted = false;
    private Vector3 lastPosition = Vector3.zero;

    // Start is called before the first frame update
    public void Start()
    {
        // Subscribe to events
        EventSystem.current.OnLevelComplete += OnLevelComplete;
        EventSystem.current.OnSpawn += OnSpawn;

        // Figure out which color to use
        var color = ghostMode ? COLOR_GHOST : COLOR_MAIN;

        // Apply that color to the square and light
        spriteRendererComponent.color = color;
        light2dComponent.color = color;

        // If the player is a ghost, delete the rigidbody component
        if (ghostMode)
        {
            Destroy(GetComponent<Rigidbody2D>());
        }
        // If the player is not a ghost, add the movement controller script and link it to the ground sensor
        else
        {
            var playerMovementControllerComponent = gameObject.AddComponent(typeof(PlayerMovementController)) as PlayerMovementController;
            playerMovementControllerComponent.sensorControllerComponent = sensorControllerComponent;
        }
    }

    public void OnDestroy()
    {
        // Unsubscribe from events
        EventSystem.current.OnLevelComplete -= OnLevelComplete;
        EventSystem.current.OnSpawn -= OnSpawn;
    }

    // Add the current position to the history every physics update
    public void FixedUpdate()
    {
        // If the player is a ghost, skip this method
        if (ghostMode)
        {
            return;
        }

        // If recording has started and the player has moved since the last frame, save the current position to the replay data
        if (recordingStarted && transform.position != lastPosition)
        {
            positionHistory.Add(transform.position);
            lastPosition = transform.position;
        }
        // If the player has moved outside of the spawn point, start recording for a replay
        else if (Mathf.Abs(transform.position.x - spawnpoint.x) > PLAYER_RADIUS
            || Mathf.Abs(transform.position.y - spawnpoint.y) > PLAYER_RADIUS)
        {
            recordingStarted = true;
        }
    }

    // Reset the replay data when the level is completed successfully
    public void OnLevelComplete()
    {
        ResetReplayData();
    }

    // When the level is loaded, save the spawnpoint locally and move to that position
    public void OnSpawn(Vector2 spawnpoint)
    {
        this.spawnpoint = spawnpoint;
        transform.position = spawnpoint;
    }

    // If the player is not a ghost, emit an event containing the replay data and reset the player and replaying recorder
    public void Kill()
    {
        if (!ghostMode)
        {
            transform.position = spawnpoint;
            EventSystem.current.TriggerPlayerDie(spawnpoint, positionHistory.ToArray());
            ResetReplayData();
        }
    }

    public void ResetReplayData()
    {
        positionHistory.Clear();
        recordingStarted = false;
    }
}
