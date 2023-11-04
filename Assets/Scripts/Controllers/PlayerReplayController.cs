using UnityEngine;

public enum ReplayDifficulty : int
{
    TUTORIAL, EASY, MEDIUM, HARD
}

public class PlayerReplayController : MonoBehaviour
{
    [Header("General")]
    public ReplayDifficulty difficulty = ReplayDifficulty.EASY;

    [Header("Data")]
    public Vector2[] positionHistory;

    private float replaySpeed = 0f;
    private float positionFrame = 0f;
    private bool abandonLevel = false;

    public void Start()
    {
        // Subscribe to events
        EventSystem.current.OnLevelComplete += OnLevelComplete;

        // Set speed based on difficulty level
        replaySpeed = difficulty switch
        {
            ReplayDifficulty.TUTORIAL => .1f,
            ReplayDifficulty.EASY => .25f,
            ReplayDifficulty.MEDIUM => .5f,
            ReplayDifficulty.HARD => .75f,
            _ => 0f
        };
    }

    public void OnDestroy()
    {
        // Unsubscribe from events
        EventSystem.current.OnLevelComplete -= OnLevelComplete;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        // Move the ghost to the next replay frame while obeying time scale
        if (positionFrame < positionHistory.Length - 1)
        {
            var frame1 = Mathf.FloorToInt(positionFrame);
            var frame2 = Mathf.CeilToInt(positionFrame);
            var position1 = positionHistory[frame1];
            var position2 = positionHistory[frame2];
            var positionLerp = Vector2.Lerp(position1, position2, positionFrame - frame1);
            transform.position = positionLerp;
            positionFrame += replaySpeed;
        }
        // When the replay runs out of data, delete this ghost
        else
        {
            Destroy(gameObject);
        }

        // After the level is complete, accelerate the replay until it runs out of position history
        if (abandonLevel)
        {
            replaySpeed += Time.fixedDeltaTime * 10f;
        }
    }

    // When the level is completed, current ghost replays will have colission disabled and quickly leave the level
    public void OnLevelComplete()
    {
        abandonLevel = true;
        Destroy(GetComponent<BoxCollider2D>());
    }
}
