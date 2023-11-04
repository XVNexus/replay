using UnityEngine;

public class KillzoneController : MonoBehaviour
{
    // If a live player (not a ghost) enters a kill zone, reset them and spawn a new ghost replay
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<PlayerController>().Kill();
        }
    }
}
