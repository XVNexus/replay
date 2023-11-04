using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float moveForce = 3f;
    public float jumpForce = 10f;

    public Vector2 spawnPoint = new(-8f, 0f);

    private Rigidbody2D rb;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check for ground contact to allow jumping
        isGrounded = Physics2D.Raycast(transform.position - new Vector3(0f, 0.51f), Vector2.down, 0.1f);

        // Player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.AddForce(new Vector2(horizontalInput * moveSpeed - rb.velocity.x, 0f) * moveForce);

        // Jumping
        Debug.Log(isGrounded);
        if (isGrounded && verticalInput > 0)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
