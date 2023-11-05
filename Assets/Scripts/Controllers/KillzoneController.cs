using UnityEngine;

public class KillzoneController : MonoBehaviour
{
    [Header("General")]
    public bool destroyOnContact = false;

    // If a live player (not a ghost) enters a kill zone, reset them and spawn a new ghost replay
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<PlayerController>().Kill();

            // If the destroy on contact switch is enabled, destroy this killzone
            if (destroyOnContact)
            {
                Destroy(gameObject);
            }
        }
    }
}
