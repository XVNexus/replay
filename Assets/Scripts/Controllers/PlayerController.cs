using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    const float PLAYER_RADIUS = 1f;
    const int MAX_REPLAY_FRAMES = 150; // Up to 3 seconds of replay data
    readonly Color COLOR_MAIN = new(1f, .66f, 0f);
    readonly Color COLOR_GHOST = new(0f, .66f, 1f);

    [Header("General")]
    public Vector2 spawnpoint = new(0f, 0f);
    public bool ghostMode = false;

    [Header("References")]
    public Light2D light2dComponent;
    public SensorController sensorControllerComponent;

    private SpriteRenderer spriteRendererComponent;
    private BoxCollider2D colliderComponent;

    private List<Vector2> positionHistory = new();
    private bool recordingStarted = false;
    private Vector3 lastPosition = Vector3.zero;
    private bool spawnAnimationComplete = false;

    // Start is called before the first frame update
    public void Start()
    {
        // Subscribe to events
        EventSystem.current.OnLevelComplete += OnLevelComplete;
        EventSystem.current.OnSpawn += OnSpawn;

        // Get component references
        spriteRendererComponent = GetComponent<SpriteRenderer>();
        colliderComponent = GetComponent<BoxCollider2D>();

        // Figure out which color to use
        var color = ghostMode ? COLOR_GHOST : COLOR_MAIN;

        // Apply that color to the square and light
        spriteRendererComponent.color = color;
        light2dComponent.color = color;

        // If the player is a ghost, delete the rigidbody component and do a fade in animation
        if (ghostMode)
        {
            Destroy(GetComponent<Rigidbody2D>());
            transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, Vector3.one, .5f)
                .setEaseOutExpo();
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
        // If the player has moved outside of the spawn point and there is no active spawn animation, start recording replay data
        else if (spawnAnimationComplete && (Mathf.Abs(transform.position.x - spawnpoint.x) > PLAYER_RADIUS
            || Mathf.Abs(transform.position.y - spawnpoint.y) > PLAYER_RADIUS))
        {
            recordingStarted = true;
        }
    }

    // Reset the replay data when the level is completed successfully
    public void OnLevelComplete()
    {
        ResetReplayData();
    }

    // When a level is loaded and the player is not a ghost, save the spawnpoint locally and move to that position
    public void OnSpawn(Vector2 spawnpoint)
    {
        if (!ghostMode)
        {
            this.spawnpoint = spawnpoint;
            MoveToSpawn(true);
        }
    }

    public void Kill()
    {
        // If the player is not a ghost, emit an event containing the replay data and reset the player and replay recorder
        if (!ghostMode)
        {
            Vector2[] positionHistoryArray = new Vector2[Mathf.Min(positionHistory.Count, MAX_REPLAY_FRAMES)];
            var startIndex = Mathf.Max(0, positionHistory.Count - MAX_REPLAY_FRAMES);
            for (var i = startIndex; i < positionHistory.Count; i++)
            {
                positionHistoryArray[i - startIndex] = positionHistory[i];
            }
            EventSystem.current.TriggerPlayerDie(spawnpoint, positionHistoryArray);

            ResetReplayData();
            MoveToSpawn();
        }
    }

    public void ResetReplayData()
    {
        positionHistory.Clear();
        recordingStarted = false;
    }

    public void MoveToSpawn(bool springy = false)
    {
        spawnAnimationComplete = false;
        colliderComponent.enabled = false;
        LeanTween.move(gameObject, spawnpoint, 1f)
            .setEase(springy ? LeanTweenType.easeInOutBack : LeanTweenType.easeInOutExpo)
            .setOnComplete(() => {
                spawnAnimationComplete = true;
                colliderComponent.enabled = true;
            });
    }
}
