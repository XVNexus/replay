using UnityEngine;

public class StarController : MonoBehaviour
{
    // When the player collects this star, emit an event and delete the object
    public void OnTriggerEnter2D()
    {
        EventSystem.current.TriggerStarCollected();
        Destroy(gameObject);
    }
}
