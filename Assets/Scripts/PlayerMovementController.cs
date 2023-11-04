using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("General")]
    public float moveSpeed = 10f;
    public float moveForce = 10f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        // Save a reference to the rigidbody component
        rb = GetComponent<Rigidbody2D>();

        // Subcribe to events
        EventSystem.current.OnPlayerDie += OnPlayerDie;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check if on the ground
        isGrounded = Physics2D.Raycast(transform.position - new Vector3(0f, 0.51f), Vector2.down, 0.1f);

        // Apply movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.AddForce(new Vector2(horizontalInput * moveSpeed - rb.velocity.x, 0f) * moveForce);

        // Apply jump
        if (isGrounded && verticalInput > 0)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void OnPlayerDie(Vector2 spawnPoint, List<Vector2> positionHistory) {
        rb.velocity = Vector2.zero;
        rb.position = spawnPoint;
    }
}
