using Unity.VisualScripting;
using UnityEngine;

public class StarController : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem collectParticles;

    // When the player collects this star, hide it and emit an event
    public void OnTriggerEnter2D()
    {
        EventSystem.current.TriggerStarCollected();
        collectParticles.Play();
        DisableStar();
    }

    // Delete the renderer and collider to convert this object into a placeholder
    public void DisableStar()
    {
        Destroy(GetComponent<SpriteRenderer>());
        Destroy(GetComponent<BoxCollider2D>());
    }
}
