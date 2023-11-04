using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("General")]
    public float moveSpeed = 10f;
    public float moveForce = 10f;
    public float jumpForce = 10f;

    [Header("References")]
    public SensorController sensorControllerComponent;

    private Rigidbody2D rb;
    private bool isGrounded;

    // Start is called before the first frame update
    public void Start()
    {
        // Subcribe to events
        EventSystem.current.OnPlayerDie += OnPlayerDie;

        // Save a reference to the rigidbody component
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        // Check if on the ground
        isGrounded = Physics2D.Raycast(transform.position - new Vector3(0f, 0.51f), Vector2.down, 0.1f);

        // Apply movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.AddForce(new Vector2(horizontalInput * moveSpeed - rb.velocity.x, 0f) * moveForce);

        // Apply jump
        if (sensorControllerComponent.isTriggered && verticalInput > 0)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void OnPlayerDie(Vector2 spawnPoint, Vector2[] positionHistory)
    {
        rb.velocity = Vector2.zero;
    }
}
