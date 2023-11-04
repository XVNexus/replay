using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    const float PLAYER_RADIUS = 1f;
    readonly Color COLOR_MAIN = new(1f, .75f, .25f);
    readonly Color COLOR_GHOST = new(.25f, .75f, 1f);

    [Header("General")]
    public Vector2 spawnPoint = new(-8f, 0f);
    public bool ghostMode = false;

    [Header("References")]
    public SpriteRenderer spriteRendererComponent;
    public Light2D light2dComponent;
    public SensorController sensorControllerComponent;

    private List<Vector2> positionHistory = new();
    private bool recordingStarted = false;

    // Start is called before the first frame update
    public void Start()
    {
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

    // Add the current position to the history every physics update
    public void FixedUpdate()
    {
        // If the player is a ghost, skip this method
        if (ghostMode)
        {
            return;
        }

        // If recording has started, save the current position to the replay data
        if (recordingStarted)
        {
            positionHistory.Add(transform.position);
        }
        // If the player has moved outside of the spawn point, start recording for a replay
        else if (Mathf.Abs(transform.position.x - spawnPoint.x) > PLAYER_RADIUS
            || Mathf.Abs(transform.position.y - spawnPoint.y) > PLAYER_RADIUS)
        {
            recordingStarted = true;
        }
    }

    // If the player is not a ghost, emit an event containing the replay data and reset the player and replaying recorder
    public void Kill()
    {
        if (!ghostMode)
        {
            EventSystem.current.TriggerPlayerDie(spawnPoint, positionHistory.ToArray());
            transform.position = spawnPoint;
            positionHistory.Clear();
            recordingStarted = false;
        }
    }
}
