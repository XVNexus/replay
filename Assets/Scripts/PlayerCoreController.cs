using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    readonly Color COLOR_MAIN = new(1f, .75f, .25f);
    readonly Color COLOR_GHOST = new(.25f, .75f, 1f);

    [Header("General")]
    public Vector2 spawnPoint = new(-8f, 0f);
    public bool ghostMode = false;

    [Header("References")]
    public SpriteRenderer spriteRendererComponent;
    public Light2D light2dComponent;

    private List<Vector2> positionHistory = new();

    // Start is called before the first frame update
    void Start()
    {
        // Figure out which color to use
        var color = ghostMode ? COLOR_GHOST : COLOR_MAIN;

        // Apply that color to the square and light
        spriteRendererComponent.color = color;
        light2dComponent.color = color;

        // If the player is a ghost, disable gravity and movement
        if (ghostMode)
        {
            GetComponent<Rigidbody2D>().simulated = false;
        }
        // If the player is not a ghost, add the movement controller script
        else
        {
            var _ = gameObject.AddComponent(typeof(PlayerMovement)) as PlayerMovement;
        }
    }

    // Add the current position to the history every physics update
    public void FixedUpdate() {
        positionHistory.Add(transform.position);
    }

    // Reset the player back to the spawn point
    public void Kill()
    {
        EventSystem.current.TriggerPlayerDie(spawnPoint, positionHistory);
    }
}
